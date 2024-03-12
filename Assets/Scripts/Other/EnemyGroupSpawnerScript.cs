using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyGroupSpawnerScript : MonoBehaviour
{
    [SerializeField] private BasicEnemySpawner[] _basicEnemySpawners;

    private const int THIRTY_SECONDS = 3;
    private const int ONE_MINUTE_THIRTY = 9;
    private const int THREE_MINUTES = 18;
    private const int FOUR_MINUTES = 24;

    private List<BasicEnemySpawner> _activeEnemySpawners = new List<BasicEnemySpawner>();



    private void Awake()
    {
        TimerManagerDataHandler.OnSendTimeLevel += OnSendTimeLevel;
        for(int i = 0; i<=2;  i++)
        {
            _activeEnemySpawners.Add(_basicEnemySpawners[i]);
        }
        foreach(var spawner in _activeEnemySpawners)
        {
            spawner.gameObject.SetActive(true);
        }
    }

    private void OnSendTimeLevel(int currentTimeLevel)
    {
        switch(currentTimeLevel)
        {
            case THIRTY_SECONDS:
                _activeEnemySpawners.Add(_basicEnemySpawners[3]);
                _activeEnemySpawners.Last().gameObject.SetActive(true);
                break;
            case ONE_MINUTE_THIRTY:
                _activeEnemySpawners.Add(_basicEnemySpawners[4]);
                _activeEnemySpawners.Last().gameObject.SetActive(true);
                break;
            case THREE_MINUTES:
                _activeEnemySpawners.Add(_basicEnemySpawners[5]);
                _activeEnemySpawners.Last().gameObject.SetActive(true);
                break;
            case FOUR_MINUTES:
                _activeEnemySpawners.Add(_basicEnemySpawners[6]);
                _activeEnemySpawners.Last().gameObject.SetActive(true);
                break;
        }
    }

    private void OnDestroy()
    {
        TimerManagerDataHandler.OnSendTimeLevel -= OnSendTimeLevel;
    }
}
