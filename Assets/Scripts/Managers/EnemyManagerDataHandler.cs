using System;
using UnityEngine;

public static class EnemyManagerDataHandler
{
    public static event Func<Vector2, EnemyDatas, EnemyScript> OnSpawnEnemy;
    public static event Func<EnemyDatas> OnGetEnemyDatas;
    public static event Action<EnemyDatas, int> OnSpawnEnemyWave;
    public static event Action OnBossSpawned;
    public static event Action<Action> OnTriggerBossCinematic;
    public static event Action OnShowBoss;
    public static event Action OnBossDeath;

    public static EnemyScript SpawnEnemy(Vector2 position, EnemyDatas data) => OnSpawnEnemy?.Invoke(position, data);
    public static EnemyDatas GetEnemyDatas() => OnGetEnemyDatas?.Invoke();
    public static void SpawnEnemyWave(EnemyDatas data, int number) => OnSpawnEnemyWave?.Invoke(data, number);
    public static void BossSpawned(this EnemyManager manager) => OnBossSpawned?.Invoke();
    public static void TriggerBossCinematic(this EnemyManager manager, Action onEndCinematic) => OnTriggerBossCinematic?.Invoke(onEndCinematic);
    public static void BossDeath(this EnemyManager manager) => OnBossDeath?.Invoke();
    public static void ShowBoss() => OnShowBoss?.Invoke();
}
