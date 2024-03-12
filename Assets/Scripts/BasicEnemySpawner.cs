using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyDatas _enemyDatas;
    private int _tickNeed;
    private bool _canSpawn = true;
    private void OnEnable()
    {
        _tickNeed = 20;
        TimeTickSystemDataHandler.OnTick += TimeTickSystemDataHandler_OnTick;
        GameManager.OnLevelUp += GameManager_OnLevelUp;
        TimerManagerDataHandler.OnSendTimeLevel += OnSendTimeLevel;
        TimerManagerDataHandler.OnEndTimer += OnEndTimer;
        
    }

    private void OnEndTimer()
    {
        _canSpawn = false;
    }

    private void OnSendTimeLevel(int timeLevel)
    {
        _canSpawn = timeLevel < 29;
        if(!_canSpawn )
            TimerManagerDataHandler.OnSendTimeLevel -= OnSendTimeLevel;
    }

    private void GameManager_OnLevelUp(uint level)
    {
        switch (level)
        {
            case 2:
                _tickNeed = 12;
                return;
            case 6:
                _tickNeed = 10;
                return;
            case 12:
                _tickNeed = 8;
                return;
            case 16:
                _tickNeed = 4;
                return;
        }
    }

    private void TimeTickSystemDataHandler_OnTick(uint tick)
    {
        if(tick % _tickNeed == 0 && _canSpawn)
        {
            _enemyDatas = EnemyManagerDataHandler.GetEnemyDatas();
            var newEnemy = EnemyManagerDataHandler.SpawnEnemy(this.transform.position,_enemyDatas);
        }
    }

    private void OnDisable()
    {
        TimeTickSystemDataHandler.OnTick -= TimeTickSystemDataHandler_OnTick;
        GameManager.OnLevelUp -= GameManager_OnLevelUp;
        TimerManagerDataHandler.OnSendTimeLevel -= OnSendTimeLevel;
        TimerManagerDataHandler.OnEndTimer -= OnEndTimer;


    }
}
