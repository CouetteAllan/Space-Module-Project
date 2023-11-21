using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour, IHittable
{
    public struct EnemyStat
    {
        //Data we need to know when an enemy dies (tier, level, timer ?)
        public int level;
        public int tier;
        public Vector2 finalPos;
    }

    public static event Action<EnemyStat> OnDeath;

    [SerializeField] private float speed = 3.0f;
    [SerializeField] private GameObject _particleEffect;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Color _baseColor;
    private Rigidbody2D _rigidbody;
    private PlayerController _playerController;

    private int _baseHealth = 1;
    private bool _gotHit = false;
    private int _health;

    private float _damageTimer = 0.8f;
    private float _timer = 0.0f;
    private bool _canDealDamage = true;

    private void Start()
    {
        SetUpEnemy();
        _baseColor = _spriteRenderer.color;
    }


    public void SetUpEnemy()
    {
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerController = GameManager.Instance.PlayerController;

        Vector2 dir = (Vector2)_playerController.transform.position - this._rigidbody.position;
        _rigidbody.velocity = dir.normalized * speed;
        _health = (int)GameManager.Instance.CurrentLevel * _baseHealth;
    }

    private void GameManager_OnGameStateChanged(GameState newState)
    {
        if(newState == GameState.StartGame)
        {
            
        }
    }

    private void Update()
    {
        Vector2 dir = (Vector2)_playerController.transform.position - this._rigidbody.position;
        this._rigidbody.velocity = dir.normalized * speed;

        _timer += Time.deltaTime;
        if(_timer >= _damageTimer && _canDealDamage == false)
        {
            _timer = 0.0f;
            _canDealDamage = true;
        }

    }

    public void TryHit(IDamageSource source, int damage)
    {
        //recoil + feedback
        if (_gotHit)
            return;
        _gotHit = true;
        _rigidbody.AddForce( (this._rigidbody.position - (Vector2)source.Transform.position) * 30.0f, ForceMode2D.Impulse);
        StartCoroutine(ChangeColorCoroutine());
        ChangeHealth(-damage);
    }

    private void ChangeHealth(int healthChange)
    {
        _health += healthChange;
        if (_health <= 0)
            Die();
        _gotHit = false;

    }

    private void Die()
    {
        StopAllCoroutines();
        GameManager.Instance.GrantXP(5);
        Instantiate(_particleEffect,this.transform.position,Quaternion.identity);
        _gotHit = false;
        OnDeath?.Invoke(new EnemyStat {
            level = (int)GameManager.Instance.CurrentLevel ,
            tier = 1,
            finalPos = this.transform.position,
        });
        Destroy(this.gameObject);
        return;
    }

    private IEnumerator ChangeColorCoroutine()
    {
        _spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(0.12f);

        _spriteRenderer.color = _baseColor;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!_canDealDamage)
            return;

        if(collision.collider.TryGetComponent<HealthScript>(out HealthScript healthScript))
        {
            healthScript.ChangeHealth(-1);
            _canDealDamage = false;
        }
    }
}
