using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public enum GameState
{
    MainMenu,
    BeforeGameStart,
    StartGame,
    InGame,
    GameOver,
    ShopState
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    public static event Action<GameState> OnGameStateChanged;
    public static event Action<uint> OnLevelUp;
    public GameState CurrentState { get; private set; }
    public uint CurrentLevel { get; private set; } = 1;
    public uint CurrentXP { get; private set; } = 0;
    public uint NextTresholdLevelUp { get; private set; } = 10;
    public PlayerController PlayerController { get; private set; }

    private void Start()
    {
        ChangeGameState(GameState.BeforeGameStart);
    }

    public void ChangeGameState(GameState newState)
    {
        if (newState == CurrentState)
            return;

        CurrentState = newState;
        switch (CurrentState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1.0f;
                break;
            case GameState.StartGame:
                Time.timeScale = 1.0f;

                OpenShop();
                break;
            case GameState.InGame:
                Time.timeScale = 1.0f;
                break;
            case GameState.GameOver:
                Time.timeScale = 0.2f;
                break;
            case GameState.ShopState:
                Time.timeScale = 0.0f;
                OpenShop();
                break;
        }
        OnGameStateChanged?.Invoke(newState);
        Debug.Log("Game State: " + CurrentState.ToString());
    }

    public void GrantXP(uint xp)
    {
        CurrentXP += xp;
        UIManager.Instance.UpdateXpBar(CurrentXP);
        if(CurrentXP >= NextTresholdLevelUp)
        {
            LevelUp();
            UIManager.Instance.UpdateLevel(CurrentLevel);
        }
    }

    private void LevelUp()
    {
        CurrentLevel++;
        _virtualCamera.m_Lens.OrthographicSize += 0.2f;
        OnLevelUp?.Invoke(CurrentLevel);

        CurrentXP -= NextTresholdLevelUp;
        NextTresholdLevelUp += 20;
        ChangeGameState(GameState.ShopState);
    }

    public void OpenShop()
    {
        UIManager.Instance.OpenShop();
    }

    public void CloseShop()
    {
        ChangeGameState(GameState.InGame);
        //jouer son
        //close UI
        UIManager.Instance.CloseShop();
    }

    public void SetPlayer(PlayerController player)
    {
         this.PlayerController = player;
    }

    private void Update()
    {
        if(Keyboard.current.cKey.wasPressedThisFrame)
        {
            GrantXP(5);
        }
    }

}
