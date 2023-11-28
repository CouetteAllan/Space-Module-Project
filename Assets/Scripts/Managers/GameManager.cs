using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.SceneManagement;

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
    public GameState CurrentState { get; private set; } = GameState.GameOver;
    public uint CurrentLevel { get; private set; } = 1;
    public uint CurrentXP { get; private set; } = 0;
    public uint NextTresholdLevelUp { get; private set; } = 10;
    public PlayerController PlayerController { get; private set; }

    private bool _blockXp = false;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
            ChangeGameState(GameState.MainMenu);
        else
            ChangeGameState(GameState.BeforeGameStart);
        DropModuleOnCanvas.OnModuleAttached += DropModule_OnModuleAttached;
    }

    private void DropModule_OnModuleAttached(Module mod)
    {
        if(mod.GetModuleClass() == Module.ModuleClass.Placement)
        {
            _virtualCamera.m_Lens.OrthographicSize += 1.5f;
        }
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

            case GameState.BeforeGameStart:
                if (_virtualCamera == null)
                    _virtualCamera = GameObject.FindGameObjectWithTag("Camera").GetComponent<CinemachineVirtualCamera>();
                break;

            case GameState.StartGame:
                Time.timeScale = 1.0f;
                InitGame();
                OpenShop();
                
                break;
            case GameState.InGame:
                Time.timeScale = 1.0f;
                Time.fixedDeltaTime = Time.timeScale * 0.01f;
                break;
            case GameState.GameOver:
                Time.timeScale = 0.2f;
                Time.fixedDeltaTime = Time.timeScale * 0.01f;
                //faire des trucs de game over
                break;
            case GameState.ShopState:
                StartCoroutine(SlowMoCoroutine());
                OpenShop();
                break;
        }
        OnGameStateChanged?.Invoke(newState);
        Debug.Log("Game State: " + CurrentState.ToString());
    }

    public void GrantXP(uint xp)
    {
        if (_blockXp)
            return;
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
        OnLevelUp?.Invoke(CurrentLevel);

        CurrentXP -= NextTresholdLevelUp;
        NextTresholdLevelUp += 20;
        ChangeGameState(GameState.ShopState);
    }

    private void InitGame()
    {
        CurrentXP = 0;
        CurrentLevel = 1;
        NextTresholdLevelUp = 10;
        _blockXp = false;
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
        
        if(Keyboard.current.lKey.wasPressedThisFrame)
        {
            _blockXp = !_blockXp;
        }
    }

    public void ZoomIn()
    {
        _virtualCamera.m_Lens.OrthographicSize -= 1.5f;
    }

    IEnumerator SlowMoCoroutine()
    {
        Time.timeScale = 0.8f;
        Time.fixedDeltaTime = Time.timeScale * 0.01f;
        yield return new WaitForSecondsRealtime(0.3f);
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = Time.timeScale * 0.01f;
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = Time.timeScale * 0.01f;
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 0.0f;
        yield break;
    }

}
