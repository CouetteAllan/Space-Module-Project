using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PowerUpManagerDataHandler
{
    public static event Action<Vector2> OnCreatePowerUpAtLocation;
    public static void CreatePowerUp(Vector2 vector2) => OnCreatePowerUpAtLocation?.Invoke(vector2);
}
