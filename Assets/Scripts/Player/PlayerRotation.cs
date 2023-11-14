using Rayqdr.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private float _rotationRate = 180.0f; //5 degr� par seconde

    private PlayerController _playerController;
    private MInputActionAsset _inputActions;
    private Rigidbody2D _rigidbody;

    private float _rotDir = 1.0f;
    private bool _isRotating = false;


    private void Awake()
    {
        _playerController = this.GetComponent<PlayerController>();
        _rigidbody = this.transform.GetComponent<Rigidbody2D>();

        _inputActions = _playerController.InputActions;
        _inputActions.Player.Rotate.performed += Rotate_performed;
        _inputActions.Player.Rotate.canceled += Rotate_canceled; ;
    }

    private void Rotate_canceled(InputAction.CallbackContext obj)
    {
        _isRotating = false;
    }

    private void Rotate_performed(InputAction.CallbackContext context)
    {
        _rotDir = - _inputActions.Player.Rotate.ReadValue<float>();
        _isRotating = true;
    }

    private void FixedUpdate()
    {
        if (_isRotating)
        {
            PerformRotation();
        }
    }

    private void PerformRotation()
    {
        if (_rigidbody.centerOfMass != Vector2.zero)
        {
            _rigidbody.centerOfMass = Vector2.zero;
        }
        _rigidbody.MoveRotation(_rigidbody.rotation + _rotationRate *_rotDir * Time.fixedDeltaTime);
    }
    
}
