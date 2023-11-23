using System;
using UnityEngine;

public static class EnemyManagerDataHandler
{
    public static event Func<Vector2, EnemyDatas, EnemyScript> OnSpawnEnemy;
    public static event Func<EnemyDatas> OnGetEnemyDatas;

    public static EnemyScript SpawnEnemy(Vector2 position, EnemyDatas data) => OnSpawnEnemy?.Invoke(position, data);
    public static EnemyDatas GetEnemyDatas() => OnGetEnemyDatas?.Invoke();
}
