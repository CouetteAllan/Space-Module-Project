using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StatSystem : Singleton<StatSystem>
{
    [SerializeField] private BaseStatDatas _baseStatDatas;

    private StatClass _playerStat;
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
        if(newState == GameState.BeforeGameStart)
        {
            if(_playerStat == null)
                _playerStat = new StatClass(_baseStatDatas);

            GameManager.Instance.ChangeGameState(GameState.StartGame);
        }
    }


    private void Update()
    {
        if (Keyboard.current.uKey.wasPressedThisFrame)
        {
            _playerStat.MultiplyPercentStat(StatType.ReloadSpeed, 0.2f);
        }
    }
}
