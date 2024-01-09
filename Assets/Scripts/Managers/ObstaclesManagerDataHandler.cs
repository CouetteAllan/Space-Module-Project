using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObstaclesManagerDataHandler
{
    public static event Action<Vector3> OnSpawnObstacle;
    public static void SpawnObstacles(Vector3 position) => OnSpawnObstacle?.Invoke(position);
    
}
