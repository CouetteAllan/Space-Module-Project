using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    public static bool _toggleReplaceModule = false;
    public static event Action<bool> OnToggleReplaceModule;

    [SerializeField] private GameObject _moduleShop;
    [SerializeField] private GameObject _scrapShop;
    [SerializeField] private GameObject _openScrapShopTxT;
    [SerializeField] private UI_XPScript _xpScript;

    [SerializeField] private RectTransform _gameOverPanel;
    private ModuleImageScript[] _moduleImageScripts;

    private void Start()
    {
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
        _moduleImageScripts = _moduleShop.GetComponentsInChildren<ModuleImageScript>();
        _openScrapShopTxT.SetActive(true);
        _scrapShop.SetActive(false);
    }

    private void GameManager_OnGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.MainMenu:
                break;
            case GameState.StartGame:
                break;
            case GameState.InGame:
                CloseShop();
                break;
            case GameState.GameOver:
                ShowGameOverPanel(true);
                break;
            case GameState.ShopState:
                break;
        }
    }

    public void OpenShop()
    {

        if(GameManager.Instance.CurrentLevel != 1)
        {
            foreach (var moduleImage in  _moduleImageScripts)
            {
                moduleImage.SetModuleDatas(ModuleManager.Instance.GetRandomModuleData());
            }

        }
        _moduleShop.SetActive(true);
    }

    public void CloseShop()
    {
        ModuleManager.Instance.ResetModuleList();
        _moduleShop.SetActive(false);
        UpdateXpBar(GameManager.Instance.CurrentXP);
        UpdateLevel(GameManager.Instance.CurrentLevel);
    }

    public void OpenScrapShop(bool open)
    {
        _scrapShop.SetActive(open);
        _openScrapShopTxT.SetActive(!open);
        if(GameManager.Instance.CurrentState != GameState.ShopState)
        {
            if (open)
            {
                Time.timeScale = 0.15f;
                Time.fixedDeltaTime = Time.timeScale * 0.01f;

            }
            
            else
                Time.timeScale = 1.0f;
        }
        
    }

    private void ShowGameOverPanel(bool show)
    {
        _gameOverPanel.gameObject.SetActive(show);
        if (show)
        {
            _gameOverPanel.GetComponent<Animator>().SetTrigger("GameOver");
        }
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }

    public void UpdateXpBar(uint currentXP)
    {
        _xpScript.UpdateXP(currentXP);

    }
    public void UpdateLevel(uint level)
    {
        _xpScript.RefreshTextLvl(level);
    }

    public void Replay()
    {
        GameManager.Instance.ChangeGameState(GameState.StartGame);
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SetReplaceModule()
    {
        _toggleReplaceModule = !_toggleReplaceModule;
        OnToggleReplaceModule?.Invoke(_toggleReplaceModule);
    }
}
