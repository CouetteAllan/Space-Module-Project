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
    ShopState,
    Pause
}

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private XpDatas _xpDatas;

    public static event Action<GameState> OnGameStateChanged;
    public static event Action<uint> OnLevelUp;
    public GameState CurrentState { get; private set; } = GameState.GameOver;
    public uint CurrentLevel { get; private set; } = 1;
    public uint CurrentXP { get; private set; } = 0;
    public uint NextTresholdLevelUp { get; private set; } = 60;
    public PlayerController PlayerController { get; private set; }

    public bool HasShownTutoOnce { get; set; } = false;

    private bool _blockXp = false;
    private GameState _previousState;
    public GameState PreviousState { get { return _previousState; } }

    private bool _isPause;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
            ChangeGameState(GameState.MainMenu);
        else
            ChangeGameState(GameState.StartGame);

        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        if (arg1.buildIndex == 1)
            ChangeGameState(GameState.StartGame);
    }

    public void ChangeGameState(GameState newState)
    {
        if (newState == CurrentState)
            return;

        _previousState = CurrentState;
        CurrentState = newState;
        switch (CurrentState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1.0f;
                Cursor.visible = true;
                break;

            case GameState.BeforeGameStart:
                //FantomaticTutoManagerDataHandler.ShowTuto(true);
                break;

            case GameState.StartGame:
                Time.timeScale = 1.0f;
                //TutoManagerDataHandler.ShowTuto(true);
                InitGame();
                OpenShop();
                WaveManagerDataHandler.SetUpWaveManager();
                ModuleManager.Instance.SetModuleManager();
                
                break;
            case GameState.InGame:
                Time.timeScale = 1.0f;
                Time.fixedDeltaTime = Time.timeScale * 0.01f;
                Cursor.visible = false;
                _isPause = false;
                break;
            case GameState.GameOver:
                Time.timeScale = 0.2f;
                Time.fixedDeltaTime = Time.timeScale * 0.01f;
                //faire des trucs de game over
                Cursor.visible = true;
                break;
            case GameState.ShopState:
                StartCoroutine(SlowMoCoroutine(false));
                OpenShop();
                break;
            case GameState.Pause:
                StartCoroutine(SlowMoCoroutine(false));
                Cursor.visible = true;
                _isPause = true;
                break;
        }
        OnGameStateChanged?.Invoke(newState);
        Debug.Log("Game State: " + CurrentState.ToString());
    }

    public void ResumePreviousState()
    {
        ChangeGameState(_previousState);
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

    public void SwitchPauseMode()
    {
        _isPause = !_isPause;
        if(_isPause)
            ChangeGameState(GameState.Pause);
        else
            ResumePreviousState();
    }
    private void LevelUp()
    {
        CurrentLevel++;
        OnLevelUp?.Invoke(CurrentLevel);

        CurrentXP -= NextTresholdLevelUp;
        NextTresholdLevelUp = (uint)_xpDatas.XpCurve.Evaluate((float)CurrentLevel);
        ChangeGameState(GameState.ShopState);
    }

    private void InitGame()
    {
        CurrentXP = 0;
        CurrentLevel = 1;
        NextTresholdLevelUp = (uint)_xpDatas.XpCurve.Evaluate((float)CurrentLevel);
        _blockXp = false;
    }

    public void OpenShop()
    {
        UIManager.Instance.OpenShop();
        Cursor.visible = true;
    }

    public void CloseShop()
    {
        ChangeGameState(GameState.InGame);
        //jouer son
        //close UI
        UIManager.Instance.CloseShop();
        StartCoroutine(SlowMoCoroutine(true));
    }

    public void SetPlayer(PlayerController player)
    {
         this.PlayerController = player;
    }

    private void Update()
    {
        if(Keyboard.current.cKey.wasPressedThisFrame)
        {
            GrantXP(20);
        }
        
        if(Keyboard.current.lKey.wasPressedThisFrame)
        {
            _blockXp = !_blockXp;
        }
    }

    IEnumerator SlowMoCoroutine(bool reverse)
    {
        if (!reverse)
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
        else
        {
            Time.timeScale = 0.1f;
            Time.fixedDeltaTime = Time.timeScale * 0.01f;
            yield return new WaitForSecondsRealtime(0.2f);
            Time.timeScale = 0.2f;
            Time.fixedDeltaTime = Time.timeScale * 0.01f;
            yield return new WaitForSecondsRealtime(0.2f);
            Time.timeScale = 0.6f;
            Time.fixedDeltaTime = Time.timeScale * 0.01f;
            yield return new WaitForSecondsRealtime(0.4f);
            Time.timeScale = 1f;
            yield break;
        }
        
    }

}
