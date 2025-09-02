using DG.Tweening;
using Enums;
using Unity.Cinemachine;
using UnityEngine;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        [Header("References")]
        public CinemachineCamera virtualCam;

        [Header("Rotation Settings")]
        [SerializeField] private float rotateTransitionTime = 0.7f;
        private bool _isRotating;

        [Header("Zoom Settings")]
        [SerializeField] private float zoomTransitionTime = 0.3f;
        [SerializeField] private float zoomSpeed = 5f;      
        [SerializeField] private float minSize = 5f;       
        [SerializeField] private float maxSize = 15f;       
        [SerializeField] private float minHeight = 7f;      
        [SerializeField] private float maxHeight = 12f;     

        private CinemachineFollow _follow;
        
        [Header("Panning Settings")]
        [SerializeField] private float panSpeed = 10f; 
        [SerializeField] private float panBorderThickness = 10f;

        private CameraMode _cameraMode = CameraMode.FollowPlayer;
        
        private bool _isChangingCameraMode;
        private readonly Vector3[] _offsets = new Vector3[]
        {
            new Vector3(-7.5f, 7f, -7.5f),
            new Vector3(-7.5f, 7f, 7.5f),
            new Vector3(7.5f, 7f, 7.5f),
            new Vector3(7.5f, 7f, -7.5f),
        };

        private readonly Vector3[] _rotations = new Vector3[]
        {
            new Vector3(30f, 45f, 0f),
            new Vector3(30f, 135f, 0f),
            new Vector3(30f, 225f, 0f),
            new Vector3(30f, 315f, 0f),
        };

        private int _currentIndex = 0;
        
        [ContextMenu("Toggle Camera Mode")]
        public void ToggleCameraMode()
        {
            if (_cameraMode == CameraMode.FreeCamera)
            {
                _isChangingCameraMode = true;
              
                Vector3 targetPos = virtualCam.Follow.transform.position + _follow.FollowOffset;
                Debug.Log(targetPos);
                DOTween.Kill(virtualCam.transform);
                virtualCam.transform.DOMove(targetPos, 1f).SetEase(Ease.Linear)
                    .OnComplete(() =>  
                    { 
                        _follow.enabled = true; 
                        _cameraMode = CameraMode.FollowPlayer;
                        _isChangingCameraMode = false;
                    });
            }
            else
            {
                _cameraMode = CameraMode.FreeCamera;
                _follow.enabled = false;
            }
        }
        
        void Start()
        {
            _follow = virtualCam.GetComponent<CinemachineFollow>();
            SetFollowOffset(_currentIndex, virtualCam.Lens.OrthographicSize,rotateTransitionTime);
            virtualCam.transform.rotation = Quaternion.Euler(_rotations[_currentIndex]);
        }

        void Update()
        {
            if (_isChangingCameraMode) return;
            HandleZoom();

            if (_cameraMode == CameraMode.FollowPlayer)
            {
                _follow.enabled = true;
                HandleRotation();
            }
            else if (_cameraMode == CameraMode.FreeCamera)
            {
                _follow.enabled = false;
                HandleEdgePan();
            }
         
        }

        private void HandleZoom()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                float targetSize = virtualCam.Lens.OrthographicSize - scroll * zoomSpeed;
                targetSize = Mathf.Clamp(targetSize, minSize, maxSize);

                DOTween.To(() => virtualCam.Lens.OrthographicSize,
                        x => virtualCam.Lens.OrthographicSize = x,
                        targetSize,
                        0.2f)
                    .SetEase(Ease.OutSine);
                SetFollowOffset(_currentIndex, targetSize,zoomTransitionTime);
            }
        }
        private void HandleRotation()
        {
            if (!_isRotating)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    _currentIndex = (_currentIndex + 1) % _offsets.Length;
                    RotateToView(_currentIndex);
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    _currentIndex = (_currentIndex - 1 + _offsets.Length) % _offsets.Length;
                    RotateToView(_currentIndex);
                }
            }
        }
     
        private void HandleEdgePan()
        {
            Vector3 move = Vector3.zero;

            float mouseX = Input.mousePosition.x;
            float mouseY = Input.mousePosition.y;

            if (mouseX >= Screen.width - panBorderThickness)
                move += virtualCam.transform.right;
            else if (mouseX <= panBorderThickness)
                move -= virtualCam.transform.right;

            if (mouseY >= Screen.height - panBorderThickness)
                move += Vector3.ProjectOnPlane(virtualCam.transform.forward, Vector3.up);
            else if (mouseY <= panBorderThickness)
                move -= Vector3.ProjectOnPlane(virtualCam.transform.forward, Vector3.up);

            virtualCam.transform.position += move.normalized * (panSpeed * Time.deltaTime);
        }


        void RotateToView(int index)
        {
            _isRotating = true;
            DOTween.Kill(_follow);

            SetFollowOffset(index, virtualCam.Lens.OrthographicSize,rotateTransitionTime);

            float currentY = virtualCam.transform.eulerAngles.y;
            float targetY = _rotations[index].y;
            float delta = Mathf.DeltaAngle(currentY, targetY);

            virtualCam.transform
                .DORotate(new Vector3(30f, currentY + delta, 0f), rotateTransitionTime)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => _isRotating = false);
        }

        private void SetFollowOffset(int index, float orthographicSize,float transitionTime)
        {
            float t = (orthographicSize - minSize) / (maxSize - minSize);

            float newY = Mathf.Lerp(minHeight, maxHeight, t);

            Vector3 baseOffset = _offsets[index];
            float distance = newY + 0.5f;

            Vector3 offset = new Vector3(Mathf.Sign(baseOffset.x) * distance,
                newY,
                Mathf.Sign(baseOffset.z) * distance);

            DOTween.To(() => _follow.FollowOffset,
                    x => _follow.FollowOffset = x,
                    offset,
                    transitionTime)
                .SetEase(Ease.InOutSine);
        }


    }
}
