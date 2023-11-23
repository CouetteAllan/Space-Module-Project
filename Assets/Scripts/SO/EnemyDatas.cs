using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New enemy",menuName = "Enemy")]
public class EnemyDatas : ScriptableObject
{

    public enum EnemyTier
    {
        Tier1 = 1,
        Tier2 = 2,
        Tier3 = 3,
        Tier4 = 4,
    }

    [Header("Base Stat")]
    public EnemyTier Tier;
    public float BaseHealth;
    public float BaseDamage;
    public float BaseSpeed;
    public GameObject EnemyPrefab;

    [Space]
    [Header("Loot")]
    public int XPGranted;
    public int ScrapMetalGranted;

}
