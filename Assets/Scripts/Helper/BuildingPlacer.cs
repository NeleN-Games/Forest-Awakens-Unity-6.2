using System;
using Managers;
using Models.Data;
using UnityEngine;

namespace Helper
{
    public class BuildingPlacer : MonoBehaviour
    {
        private static readonly int Surface = Shader.PropertyToID("_Surface");
        private static readonly int AlphaClip = Shader.PropertyToID("_AlphaClip");
        private static readonly int DstBlend = Shader.PropertyToID("_DstBlend");
        [SerializeField] private Camera camera;
        [SerializeField] private BuildingCrafter craftManager;
        public GameObject previewPrefab;
        public BuildingData currentBuildingData;
        [SerializeField] private LayerMask groundMask;
        private bool _isPlacing = false;
        private BoxCollider _cashedCollider;
        private readonly Collider[] _overlapResults = new Collider[3];
        [SerializeField] private Color placeableColor;
        [SerializeField] private Color notPlaceableColor;
        public void StartPlacing(BuildingData data)
        {
            currentBuildingData = data;
            previewPrefab = Instantiate(data.prefab);
            _cashedCollider = previewPrefab.GetComponentInChildren<BoxCollider>();
            _cashedCollider.isTrigger = true;
            var material = previewPrefab.GetComponentInChildren<MeshRenderer>().material;
            material.SetFloat(Surface, 1f);      
            material.SetFloat(AlphaClip, 0f);    
            material.SetInt(DstBlend, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            _isPlacing = true;
        }

        void Update()
        {
            if (!_isPlacing) return;
            if (Input.GetMouseButtonDown(1))
            {
                ForgetPlacing();
                return;
            }


            if (!Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100f,
                    groundMask)) return;
            float margin = 0.01f;
            Vector3 halfExtents = _cashedCollider.size * 0.5f - Vector3.one * margin;
            //Vector3 halfExtents = _cashedCollider.size * 0.5f;
            var addHalfPointX = currentBuildingData.buildingSize.x % 2==0;
            var addHalfPointZ = currentBuildingData.buildingSize.y % 2==0;
            Vector3 pos = new Vector3(Mathf.Round(hit.point.x)+ (addHalfPointX ? 0f : 0.5f),
                _cashedCollider.size.y*0.5f, 
                Mathf.Round(hit.point.z)+(addHalfPointZ ? 0f : 0.5f));
            previewPrefab.transform.position = pos;
            
            int hitCount = Physics.OverlapBoxNonAlloc(
                pos,
                halfExtents,
                _overlapResults,
                Quaternion.identity,
                ~LayerMask.GetMask("Ground") // ignore ground
            );
            bool canBuild = true;
            for (int i = 0; i < hitCount; i++)
            {
                if (_overlapResults[i].gameObject == previewPrefab) continue; 
                canBuild = false;
                break;
            }
            SetPreviewColor(canBuild);

            if (canBuild && Input.GetMouseButtonDown(0))
            {
                Place();
            }

        }

        private void OnDrawGizmos()
        {
            if (previewPrefab==null) return;
          
            Gizmos.DrawCube( previewPrefab.transform.position +Vector3.up * (_cashedCollider.size.y * 0.5f),new Vector3(currentBuildingData.buildingSize.x,1,currentBuildingData.buildingSize.y));
            Gizmos.color = Color.green;
        }

        private void Place()
        {
            craftManager.OnPlacingBuilding?.Invoke(currentBuildingData,previewPrefab.transform.position);
            _isPlacing = false;
            currentBuildingData = null;
            _cashedCollider = null;
            Destroy(previewPrefab);
            previewPrefab = null;
        }  
        private void ForgetPlacing()
        {
            _isPlacing = false;
            currentBuildingData = null;
            _cashedCollider = null;
            Destroy(previewPrefab);
            previewPrefab = null;
        }
        private void SetPreviewColor(bool canBuild)
        {
           previewPrefab.GetComponent<MeshRenderer>().material.color=canBuild?placeableColor:notPlaceableColor;
        }
    }
}
