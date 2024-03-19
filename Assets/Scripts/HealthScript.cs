using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{


    [SerializeField] private RectTransform _healthBar;
    [SerializeField] private Image _fillImage;
    [SerializeField] private int _maxHealth = 20;
    public event Action<int> OnChangeHealth;
    public event Action OnDeath;

    public float Health => _health;
    public int MaxHealth => _maxHealth;
    public bool IsAlive => _health > 0;

    private bool _isInvincible;
    private float _health;
    private AttachPointScript _attachPointScript;

    private void Awake()
    {
        _health = _maxHealth;
    }

    public void SetMaxHealth(int maxHealth)
    {
        _maxHealth = maxHealth;
        _health = maxHealth;
    }

    public void ChangeHealth(float value, bool canBeInvincible = true)
    {
        if (value < 0 && _isInvincible && canBeInvincible)
            return;
        else
            SetInvincibility();

        _health = (int)Mathf.Clamp(_health + value,-1, _maxHealth);
        OnChangeHealth?.Invoke((int)value);

        if(!_healthBar.gameObject.activeSelf)
            _healthBar.gameObject.SetActive(true);
        if(_health == _maxHealth)
            _healthBar.gameObject.SetActive(false);


        _fillImage.fillAmount = (float)_health / (float)_maxHealth;

        if (!IsAlive)
        {
            _attachPointScript?.EnableAttachPoint();
            OnDeath?.Invoke();
        }
    }

    public void SetInvincibility()
    {
        StartCoroutine(InvincibilityTimerCoroutine());
    }

    IEnumerator InvincibilityTimerCoroutine()
    {
        _isInvincible = true;
        yield return new WaitForSeconds(0.65f);
        _isInvincible = false;
    }

    public void SetHealthScript(AttachPointScript attachPointScript, int maxHealth)
    {
        this._attachPointScript = attachPointScript;
        _healthBar = this.transform.Find("HealthCanvas/HealthBar").GetComponent<RectTransform>();
        _fillImage = this.transform.Find("HealthCanvas/HealthBar/Background/Fill").GetComponent<Image>();
        _maxHealth = maxHealth;
        _health = maxHealth;
    }
}
