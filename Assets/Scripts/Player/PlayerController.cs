using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rayqdr.Input;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _explosionParticles;

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
    private HealthScript _healthScript;

    private bool _scrapShopOpen = false;

    private void Awake()
    {
        _rotation = GetComponent<PlayerRotation>();
        _playerModule = GetComponent<PlayerModule>();
        _healthScript = GetComponent<HealthScript>();
        SetUpInputAction();

        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
        _healthScript.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        //Game Over !
        GameManager.Instance.ChangeGameState(GameState.GameOver);

        //Spawn Particles
        Instantiate(_explosionParticles,this.transform.position,Quaternion.identity);
        Destroy(gameObject);
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

