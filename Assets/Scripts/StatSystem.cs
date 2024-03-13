using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StatSystem : Singleton<StatSystem>
{
    [SerializeField] private BaseStatDatas _baseStatDatas;

    private StatClass _playerStat = null;
    public StatClass PlayerStat {
        get
        {
            if( _playerStat == null )
                _playerStat = new StatClass(_baseStatDatas);
            return _playerStat;
        }
    }

    public void Start()
    {
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(GameState newState)
    {
        if(newState == GameState.StartGame)
        {
            _playerStat = new StatClass(_baseStatDatas);
        }
    }



    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;

    }
}
