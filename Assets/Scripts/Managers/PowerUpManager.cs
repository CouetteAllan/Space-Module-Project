using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private List<PowerUpObject> _powerUpObjects = new List<PowerUpObject>();

    private void Start()
    {
        PowerUpManagerDataHandler.OnCreatePowerUpAtLocation += OnCreatePowerUp;
    }

    private void OnCreatePowerUp(Vector2 vector2)
    {
        //Instantiate a random powerUp at location
    }
}
