using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStatsManagerDataHandler 
{
    public static event Action<int> OnSendNbOfEnemiesKilled;
    public static event Action OnRequestNbOfEnemiesKilled;

    public static void SendNbOfEnemiesKilled(this GameStatsManager manager,int nbOfEnemiesKilled) => OnSendNbOfEnemiesKilled?.Invoke(nbOfEnemiesKilled);
    public static void RequestNbOfEnemiesKilled() => OnRequestNbOfEnemiesKilled?.Invoke();
}
