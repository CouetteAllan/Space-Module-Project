using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<EnemyScript> _enemyScripts = new List<EnemyScript>();

    private void Awake()
    {
        EnemyManagerDataHandler.OnSpawnEnemy += OnSpawnEnemy;
        EnemyScript.OnDeath += EnemyScript_OnDeath;
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
        EnemyScript.OnDeath -= EnemyScript_OnDeath;
    }
}
