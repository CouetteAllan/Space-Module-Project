using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static event Action OnEndBossCinematic;

    [SerializeField] private EnemyDatas[] _enemyDatas;
    [SerializeField] private BossData _bossData;
    [SerializeField] private BasicEnemySpawner[] _spawns; //Change this later
    [Space]
    [SerializeField] private int _enemyLimit = 60;
    private List<EnemyScript> _enemiesList = new List<EnemyScript>();
    private int _currentTimerLevel = 0;
    private bool _bossCinematicEnd = false;

    private void Awake()
    {
        EnemyManagerDataHandler.OnSpawnEnemy += OnSpawnEnemy;
        EnemyManagerDataHandler.OnGetEnemyDatas += OnGetEnemyDatas;
        EnemyScript.OnDeath += EnemyScript_OnDeath;

        TimerManagerDataHandler.OnTimeElapsed += OnTimeElapsed;
        TimerManagerDataHandler.OnSendTimeLevel += OnSendTimeLevel;
        TimerManagerDataHandler.OnEndTimer += OnEndTimer;
        _currentTimerLevel = 0;

        EnemyManagerDataHandler.OnSpawnEnemyWave += OnSpawnEnemyWave;

    }

    private void OnEndTimer()
    {
        StartCoroutine(BossSpawnCoroutine());
    }

    private IEnumerator BossSpawnCoroutine()
    {
        yield return StartCoroutine(BossAppearCinematic());
        
        //Set Up Camera
        this.BossSpawned();
    }

    private IEnumerator BossAppearCinematic()
    {
        yield return new WaitForSeconds(2);

        ClearAllEnemies();
        float distanceFromPlayer = 70.0f;
        Vector2 playerPos = GameManager.Instance.PlayerController.transform.position + UtilsClass.GetRandomDir() * distanceFromPlayer;

        //Set Up Boss
        OnSpawnEnemy(playerPos, _bossData);
        //Wait until the end of the timeline/cinematic
        this.TriggerBossCinematic(EndBossCinematic);
        yield return new WaitUntil(() => _bossCinematicEnd = true);
        yield break;

    }

    private void EndBossCinematic()
    {
        _bossCinematicEnd = true;
        OnEndBossCinematic?.Invoke();
    }

    private void OnSpawnEnemyWave(EnemyDatas datas, int number)
    {
        var startLimit = _enemyLimit;
        _enemyLimit += number * 2;
        for (int i = 0; i < number; i++)
        {
            float distanceFromPlayer = 28.0f;
            var pos = GameManager.Instance.PlayerController.transform.position + UtilsClass.GetRandomDir() * distanceFromPlayer;
            OnSpawnEnemy( pos, datas);

        }
        _enemyLimit = startLimit;
    }

    private void OnSendTimeLevel(int currentTimerLevel)
    {
        _currentTimerLevel = currentTimerLevel;
    }

    private void OnTimeElapsed(float elapsedTime)
    {
        var startLimit = _enemyLimit;
        _enemyLimit += 6;
        OnSpawnEnemy(_spawns[0].transform.position, _enemyDatas[3]);
        //Spawn elite enemy, wave etc...
        foreach (var spawner in _spawns)
        {
            OnSpawnEnemy(spawner.transform.position, _enemyDatas[1]);
            OnSpawnEnemy(spawner.transform.position, _enemyDatas[1]);
            OnSpawnEnemy(spawner.transform.position, _enemyDatas[1]);
            OnSpawnEnemy(spawner.transform.position, _enemyDatas[1]);

        }
        _enemyLimit = startLimit;
    }

    private EnemyDatas OnGetEnemyDatas()
    {
        //Pick datas depending on the current timer level (1 timer level = 10sec);
        switch (_currentTimerLevel)
        {
            case < 15:
            case < 23 when _currentTimerLevel > 18:
                
                _enemyLimit = 25;
                return _enemyDatas[0];

            case > 15 when _currentTimerLevel < 23:
                _enemyLimit = 60;
                return _enemyDatas[2];
            case > 23:
                _enemyLimit = 40;
                return _enemyDatas[3];

            default:
                _enemyLimit = 25;
                return _enemyDatas[0];
        }
    }

    private void EnemyScript_OnDeath(object enemy, EnemyScript.EnemyStatsOnDeath e)
    {
        _enemiesList.Remove(e.enemyRef);
        SoundManager.Instance.Play("ShipExplosion");
        if (e.tier == 5)
            this.BossDeath();
    }

    private EnemyScript OnSpawnEnemy(Vector2 pos, EnemyDatas datas)
    {
        if (_enemiesList.Count > _enemyLimit)
            return null;
        var newEnemy = EnemyScript.CreateEnemy(pos, datas);
        _enemiesList.Add(newEnemy);
        return newEnemy;
    }

    private void ClearAllEnemies()
    {
        foreach(var enemy in _enemiesList)
        {
            enemy.Die(grantLoot: false);
        }
        _enemiesList.Clear();
    }


    private void OnDisable()
    {
        EnemyManagerDataHandler.OnSpawnEnemy -= OnSpawnEnemy;
        EnemyManagerDataHandler.OnGetEnemyDatas -= OnGetEnemyDatas;
        EnemyScript.OnDeath -= EnemyScript_OnDeath;

        TimerManagerDataHandler.OnTimeElapsed -= OnTimeElapsed;
        TimerManagerDataHandler.OnSendTimeLevel -= OnSendTimeLevel;
        TimerManagerDataHandler.OnEndTimer -= OnEndTimer;

        EnemyManagerDataHandler.OnSpawnEnemyWave -= OnSpawnEnemyWave;


    }
}
