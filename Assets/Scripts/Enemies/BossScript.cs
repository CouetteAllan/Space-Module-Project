
using UnityEngine;

public class BossScript : EnemyScript
{
    [SerializeField] private Transform[] _spawnPositions;
    [SerializeField] private HealthScript _healthScript;

    private BossData _bossDatas;
    private float _timeNextAttack;
    private float _timerAttack;

    private enum BossState
    {
        Move,
        Attack,
        Swarm
    }

    private BossState _currentState = BossState.Move;
    public override void SetUpEnemy(EnemyDatas datas)
    {
        base.SetUpEnemy(datas);
        _healthScript.SetMaxHealth((int)_currentHealth);
        _bossDatas = (BossData)datas;
        _timeNextAttack = _bossDatas.TimeNextAttack;
        _timerAttack = _timeNextAttack;
    }

    protected override void ChangeHealth(float healthChange)
    {
        base.ChangeHealth(healthChange);
        _healthScript.ChangeHealth(healthChange,canBeInvincible: false);
    }

    protected override void Update()
    {
        base.Update();
        _timerAttack -= Time.deltaTime;
        if( _timerAttack < 0 )
        {
            bool rand = Random.Range(0, 1) == 0;
            BossState newState = rand ? BossState.Swarm : BossState.Attack;
            EnterState(newState);
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
                break;
            case BossState.Attack:
                //Stop movement
                //Do Mega Laser Attack
                break;
            case BossState.Swarm:
                //Start to open fire and swarm with enemies
                break;
        }
    }

}
