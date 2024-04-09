using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private BuffDatas[] _buffDatas;
    [SerializeField] private PowerUpObject _powerUpPrefab;
    [SerializeField] private int _maxPowerUpInWorld = 5;

    private List<PowerUpObject> _powerUpObjects = new List<PowerUpObject>();

    private void Start()
    {
        PowerUpManagerDataHandler.OnCreatePowerUpAtLocation += OnCreatePowerUp;
        EnemyScript.OnDeath += EnemyScript_OnDeath;
    }

    private void EnemyScript_OnDeath(object sender, EnemyScript.EnemyStatsOnDeath enemyStats)
    {
        if (Utils.RollChance(chance: .015f)) //1.5% chances to drop Buff
        {
            OnCreatePowerUp(enemyStats.finalPos);
        }
    }

    private void OnCreatePowerUp(Vector2 vector2)
    {
        if (_powerUpObjects.Count >= _maxPowerUpInWorld)
            return;

        //Instantiate a random powerUp at location
        var randomIndex = Random.Range(0, _buffDatas.Length);
        BuffDatas randomBuff = _buffDatas[randomIndex];
        PowerUpObject createdPowerUp = Instantiate(_powerUpPrefab,vector2,Quaternion.identity);
        createdPowerUp.SetUpObject(this, randomBuff);

        _powerUpObjects.Add(createdPowerUp);
    }

    public void RemoveObjectFromList(PowerUpObject powerUpObject)
    {
        _powerUpObjects.Remove(powerUpObject);
    }

    private void OnDisable()
    {
        PowerUpManagerDataHandler.OnCreatePowerUpAtLocation -= OnCreatePowerUp;
        EnemyScript.OnDeath -= EnemyScript_OnDeath;
    }
}
