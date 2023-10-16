using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : Singleton<PlayerStatsManager>
{
    private PlayerStats _playerStats;

    public void InitPlayerStats(PlayerStats playerStats)
    {
        _playerStats = new PlayerStats
        {
            Armor = 0,
            Health = 50,
            Speed = 10,
            Lvl = 1,
        };
    }

    public void UpdatePlayerStats()
    {

    }

    public PlayerStats GetCurrentPlayerStats()
    {
        return _playerStats;
    }
}

public class PlayerStats
{
    public uint Health;
    public uint Speed;
    public uint Armor;
    public uint Lvl;

}
