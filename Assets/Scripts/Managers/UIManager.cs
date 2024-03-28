using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public static bool _toggleReplaceModule = false;
    public static event Action<bool> OnToggleReplaceModule;
    public static event Action OnCloseScrapShop;

    [Header("Module Shop")]
    [SerializeField] private GameObject _moduleShop;
    [SerializeField] private GameObject _skipButton;

    [Header("Scrap Shop")]
    [SerializeField] private GameObject _scrapShop;
    [SerializeField] private GameObject _openScrapShopTxT;
    [SerializeField] private GameObject _scrapTxt;
    [SerializeField] private GameObject[] _scrapTab;
    [SerializeField] private Image[] _dots;

    [Header("PauseMenu")]
    [SerializeField] private GameObject _pauseGO;

    [Space]
    [SerializeField] private UI_XPScript _xpScript;

    [SerializeField] private RectTransform _gameOverPanel;

    [Space]
    [SerializeField] private Animator _tabAnimator;
    private ModuleImageScript[] _moduleImageScripts;

    [Header("Animators")]
    [SerializeField] private Animator _levelUpAnimatorInShop;


    private int _scrapIndex = 0;
    private void Start()
    {
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;

        ScrapManagerDataHandler.OnEnoughScrap += OnEnoughScrap;

        _moduleImageScripts = _moduleShop.GetComponentsInChildren<ModuleImageScript>();
        _scrapShop.SetActive(false);
        _openScrapShopTxT.SetActive(false);
        _skipButton.SetActive(false);

    }

    private void OnEnoughScrap(bool enoughScrap)
    {
        _tabAnimator?.SetBool("CanPurchase", enoughScrap);
        if (enoughScrap)
        {
            _tabAnimator.gameObject.SetActive(true);
            _tabAnimator.SetTrigger("PurchaseNotif");
        }
    }

    private void GameManager_OnGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.MainMenu:
                break;
            case GameState.StartGame:
                SetPause(false);

                break;
            case GameState.InGame:
                //CloseShop();
                SetPause(false);

                break;
            case GameState.GameOver:
                ShowGameOverPanel(true);
                break;
            case GameState.ShopState:
                SetPause(false);
                break;
            case GameState.Pause:
                SetPause(true);
                break;
        }
    }

    public void OpenShop()
    {
        
        if (GameManager.Instance.CurrentLevel != 1 && GameManager.Instance.PreviousState != GameState.Pause)
        {
            foreach (var moduleImage in _moduleImageScripts)
            {
                moduleImage.SetModuleDatas(ModuleManager.Instance.GetRandomModuleData());
            }
            _skipButton.SetActive(true);

        }
        else
            _skipButton.SetActive(false);

        _moduleShop.SetActive(true);
        _levelUpAnimatorInShop.SetTrigger("LevelUp");
        _openScrapShopTxT.SetActive(true);
        if ((bool)ScrapManagerDataHandler.AbleToBuyScrap())
            OpenScrapShop(true);
    }

    public void CloseShop()
    {
        ModuleManager.Instance.ResetModuleList();
        _moduleShop.SetActive(false);
        UpdateXpBar(GameManager.Instance.CurrentXP);
        UpdateLevel(GameManager.Instance.CurrentLevel);
        _openScrapShopTxT.SetActive(false);
        OpenScrapShop(false);
        //_toggleReplaceModule = true;
        //SetReplaceModule();
    }
    #region ScrapShop
    public void OpenScrapShop(bool open)
    {
        _scrapShop.SetActive(open);
        _scrapTab[_scrapIndex].SetActive(open);
        _scrapTxt.SetActive(open);
        _openScrapShopTxT.SetActive(false);
        if (GameManager.Instance.CurrentState != GameState.ShopState)
        {
            if (open)
            {
                Time.timeScale = 0.03f;
                Time.fixedDeltaTime = Time.timeScale * 0.01f;


            }
            else
            {
                Time.timeScale = 1.0f;
                Time.fixedDeltaTime = Time.timeScale * 0.01f;
                OnCloseScrapShop?.Invoke();
                ScrapManagerDataHandler.CheckScrap();
            }
        }

        ScrapManagerDataHandler.CheckScrap();

        Cursor.visible = open;
        if (!open && _moduleShop.activeSelf == true)
        {
            Cursor.visible = true;
        }
    }


    public void ChangeScrapTab(int tab)
    {
        if (tab == _scrapIndex)
            return;

        _scrapTab[_scrapIndex].SetActive(false);
        _dots[_scrapIndex].color = Color.gray;

        _scrapIndex = tab;
        _scrapTab[_scrapIndex].SetActive(true);
        _dots[_scrapIndex].color = Color.white;
    }
    public void ChangeScrapTab(bool positive)
    {
        _scrapTab[_scrapIndex].SetActive(false);
        _dots[_scrapIndex].color = Color.gray;
        _scrapIndex = positive ? ++_scrapIndex : --_scrapIndex;
        if (_scrapIndex > _scrapTab.Length - 1)
        {
            _scrapIndex = 0;
        }
        else if (_scrapIndex < 0)
        {
            _scrapIndex = _scrapTab.Length - 1;
        }
        _scrapTab[_scrapIndex].SetActive(true);
        _dots[_scrapIndex].color = Color.white;
    }
    #endregion
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
        ScrapManagerDataHandler.OnEnoughScrap -= OnEnoughScrap;

    }

    #region UIButtonFunctions
    public void Replay()
    {
        GameManager.Instance.ChangeGameState(GameState.StartGame);
        SceneManager.LoadScene(1);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void SetPause(bool active)
    {
        _pauseGO.SetActive(active);
        if (active)
            _pauseGO.GetComponent<Animator>().SetTrigger("Pause");
    }

    public void Resume()
    {
        GameManager.Instance.ResumePreviousState();
    }

    public void Quit()
    {
        GameManager.Instance.ChangeGameState(GameState.MainMenu);
        SceneManager.LoadScene(0);
    }

    #endregion
    public void UpdateXpBar(uint currentXP)
    {
        _xpScript.UpdateXP(currentXP);

    }
    public void UpdateLevel(uint level)
    {
        _xpScript.RefreshTextLvl(level);
    }
    public void SetReplaceModule()
    {
        _toggleReplaceModule = !_toggleReplaceModule;
        OnToggleReplaceModule?.Invoke(_toggleReplaceModule);
    }

    public void SkipLevelUp()
    {
        //ScrapManagerDataHandler.PickUpScrap(20);
        GameManager.Instance.PlayerController.GetHealthScript().ChangeHealth(15);
        GameManager.Instance.CloseShop();
    }

    public bool IsScrapShopOpen() => _scrapShop.activeSelf;
}
