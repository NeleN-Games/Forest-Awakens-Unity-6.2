using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementInputHandler : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset playerInputAction;
    private InputAction _moveAction;
    private Vector2 _moveInput;
    private Vector2 _lastMoveInput;
    private Vector2 _cashedMoveDirection=Vector2.zero;
    private Vector2 _lastNonZeroDirection=Vector2.zero;
    
    [Header("Movement")]
    [SerializeField]
    private float baseSpeed;
    [SerializeField] 
    private Rigidbody rigidBody; 
    [SerializeField] 
    private Transform cameraTransform;
    
    [SerializeField]
    private float accelerationTime = 3f;
    [SerializeField]
    private float decelerationTime = 1f;
    
    [SerializeField]
    private float maxSpeedMultiplier = 1.15f;
    private float _currentSpeedMultiplier = 0f;
    private float _targetSpeedMultiplier = 0f;
    private void Awake()
    {
        InitializeMoveActions();
    }
    private void OnDisable()
    {
        _moveAction.Disable();
    }
    private void InitializeMoveActions()
    {
        try
        {
            var actionMap = playerInputAction.FindActionMap("Movement");
            _moveAction = actionMap["Walking"];
            _moveAction.Enable();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        } 
    }

    private void FixedUpdate()
    {
        _moveInput = _moveAction.ReadValue<Vector2>(); 
        UpdateSpeedMultiplier();
        Move();
    }

    private void UpdateSpeedMultiplier()
    {
        if (_moveInput!= Vector2.zero)
        {
            _targetSpeedMultiplier = maxSpeedMultiplier;
            _currentSpeedMultiplier= Mathf.Lerp(_currentSpeedMultiplier,
                _targetSpeedMultiplier, 
                Time.fixedDeltaTime / accelerationTime);
        }
        else
        {
            _targetSpeedMultiplier = 0;
            _currentSpeedMultiplier=Mathf.Lerp(_currentSpeedMultiplier,
                _targetSpeedMultiplier,
                Time.fixedDeltaTime / decelerationTime);
        }
    }
    private void Move()
    {
        if (_moveInput != Vector2.zero)
        {
            if (_lastMoveInput != _moveInput)
            {
                _cashedMoveDirection = _moveInput.normalized;
                _lastNonZeroDirection = _cashedMoveDirection;
            }
            _lastMoveInput = _moveInput;
        }
        else
        {
            _cashedMoveDirection = _currentSpeedMultiplier > 0.05f ?
                _lastNonZeroDirection :
                Vector2.Lerp(_cashedMoveDirection, Vector2.zero, Time.fixedDeltaTime * 20f);
        }
        
        /* Old version: fixed 45-degree movement relative to world axes
        Vector3 rawMove = new Vector3(_cashedMoveDirection.x, 0f, _cashedMoveDirection.y);
        Quaternion rotation = Quaternion.Euler(0f, 45f, 0f);
        Vector3 moveDirection = rotation * rawMove;
        rigidBody.linearVelocity = moveDirection * (baseSpeed * _currentSpeedMultiplier);
        */
        
        // New version: movement relative to camera forward (pressing 'up' always moves player forward relative to camera)

        Vector3 forward = cameraTransform.forward;
        Vector3 right =cameraTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = _cashedMoveDirection.y * forward + _cashedMoveDirection.x * right;

        rigidBody.linearVelocity = moveDir * (baseSpeed * _currentSpeedMultiplier);
        
        // Old System for moving along x, y axis
        
        /*if (_moveInput != Vector2.zero)
        {            
            if (_lastMoveInput != _moveInput)
            {
                _cashedMoveDirection = _moveInput.normalized;
                _lastNonZeroDirection = _cashedMoveDirection;
            }
            _lastMoveInput = _moveInput;
        }
        else
        {
            _cashedMoveDirection = _currentSpeedMultiplier > 0.05f ?
                _lastNonZeroDirection :
                Vector2.Lerp(_cashedMoveDirection, Vector2.zero, Time.fixedDeltaTime * 20f);
        }
        rigidBody.linearVelocity = _cashedMoveDirection * (baseSpeed*_currentSpeedMultiplier);*/
    }
}
