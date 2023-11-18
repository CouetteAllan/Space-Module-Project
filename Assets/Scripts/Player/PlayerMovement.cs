using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rayqdr.Input;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float _maxSpeed = 12.0f;
    [SerializeField] private float _minSpeed = 6.0f;
    [SerializeField] private AnimationCurve _accelerationCurve;
    [SerializeField] private AnimationCurve _decelerationCurve;

    private AnimationCurve _selectedCurve;
    private float _speed = 12.0f;

    private PlayerController _playerController;
    private Rigidbody2D _rigidbody;
    private MInputActionAsset _inputActions;

    private bool _isMoving = false;
    private Vector2 _input
    {
        get
        {
            if (_inputActions == null)
                return Vector2.zero;

            var input = _inputActions.Player.Move.ReadValue<Vector2>();
            if (Mathf.Abs(input.x) > 0.01f || Mathf.Abs(input.y) > 0.01f)
                return input;
            return Vector2.zero;
        }
    }
    private Vector2 _lastInput = Vector2.zero;

    private float _timerCurve = 0.0f;

    private void Awake()
    {
        _speed = _maxSpeed;

        _playerController = this.GetComponent<PlayerController>();
        _rigidbody = this.transform.GetComponent<Rigidbody2D>();

        _inputActions = _playerController.InputActions;
        _inputActions.Player.Move.started += Move_started;
        _inputActions.Player.Move.performed += Move_performed;
        _inputActions.Player.Move.canceled += Move_canceled;
        _selectedCurve = _decelerationCurve;

        DropModule.OnModuleAttached += DropModule_OnModuleAttached;
    }

    private void DropModule_OnModuleAttached(Module mod)
    {
        CalculateSpeedRate();
    }

    private void Move_started(InputAction.CallbackContext obj)
    {
        _isMoving = true;
        _timerCurve = 0.0f;
        _selectedCurve = _accelerationCurve;

    }

    private void Move_canceled(InputAction.CallbackContext obj)
    {
        _isMoving = false;
        _timerCurve = 0.0f;
        _selectedCurve = _decelerationCurve;
    }

    private void Move_performed(InputAction.CallbackContext context)
    {
    }

    private void Update()
    {
        _timerCurve += Time.deltaTime;

        var input = _inputActions.Player.Move.ReadValue<Vector2>();
        if (Mathf.Abs(input.x) > 0.1f || Mathf.Abs(input.y) > 0.1f)
            _lastInput = input;
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        Vector2 targetMovement = _input;
        if(_isMoving)
            _rigidbody.velocity = new Vector2(targetMovement.x * _selectedCurve.Evaluate(_timerCurve) * _speed, targetMovement.y * _selectedCurve.Evaluate(_timerCurve) * _speed);
        else
            _rigidbody.velocity = new Vector2(_lastInput.x * _selectedCurve.Evaluate(_timerCurve) * _speed, _lastInput.y * _selectedCurve.Evaluate(_timerCurve) * _speed);
    }

    private void CalculateSpeedRate()
    {
        //call this calculation when any module is placed
        float currentMaxWeight = StatSystem.Instance.PlayerStat.GetStatValue(StatType.MaxWeight);
        float currentWeight = StatSystem.Instance.PlayerStat.GetStatValue(StatType.Weight);

        var weightRatio = Mathf.Clamp(currentWeight / currentMaxWeight, 0.0f, 1.0f);
        _speed = Mathf.Lerp(_maxSpeed, _minSpeed, weightRatio);
    }
}