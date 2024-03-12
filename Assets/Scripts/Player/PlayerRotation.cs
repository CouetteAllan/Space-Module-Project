using Rayqdr.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private float _minRotationRate = 55.0f;
    [SerializeField] private float _maxRotationRate = 190.0f;
    private float _rotationRate = 190.0f;


    private PlayerController _playerController;
    private MInputActionAsset _inputActions;
    private Rigidbody2D _rigidbody;

    private float _rotDir = 1.0f;
    private bool _isRotating = false;


    private void Awake()
    {
        _rotationRate = _maxRotationRate;

        _playerController = this.GetComponent<PlayerController>();
        _rigidbody = this.transform.GetComponent<Rigidbody2D>();

        _inputActions = _playerController.InputActions;
        _inputActions.Player.Rotate.performed += Rotate_performed;
        _inputActions.Player.Rotate.canceled += Rotate_canceled;

        DropModuleOnCanvas.OnModuleAttached += DropModule_OnModuleAttached;
        Module.OnModuleDestroyed += Module_OnModuleDestroyed;

    }

    private void OnDisable()
    {
        _inputActions.Player.Rotate.performed -= Rotate_performed;
        _inputActions.Player.Rotate.canceled -= Rotate_canceled;

        DropModuleOnCanvas.OnModuleAttached -= DropModule_OnModuleAttached;
        Module.OnModuleDestroyed -= Module_OnModuleDestroyed;
    }
    private void Module_OnModuleDestroyed(Module mod)
    {
        CalculateRotationRate();
    }

    private void DropModule_OnModuleAttached(Module mod)
    {
        CalculateRotationRate();
    }

    private void Rotate_canceled(InputAction.CallbackContext obj)
    {
        _isRotating = false;
    }

    private void Rotate_performed(InputAction.CallbackContext context)
    {
        /*if (UIManager.Instance.IsScrapShopOpen())
        {
            UIManager.Instance.ChangeScrapTab(_inputActions.Player.Rotate.ReadValue<float>() > 0);
        }
        else*/ if (GameManager.Instance.CurrentState == GameState.InGame && !UIManager.Instance.IsScrapShopOpen())
        {
            _rotDir = -_inputActions.Player.Rotate.ReadValue<float>();
            _isRotating = true;
        }
        
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

    private void CalculateRotationRate()
    {
        //call this calculation when any module is placed
        if (StatSystem.Instance == null)
            return;
        float currentMaxWeight = StatSystem.Instance.PlayerStat.GetStatValue(StatType.MaxWeight);
        float currentWeight = StatSystem.Instance.PlayerStat.GetStatValue(StatType.Weight);

        var weightRatio = Mathf.Clamp(currentWeight / currentMaxWeight, 0.0f, 1.0f);   
        _rotationRate = Mathf.Lerp(_maxRotationRate, _minRotationRate, weightRatio);
    }
    
}
