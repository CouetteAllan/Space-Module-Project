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

    public int Health => _health;
    public bool IsAlive => _health > 0;

    private bool _isInvincible;
    private int _health;
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

    public void ChangeHealth(int value)
    {
        if (value < 0 && _isInvincible)
            return;
        else
            SetInvincibility();

        _health += value;
        OnChangeHealth?.Invoke(value);

        if(!_healthBar.gameObject.activeSelf)
            _healthBar.gameObject.SetActive(true);

        _fillImage.fillAmount = (float)_health / (float)_maxHealth;

        if (!IsAlive)
        {
            _attachPointScript?.EnableAttachPoint();
            OnDeath?.Invoke();
            //Destroy(this.gameObject);
        }
    }

    public void SetInvincibility()
    {
        StartCoroutine(InvincibilityTimerCoroutine());
    }

    IEnumerator InvincibilityTimerCoroutine()
    {
        _isInvincible = true;
        yield return new WaitForSeconds(0.25f);
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
