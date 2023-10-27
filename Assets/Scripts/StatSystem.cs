using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StatSystem : Singleton<StatSystem>
{
    private StatClass _playerStat;
    public StatClass PlayerStat { get { return _playerStat; } }

    public void Start()
    {
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(GameState newState)
    {
        if(newState == GameState.BeforeGameStart)
        {
            _playerStat = new StatClass();

            GameManager.Instance.ChangeGameState(GameState.StartGame);
        }
    }


    private void Update()
    {
        if (Keyboard.current.uKey.wasPressedThisFrame)
        {
            _playerStat.ChangeStat(StatType.ReloadSpeed, 1);
            Debug.Log("reload up");
        }

        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            Debug.Log("Reload speed is: " + _playerStat.GetStatValue(StatType.ReloadSpeed));
        }
    }
}
