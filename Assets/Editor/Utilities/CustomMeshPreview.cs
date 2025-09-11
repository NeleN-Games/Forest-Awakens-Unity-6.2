using UnityEditor;
using UnityEngine;

namespace Editor.Utilities
{
    public class CustomMeshPreview
    {
        private PreviewRenderUtility _previewUtility;
        private RenderTexture _previewTexture;

        private Vector2 _previewDrag;
        private float _previewZoom;
        private const float DefaultZoom = 1.5f;

        private Mesh _currentMesh;
        private Material _currentMaterial;

        private bool _previewDirty = true;

        private Vector2 _previewPan; // Pan vector
        private const float PanLimit = 3f; // Maximum pan offset
        private Bounds _meshBounds; // store mesh bounds for dynamic zoom

        public CustomMeshPreview()
        {
            _previewUtility = new PreviewRenderUtility(true);
            _previewUtility.cameraFieldOfView = 30f;
            _previewUtility.camera.nearClipPlane = 0.001f;
            _previewUtility.camera.farClipPlane = 100f;

            _previewUtility.lights[0].intensity = 1f;
            _previewUtility.lights[0].transform.rotation = Quaternion.Euler(30f, 30f, 0f);
            _previewUtility.lights[1].intensity = 1f;

            _previewZoom = DefaultZoom;
        }

        public void Cleanup()
        {
            if (_previewTexture != null)
            {
                _previewTexture.Release();
                Object.DestroyImmediate(_previewTexture);
                _previewTexture = null;
            }

            if (_previewUtility != null)
            {
                _previewUtility.Cleanup();
                _previewUtility = null;
            }
        }

        private void ResetView()
        {
            _previewDrag = Vector2.zero;
            _previewPan = Vector2.zero;
            _previewZoom = DefaultZoom;
            _previewDirty = true;
          
            if (_currentMesh != null)
            {
                _meshBounds = _currentMesh.bounds;
                float maxSize = Mathf.Max(_meshBounds.size.x, _meshBounds.size.y, _meshBounds.size.z);
                _previewZoom = Mathf.Max(0.5f, maxSize * 1.5f); // dynamic zoom based on mesh size
            }
            else
            {
                _previewZoom = DefaultZoom;
            }

            _previewDirty = true;
        }

        public void SetMesh(Mesh mesh, Material mat)
        {
            if (_currentMesh != mesh || _currentMaterial != mat)
            {
                _currentMesh = mesh;
                _currentMaterial = mat;
                _previewDirty = true;

                if (_currentMesh != null)
                {
                    _meshBounds = _currentMesh.bounds;

                    // Automatically reset view when new mesh is set
                    ResetView();
                }
            }
        }

        public void DrawPreview(Rect previewRect)
        {
            if (_currentMesh == null || _currentMaterial == null) return;

            HandleInput(previewRect);

            // Ensure RenderTexture exists
            if (_previewTexture == null)
            {
                _previewTexture = new RenderTexture(Mathf.Max(1, (int)previewRect.width),
                                                   Mathf.Max(1, (int)previewRect.height),
                                                   16, RenderTextureFormat.ARGB32);
                _previewTexture.Create();
                _previewDirty = true;
            }

            // Render only if dirty
            if (_previewDirty)
            {
                Vector3 center = _meshBounds.center;
                float distance = _previewZoom; // already scaled dynamically

               
                Quaternion orbitRotation = Quaternion.Euler(-_previewDrag.y, -_previewDrag.x, 0f);
                Vector3 orbitPosition = orbitRotation * new Vector3(_previewPan.x, -_previewPan.y, -distance) + center;
                
                var cam = _previewUtility.camera;
                cam.transform.position = orbitPosition;
                cam.transform.rotation = orbitRotation;
                cam.targetTexture = _previewTexture;
                cam.clearFlags = CameraClearFlags.Color;
                cam.backgroundColor = new Color(0.12f, 0.12f, 0.15f);

                // MANUAL LIGHT SETUP
                for (int i = 0; i < _previewUtility.lights.Length; i++)
                {
                    var light = _previewUtility.lights[i];
                    light.enabled = true;
                    light.intensity = i == 0 ? 2f : 1f; // adjust intensities if needed
                }

                // Draw the mesh
                _previewUtility.DrawMesh(_currentMesh, Matrix4x4.identity, _currentMaterial, 0);

                cam.Render();
                cam.targetTexture = null;

                _previewDirty = false;
            }

            GUI.DrawTexture(previewRect, _previewTexture, ScaleMode.ScaleToFit, false);
            GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("🔄 Reset View", GUILayout.Width(110)))
            {
                ResetView();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

        }
        


        private void HandleInput(Rect rect)
        {
            if (!rect.Contains(Event.current.mousePosition)) return;

            // Rotate with left mouse
            if (Event.current.type == EventType.MouseDrag && Event.current.button == 0)
            {
                _previewDrag += Event.current.delta;
                _previewDirty = true;
                Event.current.Use();
            }

            // Pan with right mouse
            if (Event.current.type == EventType.MouseDrag && Event.current.button == 1)
            {
                _previewPan += -Event.current.delta * (0.001f * _previewZoom); // scale pan by zoom
                Vector2 dynamicPanLimit = GetDynamicPanLimit();
                _previewPan.x = Mathf.Clamp(_previewPan.x, -dynamicPanLimit.x, dynamicPanLimit.x);
                _previewPan.y = Mathf.Clamp(_previewPan.y, -dynamicPanLimit.y, dynamicPanLimit.y);
                _previewDirty = true;
                Event.current.Use();
            }

            // Zoom with scroll
            if (Event.current.type == EventType.ScrollWheel)
            {
                _previewZoom -= -Event.current.delta.y * 0.035f;
                _previewZoom = Mathf.Clamp(_previewZoom, 0.03f, 5f);
                _previewDirty = true;
                Event.current.Use();
            }

            _previewDrag.y = Mathf.Clamp(_previewDrag.y, -80f, 80f);
        }
        private Vector2 GetDynamicPanLimit()
        {
            if (_currentMesh == null) return new Vector2(PanLimit, PanLimit * 0.75f);

            float maxSize = Mathf.Max(_meshBounds.size.x, _meshBounds.size.y, _meshBounds.size.z);
            float scaleFactor = _previewZoom / DefaultZoom;

            return new Vector2(maxSize * 2f * scaleFactor, maxSize * 1.5f * scaleFactor);
        }

    }
}
