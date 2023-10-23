using System;
using System.Collections;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public event Action<int> OnChangeHealth;

    public int Health => _health;
    public bool IsAlive => _health > 0;

    private bool _isInvincible;
    private int _health = 10;
    private AttachPointScript _attachPointScript;

    public void ChangeHealth(int value)
    {
        if (value < 0 && _isInvincible)
            return;
        else
            SetInvincibility();

        _health += value;
        OnChangeHealth?.Invoke(value);
        Debug.Log(this.gameObject.name + " has " +_health +" health");

        if (!IsAlive)
        {
            Debug.Log(this.gameObject.name + " has no health");
            _attachPointScript.EnableAttachPoint();
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
    }
}
