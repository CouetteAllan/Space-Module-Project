using Rayqdr.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private float _rotationRate = 180.0f; //5 degré par seconde

    private PlayerController _playerController;
    private MInputActionAsset _inputActions;
    private Rigidbody2D _rigidbody;

    private float _rotDir = 1.0f;
    private bool _isRotating = false;


    private void Awake()
    {
        _playerController = this.GetComponent<PlayerController>();
        _rigidbody = this.GetComponent<Rigidbody2D>();

        _inputActions = _playerController.InputActions;
        _inputActions.Player.Rotate.performed += Rotate_performed;
        _inputActions.Player.Rotate.canceled += Rotate_canceled; ;
    }

    private void Rotate_canceled(InputAction.CallbackContext obj)
    {
        _isRotating = false;
    }

    private void Rotate_performed(InputAction.CallbackContext obj)
    {
        _rotDir = -_inputActions.Player.Rotate.ReadValue<Vector2>().x;
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
        _rigidbody.MoveRotation(_rigidbody.rotation + _rotationRate *_rotDir * Time.fixedDeltaTime);
    }
    
}
