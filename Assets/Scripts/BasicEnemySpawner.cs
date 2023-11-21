using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    private int _tickNeed;

    private void Awake()
    {
        _tickNeed = 12;
        TimeTickSystemDataHandler.OnTick += TimeTickSystemDataHandler_OnTick;
        GameManager.OnLevelUp += GameManager_OnLevelUp;
        
    }

    private void GameManager_OnLevelUp(uint level)
    {
        switch (level)
        {
            case 8:
                _tickNeed = 10;
                return;
            case 12:
                _tickNeed = 8;
                return;
            case 18:
                _tickNeed = 6;
                return;
            case 24:
                _tickNeed = 4;
                return;
        }
    }

    private void TimeTickSystemDataHandler_OnTick(uint tick)
    {
        if(tick % _tickNeed == 0)
        {
            var enemy = Instantiate(EnemyPrefab,this.transform.position,Quaternion.identity);
            enemy.GetComponent<EnemyScript>().SetUpEnemy();
        }
    }

    private void OnDisable()
    {
        TimeTickSystemDataHandler.OnTick -= TimeTickSystemDataHandler_OnTick;
    }
}
