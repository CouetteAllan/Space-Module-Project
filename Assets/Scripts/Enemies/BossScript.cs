
using Cinemachine;
using DG.Tweening;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : EnemyScript
{

    [SerializeField] private Transform[] _spawnPositions;
    [SerializeField] private HealthScript _healthScript;
    [SerializeField] private EnemyDatas[] _enemyToInstantiate;
    [SerializeField] private Animator _bossAnimator;
    [SerializeField] private CinemachineVirtualCamera _cam;
    [SerializeField] private GameObject[] _graphs;
    [SerializeField] private Collider2D[] _colliders;

    [Header("Feedbacks")]
    [SerializeField] private Image _healthBarFill;
    [SerializeField] private GameObject _shield;
    [SerializeField] private MMF_Player _feedbackDeath;

    private BossData _bossDatas;
    private float _timeNextAttack;
    private float _timerAttack;

    private enum BossState
    {
        Move,
        Attack,
        Swarm,
        Debut
    }

    private BossState _currentState = BossState.Move;
    public override void SetUpEnemy(EnemyDatas datas)
    {
        base.SetUpEnemy(datas);
        _healthScript.SetMaxHealth((int)_currentHealth);
        _bossDatas = (BossData)datas;
        _timeNextAttack = _bossDatas.TimeNextAttack;
        _timerAttack = _timeNextAttack;
        EnemyManagerDataHandler.OnTriggerBossCinematic += OnTriggerBossCinematic;
        EnemyManager.OnEndBossCinematic += OnEndBossCinematic;
        _currentState = BossState.Debut;
        foreach (var go in _graphs)
        {
            go.SetActive(false);
        }

        foreach (var col in _colliders)
        {
            col.enabled = false;
        }
    }

    private void OnEndBossCinematic()
    {
        _cam.Priority = 0;
    }

    private void OnTriggerBossCinematic(Action callback)
    {
        ActivateCamera();
        FXManager.Instance.PlayEffect("boss",this.transform.position,this.transform.rotation);
        StartCoroutine(AppearBoss());
    }

    public void ActivateCamera()
    {
        _cam.Priority = 100;
    }

    public void HideBoss()
    {
        StartCoroutine(DelayHideBoss());
    }

    IEnumerator DelayHideBoss()
    {
        yield return new WaitForSeconds(2.5f);
        foreach (var graph in _graphs)
        {
            graph.SetActive(false);
        }
    }

    private IEnumerator AppearBoss()
    {
        yield return new WaitForSeconds(7);
        EnemyManagerDataHandler.ShowBoss();
        yield return new WaitForSeconds(1);
        
    }

    protected override void ChangeHealth(float healthChange)
    {
        _currentHealth += healthChange;
        if (_currentHealth <= 0)
            Die(true);
        _healthScript.ChangeHealth(healthChange,canBeInvincible: false);
    }

    public override void Die(bool grantLoot)
    {
        _feedbackDeath.PlayFeedbacks();
        EnterState(BossState.Debut);

        foreach (var col in _colliders)
        {
            col.enabled = false;
        }
    }

    public void FeedbackDone()
    {
        _cam.gameObject.SetActive(false);
        SendDeath();
        Destroy(gameObject,.2f);
    }

    protected override void Update()
    {
        _canTurn = _currentState != BossState.Attack;
        base.Update();
        if(_currentState == BossState.Move)
        {
            _timerAttack -= Time.deltaTime;
            if (_timerAttack < 0)
            {
                bool rand = UnityEngine.Random.Range(0, 2) == 0;
                BossState newState = rand ? BossState.Swarm : BossState.Attack;
                EnterState(newState);
                _timerAttack = _timeNextAttack;
            }
        }
        
    }

    protected override void FixedUpdate()
    {
        switch(_currentState)
        {
            case BossState.Move:
                base.FixedUpdate();
                break;
        }
    }

    private void EnterState(BossState newState)
    {
        _currentState = newState;
        switch (newState)
        {
            case BossState.Move:
                _healthBarFill.color = Color.red;
                _shield.SetActive(false);

                break;
            case BossState.Attack:
                _healthBarFill.color = new Color(205.0f / 255.0f, 205.0f / 255.0f, 205.0f / 255.0f, 150.0f / 255.0f);
                _shield.SetActive(true);
                StartCoroutine(AttackCoroutine());
                break;
            case BossState.Swarm:
                _healthBarFill.color = new Color(205.0f/255.0f, 205.0f/255.0f, 205.0f/255.0f, 150.0f/255.0f);
                _shield.SetActive(true);
                StartCoroutine(SwarmCoroutine());

                break;
        }
    }


    private IEnumerator SwarmCoroutine()
    {
        this._rigidbody.isKinematic = true;
        this._rigidbody.simulated = false;
        //Instantiate over time
        for (int i = 0; i < _bossDatas.NbEnemiesToInstantiate; i++)
        {
            EnemyManagerDataHandler.SpawnEnemy(_spawnPositions[i % 3].position, _enemyToInstantiate[i % 4]);
            yield return new WaitForSeconds(0.25f);
        }

        this._rigidbody.isKinematic = false;
        this._rigidbody.simulated = true;

        EnterState(BossState.Move);
        yield break;
    }

    private IEnumerator AttackCoroutine()
    {
        this._rigidbody.isKinematic = true;
        this._rigidbody.simulated = false;

        yield return new WaitForSeconds(.3f);
        _bossAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(5f);

        this._rigidbody.isKinematic = false;
        this._rigidbody.simulated = true;
        EnterState(BossState.Move);
    }

    public void ActivateHitBoxes(bool active)
    {
        this._rigidbody.isKinematic = !active;
        this._rigidbody.simulated = active;
    }

    public void ActivateBoss()
    {
        this.transform.localScale = Vector3.one * .1f;
        foreach (var go in _graphs)
        {
            go.SetActive(true);
        }

        foreach (var col in _colliders)
        {
            col.enabled = true;
        }
        this.transform.DOScale(1.0f, .8f).SetEase(Ease.OutBack).OnComplete(() => _currentState = BossState.Move);
        
    }

    private void OnDestroy()
    {
        EnemyManagerDataHandler.OnTriggerBossCinematic -= OnTriggerBossCinematic;
        EnemyManager.OnEndBossCinematic -= OnEndBossCinematic;
    }

}
