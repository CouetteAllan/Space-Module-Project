using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObstaclesManagerDataHandler
{
    public static event Action<Vector2> OnSpawnObstacle;
    public static void SpawnObstacles(Vector2 position) => OnSpawnObstacle?.Invoke(position);
    
}
