using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System.Linq;

public class ObstaclesManager : MonoBehaviour
{
    [SerializeField] private GameObject _obstaclePrefab;
    [SerializeField] private int _maxObstacleOnField;
    [SerializeField] private int _obstacleHealth;
    [SerializeField] private bool _debug = false;
    private int _currentNumberObstacle;
    private List<ObstacleScript> _obstacleList = new List<ObstacleScript>();

    private void Awake()
    {
        ObstaclesManagerDataHandler.OnSpawnObstacle += OnSpawnObstacle;
        TimeTickSystemDataHandler.OnTick += OnTick;
    }

    private void OnTick(uint currentTick)
    {
        int tickNeeded = _debug ? 20 : 55;
        if(currentTick % tickNeeded == 0)
        {
            Vector3 playerPos = GameManager.Instance.PlayerController.transform.position;
            float distanceFromPlayer = 28.0f;
            Vector3 randomPos = playerPos + UtilsClass.GetRandomDir() * distanceFromPlayer;
            ObstaclesManagerDataHandler.SpawnObstacles(randomPos);
        }
    }

    private void OnSpawnObstacle(Vector3 pos)
    {
        int maxObstacle = _debug ? 2 : _maxObstacleOnField;
        if (_currentNumberObstacle >= maxObstacle)
        {
            ObstacleScript farthestObstacle = FindFarthestObstacle();
            _currentNumberObstacle--;
            _obstacleList.Remove(farthestObstacle);
            Destroy(farthestObstacle.gameObject);
        }
        _currentNumberObstacle++;
        var newObstacle = Instantiate(_obstaclePrefab, pos, Quaternion.identity).GetComponent<ObstacleScript>();
        _obstacleList.Add(newObstacle);
        int obstacleHealth = _obstacleHealth * (int)GameManager.Instance.CurrentLevel;
        newObstacle.SetUpObstacle(this, obstacleHealth);
    }

    public void DestroyObstacle(ObstacleScript obstacle)
    {
        _currentNumberObstacle--;
        _obstacleList.Remove(obstacle);
    }

    private ObstacleScript FindFarthestObstacle()
    {
        ObstacleScript farthestObstacle = _obstacleList[0];
        Transform player = GameManager.Instance.PlayerController.transform;
        foreach (var obstacle in _obstacleList)
        {
            if(Vector2.SqrMagnitude((Vector2)player.position - (Vector2)obstacle.transform.position) > Vector2.SqrMagnitude((Vector2)player.position - (Vector2)farthestObstacle.transform.position))
            {
                farthestObstacle = obstacle;
            }
        }

        return farthestObstacle;
    }

    private void OnDisable()
    {
        ObstaclesManagerDataHandler.OnSpawnObstacle -= OnSpawnObstacle;
        TimeTickSystemDataHandler.OnTick -= OnTick;

    }
}
