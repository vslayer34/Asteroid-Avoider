using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Camera _mainCamera;

    private Vector3 _screenTouchPosition;
    private Vector3 _worldTouchPosition;


    // Ship Movment
    private Vector3 _movementDirection;

    [SerializeField, Tooltip("Maximum velocity that the ship can't excede")]
    private float _maxVelocity;

    [SerializeField, Tooltip("force magnitute to be applied to the ship")]
    private float _forceMagnitute;


    // Handle spawning at screen bounds
    private Vector3 _newPosition;
    private Vector3 _viewPortPosition;


    private Rigidbody _rb;


    private void Start()
    {
        _mainCamera = Camera.main;
        EnhancedTouchSupport.Enable();
        _rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        ProcessInput();

        KeepPlayerOnScreen();
    }


    private void FixedUpdate()
    {
        if (_movementDirection.Equals(Vector3.zero))
        {
            return;
        }

        _rb.AddForce(_movementDirection * _forceMagnitute, ForceMode.Force);

        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxVelocity);
    }


    private void ProcessInput()
    {
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            _screenTouchPosition = Touchscreen.current.position.ReadValue();
            _worldTouchPosition = _mainCamera.ScreenToWorldPoint(_screenTouchPosition);

            _movementDirection = (transform.position - _worldTouchPosition).normalized;
            _movementDirection.z = 0.0f;
        }
        else
        {
            _movementDirection = Vector3.zero;
        }
    }


    private void KeepPlayerOnScreen()
    {
        float teleportationOffset = 5.0f;
        _newPosition = transform.position;

        // calculate view port bounds
        _viewPortPosition = _mainCamera.WorldToViewportPoint(transform.position);

        if (_viewPortPosition.x > 1)
        {
            _newPosition.x = -_newPosition.x + teleportationOffset;
        }
        else if (_viewPortPosition.x <= 0)
        {
            _newPosition.x = -_newPosition.x - teleportationOffset;
        }
        
        if (_viewPortPosition.y > 1)
        {
            _newPosition.y = -_newPosition.y + teleportationOffset;
            Debug.Log(_newPosition.y);
        }
        else if (_viewPortPosition.y <= 0)
        {
            _newPosition.y = -_newPosition.y - teleportationOffset;
        }

        transform.position = _newPosition;
    }
}
