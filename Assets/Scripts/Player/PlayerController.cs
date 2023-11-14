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

    private bool _scrapShopOpen = false;

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



    private void SetUpInputAction()
    {
        if (_inputActions != null)
            return;

        _inputActions = new MInputActionAsset();
        _inputActions.Player.OpenScrapShop.performed += OpenScrapShop_performed;
        _inputActions.Enable();
    }

    private void OpenScrapShop_performed(InputAction.CallbackContext context)
    {
        _scrapShopOpen = !_scrapShopOpen;
        UIManager.Instance.OpenScrapShop(_scrapShopOpen);
    }

    private void OnDisable()
    {
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

