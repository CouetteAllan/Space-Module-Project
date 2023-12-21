using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class ObstaclesManager : MonoBehaviour
{
    [SerializeField] private GameObject _obstaclePrefab;
    [SerializeField] private int _maxObstacleOnField;
    private int _currentNumberObstacle;
    private Queue<GameObject> _obstacleQueue = new Queue<GameObject>();

    private void Awake()
    {
        ObstaclesManagerDataHandler.OnSpawnObstacle += OnSpawnObstacle;
        TimeTickSystemDataHandler.OnTick += OnTick;
    }

    private void OnTick(uint currentTick)
    {
        if(currentTick % 100 == 0)
        {
            Vector2 playerPos = GameManager.Instance.PlayerController.transform.position;
            float distanceFromPlayer = 16.0f;
            Vector2 randomPos = playerPos + (Vector2)UtilsClass.GetRandomDir() * distanceFromPlayer;
            OnSpawnObstacle(randomPos);
        }
    }

    private void OnSpawnObstacle(Vector2 pos)
    {
        if (_currentNumberObstacle >= _maxObstacleOnField)
        {
            if(_obstacleQueue.TryDequeue(out GameObject obstacle))
            {
                Destroy(obstacle);
                _currentNumberObstacle--;
            }
        }
        _currentNumberObstacle++;
        GameObject newObstacle = Instantiate(_obstaclePrefab, pos, Quaternion.identity);
        _obstacleQueue.Enqueue(newObstacle);

    }

    private void OnDisable()
    {
        ObstaclesManagerDataHandler.OnSpawnObstacle -= OnSpawnObstacle;
    }
}
