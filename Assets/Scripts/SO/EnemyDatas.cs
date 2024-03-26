using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New enemy",menuName = "Enemy/Basic")]
public class EnemyDatas : ScriptableObject
{

    public enum EnemyTier
    {
        Tier1 = 1,
        Tier2 = 2,
        Tier3 = 3,
        Tier4 = 4,
        Tier5 = 5,
    }

    public enum EnemyType
    {
        Melee,
        Ranged,
        Boss
    }

    [Header("Base Stat")]
    public EnemyTier Tier = EnemyTier.Tier1;
    public EnemyType Type = EnemyType.Melee;
    public float BaseHealth;
    public float HealthMultplier;
    public float BaseDamage;
    public float BaseSpeed;
    [Range(1.0f,3.0f)]
    public float TurnRate = 1.0f;
    public float RecoilResistanceb = 1.0f;
    public GameObject EnemyPrefab;
    public EnemyProjectile ProjectilePrefab;

    [Space]
    [Header("Loot")]
    public int XPGranted;
    public int ScrapMetalGranted;

    public IEnemyBehaviour GetEnemyBehaviour(Rigidbody2D enemyRB)
    {
        switch (Type)
        {
            default:
            case EnemyType.Melee:
                return new EnemyMeleeBehaviour(BaseSpeed,enemyRB);
            case EnemyType.Ranged:
                return new EnemyRangedBehaviour(BaseSpeed, enemyRB, ProjectilePrefab, BaseDamage);
            case EnemyType.Boss:
                return new EnemyMeleeBehaviour(BaseSpeed, enemyRB); ;
        }
    }

}
