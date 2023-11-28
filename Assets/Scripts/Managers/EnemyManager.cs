using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyDatas[] _enemyDatas;
    [SerializeField] private BasicEnemySpawner[] _spawns; //Change this later
    [Space]
    [SerializeField] private int _enemyLimit = 15;
    private List<EnemyScript> _enemiesList = new List<EnemyScript>();

    private void Awake()
    {
        EnemyManagerDataHandler.OnSpawnEnemy += OnSpawnEnemy;
        EnemyManagerDataHandler.OnGetEnemyDatas += OnGetEnemyDatas;
        EnemyScript.OnDeath += EnemyScript_OnDeath;

        ChronoManagerDataHandler.OnTimeElapsed += OnTimeElapsed;
    }

    private void OnTimeElapsed(float elapsedTime)
    {
        //Spawn elite enemy, wave etc...
        foreach (var spawner in _spawns)
        {
            OnSpawnEnemy(spawner.transform.position, _enemyDatas[0]);
            OnSpawnEnemy(spawner.transform.position, _enemyDatas[0]);
            OnSpawnEnemy(spawner.transform.position, _enemyDatas[0]);

        }

        OnSpawnEnemy(_spawns[0].transform.position, _enemyDatas[2]);
    }

    private EnemyDatas OnGetEnemyDatas()
    {
        //Pick datas depending on the current level;
        if (GameManager.Instance.CurrentLevel < 12)
            return _enemyDatas[0];
        else
            return _enemyDatas[1];
    }

    private void EnemyScript_OnDeath(object enemy, EnemyScript.EnemyStatsOnDeath e)
    {
        _enemiesList.Remove(e.enemyRef);
    }

    private EnemyScript OnSpawnEnemy(Vector2 pos, EnemyDatas datas)
    {
        if (_enemiesList.Count > _enemyLimit)
            return null;
        var newEnemy = EnemyScript.CreateEnemy(pos, datas);
        _enemiesList.Add(newEnemy);
        return newEnemy;
    }

    private void OnDisable()
    {
        EnemyManagerDataHandler.OnSpawnEnemy -= OnSpawnEnemy;
        EnemyManagerDataHandler.OnGetEnemyDatas -= OnGetEnemyDatas;
        EnemyScript.OnDeath -= EnemyScript_OnDeath;
    }
}
