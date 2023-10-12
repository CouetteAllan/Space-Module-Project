using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{

    [SerializeField] private GameObject _moduleShop;
    [SerializeField] private UI_XPScript _xpScript;

    private void Start()
    {
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
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
                break;
            case GameState.ShopState:
                break;
        }
    }

    public void OpenShop()
    {
        _moduleShop.SetActive(true);
    }

    public void CloseShop()
    {
        _moduleShop.SetActive(false);
        UpdateXpBar(GameManager.Instance.CurrentXP);
        UpdateLevel(GameManager.Instance.CurrentLevel);
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
}
