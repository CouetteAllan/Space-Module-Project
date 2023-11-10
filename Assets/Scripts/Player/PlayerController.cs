using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rayqdr.Input;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;

public class PlayerController : MonoBehaviour
{
    public static event Action OnFire;
    public MInputActionAsset InputActions {
        get
        {
            SetUpInputAction();
            return _inputActions;
        }
    }
    private MInputActionAsset _inputActions;
    private PlayerRotation _rotation;
    private PlayerModule _playerModule;
    


    private void Awake()
    {
        _rotation = GetComponent<PlayerRotation>();
        _playerModule = GetComponent<PlayerModule>();
        SetUpInputAction();
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }
    private void Start()
    {
        GameManager.Instance.SetPlayer(this);
    }

    private void GameManager_OnGameStateChanged(GameState newState)
    {
        if(newState == GameState.StartGame)
        {
            GameManager.Instance.SetPlayer(this);
        }
    }

    private void Fire_performed(InputAction.CallbackContext obj)
    {
        OnFire?.Invoke();
    }


    private void SetUpInputAction()
    {
        if (_inputActions != null)
            return;

        _inputActions = new MInputActionAsset();
        _inputActions.Player.Fire.performed += Fire_performed;
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Fire.performed -= Fire_performed;
        _inputActions = null;
    }

    public PlayerModule GetPlayerModule()
    {
        return _playerModule;
    }

    public void GatherScrapMetal(int value)
    {
        ScrapManagerDataHandler.PickUpScrap(value);
    }
}

