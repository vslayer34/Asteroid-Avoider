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



    private Rigidbody _rb;


    private void Start()
    {
        _mainCamera = Camera.main;
        EnhancedTouchSupport.Enable();
        _rb = GetComponent<Rigidbody>();
    }


    private void Update()
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


    private void FixedUpdate()
    {
        if (_movementDirection.Equals(Vector3.zero))
        {
            return;
        }

        _rb.AddForce(_movementDirection * _forceMagnitute * Time.deltaTime, ForceMode.Force);
        Debug.Log(_rb.velocity);

        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxVelocity);
    }


    private void GetTouchPosition()
    {
        
    }
}
