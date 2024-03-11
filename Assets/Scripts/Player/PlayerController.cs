using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rayqdr.Input;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IGatherScrap
{
    [SerializeField] private GameObject _explosionParticles;
    [SerializeField] private SpriteRenderer _graph;
    [SerializeField] private SpriteRenderer _circleGraph;
    [SerializeField] private Color _targetColorLowHealth;

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
    private Color _startColor;

    private bool _scrapShopOpen = false;

    private void Awake()
    {
        _rotation = GetComponent<PlayerRotation>();
        _playerModule = GetComponent<PlayerModule>();
        _healthScript = GetComponent<HealthScript>();
        _startColor = _circleGraph.color;

        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
        _healthScript.OnChangeHealth += OnChangeHealth;
        _healthScript.OnDeath += OnDeath;
        Module.OnModuleDestroyed += Module_OnModuleDestroyed;
    }

    private void Module_OnModuleDestroyed(Module mod)
    {
        if (mod.GetModuleClass() != Module.ModuleClass.Placement)
            return;

        _healthScript.ChangeHealth(-10);
    }

    private void OnChangeHealth(int health)
    {
        //Do hitFeedback
        if(health < 0)
        {
            StartCoroutine(ChangeHealthCoroutine());
        }
        _circleGraph.color = Color.Lerp(_startColor, _targetColorLowHealth, 1.0f - (float)_healthScript.Health / (float)_healthScript.MaxHealth);

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
        _healthScript.SetMaxHealth((int)StatSystem.Instance.PlayerStat.GetStatValue(StatType.Health));
    }

    private void GameManager_OnGameStateChanged(GameState newState)
    {
        if(newState == GameState.StartGame)
        {
            GameManager.Instance.SetPlayer(this);
            _healthScript.SetMaxHealth((int)StatSystem.Instance.PlayerStat.GetStatValue(StatType.Health));
            SetUpInputAction();
        }
    }



    private void SetUpInputAction()
    {
        if (_inputActions != null)
            return;

        _inputActions = new MInputActionAsset();
        _inputActions.Player.OpenScrapShop.performed += OpenScrapShop_performed;
        _inputActions.Player.OpenPauseMenu.performed += OpenPauseMenu_performed;
        _inputActions.Enable();
    }

    private void OpenPauseMenu_performed(InputAction.CallbackContext context)
    {
        GameManager.Instance.SwitchPauseMode();
    }

    private void OpenScrapShop_performed(InputAction.CallbackContext context)
    {
        _scrapShopOpen = !_scrapShopOpen;
        UIManager.Instance.OpenScrapShop(_scrapShopOpen);
    }

    private void OnDisable()
    {
        _inputActions.Player.OpenScrapShop.performed -= OpenScrapShop_performed;
        _inputActions.Player.OpenPauseMenu.performed -= OpenPauseMenu_performed;
        _inputActions.Disable();
        _inputActions = null;

        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
        _healthScript.OnChangeHealth -= OnChangeHealth;
        _healthScript.OnDeath -= OnDeath;
    }

    public PlayerModule GetPlayerModule()
    {
        return _playerModule;
    }

    public HealthScript GetHealthScript() { return _healthScript;}

    public void HealPlayer(int health)
    {
        _healthScript.ChangeHealth((int)((_healthScript.MaxHealth *  health)/100.0f));
    }

    public void GatherScrapMetal(int value)
    {
        ScrapManagerDataHandler.PickUpScrap(value);
    }

    private IEnumerator ChangeHealthCoroutine()
    {
        //Turn red
        var startColor = _graph.color;
        var newColor = new Color(212.0f / 255.0f, 149.0f/255.0f, 149.0f/255.0f, 255.0f/255.0f);
        _graph.color = newColor;

        yield return new WaitForSeconds(0.25f);

        //Turn normal
        _graph.color = startColor;
    }
}

