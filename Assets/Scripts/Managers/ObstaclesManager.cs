using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System.Linq;

public class ObstaclesManager : MonoBehaviour
{
    [SerializeField] private GameObject _obstaclePrefab;
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
            Vector3 playerPos = GameManager.Instance.PlayerController.transform.position;
            float distanceFromPlayer = 20.0f;
            Vector3 randomPos = playerPos + UtilsClass.GetRandomDir() * distanceFromPlayer;
            ObstaclesManagerDataHandler.SpawnObstacles(randomPos);
        }
    }

    private void OnSpawnObstacle(Vector3 pos)
    {
        if (_currentNumberObstacle >= _maxObstacleOnField)
        {
            ObstacleScript firstObstacle = _obstacleList.First();
            if (firstObstacle != null)
            {
                Destroy(firstObstacle);
                _currentNumberObstacle--;
            }
        }
        _currentNumberObstacle++;
        var newObstacle = Instantiate(_obstaclePrefab, pos, Quaternion.identity).GetComponent<ObstacleScript>();
        _obstacleList.Add(newObstacle);
        newObstacle.SetUpObstacle(this);
    }

    public void DestroyObstacle(ObstacleScript obstacle)
    {
        _currentNumberObstacle--;
        _obstacleList.Remove(obstacle);
    }

    private void OnDisable()
    {
        ObstaclesManagerDataHandler.OnSpawnObstacle -= OnSpawnObstacle;
        TimeTickSystemDataHandler.OnTick -= OnTick;

    }
}
