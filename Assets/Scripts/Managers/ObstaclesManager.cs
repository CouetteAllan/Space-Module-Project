using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesManager : MonoBehaviour
{
    [SerializeField] private GameObject _obstaclePrefab;
    [SerializeField] private int _maxObstacleOnField;
    private int _currentNumberObstacle;
    private Queue<GameObject> _obstacleQueue = new Queue<GameObject>();

    private void Awake()
    {
        ObstaclesManagerDataHandler.OnSpawnObstacle += OnSpawnObstacle;
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
}
