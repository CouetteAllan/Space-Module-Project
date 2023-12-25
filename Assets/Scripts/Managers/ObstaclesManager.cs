using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System.Linq;

public class ObstaclesManager : MonoBehaviour
{
    [SerializeField] private ObstacleScript _obstaclePrefab;
    [SerializeField] private int _maxObstacleOnField;
    private int _currentNumberObstacle;
    private List<ObstacleScript> _obstacleList = new List<ObstacleScript>();

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
            var firstObstacle = _obstacleList.First();
            if (firstObstacle != null)
            {
                Destroy(firstObstacle);
                _currentNumberObstacle--;
            }
        }
        _currentNumberObstacle++;
        ObstacleScript newObstacle = Instantiate(_obstaclePrefab, pos, Quaternion.identity);
        _obstacleList.Add(newObstacle);
        newObstacle.SetUpObstacle(this);
    }

    public void DestroyObstacle(ObstacleScript obstacle)
    {
        _currentNumberObstacle--;
        _obstacleList.Remove(obstacle);
        Destroy(obstacle);
    }

    private void OnDisable()
    {
        ObstaclesManagerDataHandler.OnSpawnObstacle -= OnSpawnObstacle;
    }
}
