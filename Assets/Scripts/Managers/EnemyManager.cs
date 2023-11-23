using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyDatas[] _enemyDatas;
    private List<EnemyScript> _enemyScripts = new List<EnemyScript>();

    private void Awake()
    {
        EnemyManagerDataHandler.OnSpawnEnemy += OnSpawnEnemy;
        EnemyManagerDataHandler.OnGetEnemyDatas += OnGetEnemyDatas;
        EnemyScript.OnDeath += EnemyScript_OnDeath;
    }

    private EnemyDatas OnGetEnemyDatas()
    {
        //Pick datas depending on the current level;
        if (GameManager.Instance.CurrentLevel <= 12)
            return _enemyDatas[0];
        else
            return _enemyDatas[1];
    }

    private void EnemyScript_OnDeath(object enemy, EnemyScript.EnemyStatsOnDeath e)
    {
        _enemyScripts.Remove(e.enemyRef);
    }

    private EnemyScript OnSpawnEnemy(Vector2 pos, EnemyDatas datas)
    {
        var newEnemy = EnemyScript.CreateEnemy(pos, datas);
        _enemyScripts.Add(newEnemy);
        return newEnemy;
    }

    private void OnDisable()
    {
        EnemyManagerDataHandler.OnSpawnEnemy -= OnSpawnEnemy;
        EnemyManagerDataHandler.OnGetEnemyDatas -= OnGetEnemyDatas;
        EnemyScript.OnDeath -= EnemyScript_OnDeath;
    }
}
