using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    [SerializeField] private RectTransform _healthBar;
    [SerializeField] private Image _fillImage;
    public event Action<int> OnChangeHealth;

    public int Health => _health;
    public bool IsAlive => _health > 0;

    private bool _isInvincible;
    private int _health = 15;
    private int _maxHealth;
    private AttachPointScript _attachPointScript;

    private void Awake()
    {
        _maxHealth = 15;
    }

    public void ChangeHealth(int value)
    {
        if (value < 0 && _isInvincible)
            return;
        else
            SetInvincibility();

        _health += value;
        OnChangeHealth?.Invoke(value);
        Debug.Log(this.gameObject.name + " has " +_health +" health");

        if(!_healthBar.gameObject.activeSelf)
            _healthBar.gameObject.SetActive(true);

        _fillImage.fillAmount = (float)_health / (float)_maxHealth;

        if (!IsAlive)
        {
            Debug.Log(this.gameObject.name + " has no health");
            _attachPointScript?.EnableAttachPoint();
            Destroy(this.gameObject);
        }
    }

    public void SetInvincibility()
    {
        StartCoroutine(InvincibilityTimerCoroutine());
    }

    IEnumerator InvincibilityTimerCoroutine()
    {
        _isInvincible = true;
        yield return new WaitForSeconds(0.2f);
        _isInvincible = false;
    }

    public void SetAttachPoint(AttachPointScript attachPointScript)
    {
        this._attachPointScript = attachPointScript;
        _healthBar = this.transform.Find("HealthCanvas/HealthBar").GetComponent<RectTransform>();
        _fillImage = this.transform.Find("HealthCanvas/HealthBar/Background/Fill").GetComponent<Image>();
        _maxHealth = _health;
    }
}
