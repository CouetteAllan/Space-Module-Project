using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : EnemyScript
{
    [SerializeField] private Transform[] _spawnPositions;
    [SerializeField] private HealthScript _healthScript;


    private enum BossState
    {
        Move,
        Attack,
        Swarm
    }
    public override void SetUpEnemy(EnemyDatas datas)
    {
        base.SetUpEnemy(datas);
        _healthScript.SetMaxHealth((int)datas.BaseHealth);
    }

    protected override void ChangeHealth(int healthChange)
    {
        _healthScript.ChangeHealth(healthChange);
        base.ChangeHealth(healthChange);
    }

    private void EnterState(BossState newState)
    {
        switch (newState)
        {
            case BossState.Move:
                break;
            case BossState.Attack:
                //Stop movement
                break;
            case BossState.Swarm:
                //Start to open fier and swarm with enemies
                break;
        }
    }

}
