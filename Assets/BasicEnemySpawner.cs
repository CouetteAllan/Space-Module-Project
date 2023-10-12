using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;

    private void Awake()
    {
        TimeTickSystemDataHandler.OnTick += TimeTickSystemDataHandler_OnTick;
    }

    private void TimeTickSystemDataHandler_OnTick(uint tick)
    {
        if(tick % 12 == 0)
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
