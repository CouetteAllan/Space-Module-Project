using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour, IHittable
{

    public static event EventHandler<EnemyStatsOnDeath> OnDeath;
    public class EnemyStatsOnDeath : EventArgs
    {
        public EnemyScript enemyRef;
        public int level;
        public int tier;
        public Vector2 finalPos;
        public int xpGranted;
        public int scrapGranted;
    }

    [SerializeField] private GameObject _particleEffect;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private MMF_Player _feedbackHit;
    protected Color _baseColor;
    protected Rigidbody2D _rigidbody;
    protected PlayerController _playerController;
    
    protected bool _gotHit = false;
    protected bool _hasHit = false;
    protected float _currentHealth;

    protected float _damageTimer = 0.6f;
    protected float _timer = 0.0f;
    protected bool _canDealDamage = true;
    protected bool _canTurn = true;

    protected EnemyDatas _datas;
    protected IEnemyBehaviour _enemyBehaviour;

    public static EnemyScript CreateEnemy(Vector2 position, EnemyDatas datas)
    {
        EnemyScript newEnemy = Instantiate(datas.EnemyPrefab, position, Quaternion.identity).GetComponent<EnemyScript>();
        newEnemy.SetUpEnemy(datas);
        return newEnemy;
    }


    public virtual void SetUpEnemy(EnemyDatas datas)
    {
        _datas = datas;
        _baseColor = _spriteRenderer.color;

        _rigidbody = GetComponent<Rigidbody2D>();
        _playerController = GameManager.Instance.PlayerController;

        Vector2 dir = (Vector2)_playerController.transform.position - this._rigidbody.position;
        _rigidbody.velocity = dir.normalized * _datas.BaseSpeed;
        _currentHealth = datas.BaseHealth + GameManager.Instance.CurrentLevel * _datas.HealthMultplier;

        _enemyBehaviour = _datas.GetEnemyBehaviour(_rigidbody);
    }


    protected virtual void Update()
    {
        if (_playerController == null)
            return;

        
        _timer += Time.deltaTime;
        if(_timer >= _damageTimer && _canDealDamage == false)
        {
            _timer = 0.0f;
            _canDealDamage = true;
            _hasHit = false;
        }
        if (_canTurn)
        {
            Vector3 targetRotation = (Vector2)_playerController.transform.position - this._rigidbody.position;
            transform.right = Vector3.Slerp(transform.right, targetRotation, Time.deltaTime * _datas.TurnRate);
        }
        
    }

    protected virtual void FixedUpdate()
    {
        if (_playerController == null)
            return;

        if (!_gotHit && !_hasHit)
        {
            _enemyBehaviour.DoMovement(_playerController);
        }
        else
        {
            _enemyBehaviour.DoMovement(_playerController,isStopped: true);
        }

    }

    public void TryHit(IDamageSource source, int damage)
    {
        //recoil + feedback
        if (_gotHit)
            return;
        _gotHit = true;
        _rigidbody.AddForce( (this._rigidbody.position - (Vector2)source.Transform.position).normalized * 10.0f * source.RecoilMultiplier, ForceMode2D.Impulse);
        StartCoroutine(ChangeColorCoroutine());
        ChangeHealth(-damage);
    }

    protected virtual void ChangeHealth(float healthChange)
    {
        _currentHealth += healthChange;
        if(_feedbackHit != null)
        {
            _feedbackHit.PlayFeedbacks();
        }
        if (_currentHealth <= 0)
            Die(true);

    }

    public virtual void Die(bool grantLoot)
    {
        StopAllCoroutines();
        _gotHit = false;
        if (grantLoot)
        {
            Instantiate(_particleEffect, this.transform.position, Quaternion.identity);
            SendDeath();
        }

        Destroy(this.gameObject);
    }

    protected void SendDeath()
    {
        GameManager.Instance.GrantXP((uint)_datas.XPGranted);
        OnDeath?.Invoke(this, new EnemyStatsOnDeath
        {
            enemyRef = this,
            level = (int)GameManager.Instance.CurrentLevel,
            tier = (int)_datas.Tier,
            finalPos = this.transform.position,
            xpGranted = _datas.XPGranted,
            scrapGranted = _datas.ScrapMetalGranted,
        });
    }

    private IEnumerator ChangeColorCoroutine()
    {
        _spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(0.12f);

        _spriteRenderer.color = _baseColor;
        _gotHit = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!_canDealDamage)
            return;

        if(collision.collider.TryGetComponent<HealthScript>(out HealthScript healthScript))
        {
            healthScript.ChangeHealth(-(int)_datas.BaseDamage);
            _canDealDamage = false;
            _hasHit = true;
        }
    }
}
