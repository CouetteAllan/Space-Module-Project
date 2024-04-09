using System;
using UnityEngine;

public class GameStatsManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static int _nbOfEnemiesKilled = 0;
    void Awake()
    {
        EnemyScript.OnDeath += OnDeath;
        _nbOfEnemiesKilled = 0;
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState)
    {
        if(newState == GameState.StartGame)
        {
            _nbOfEnemiesKilled = 0;
        }
    }

    private void OnDeath(object sender, EnemyScript.EnemyStatsOnDeath e)
    {
        _nbOfEnemiesKilled++;
    }



    private void OnDisable()
    {
        EnemyScript.OnDeath -= OnDeath;
        GameManager.OnGameStateChanged -= OnGameStateChanged;

    }
}
