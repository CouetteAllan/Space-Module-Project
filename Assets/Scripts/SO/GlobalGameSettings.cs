using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Global Settings", menuName = "Global Game Settings")]
public class GlobalGameSettings : ScriptableObject
{
    [Header("Player Base Settings")]
    public int StartMetalScrap = 0;

    [Range(0.1f,10.0f)]
    public float PlayerDamageMultplier = 1.0f;

    [Range(10.0f,200.0f)]
    public float PlayerBaseHealth = 100.0f;

    [Header("Enemy Base Settings")]
    [Range(0.1f, 10.0f)]
    public float EnemyDamageMultplier = 1.0f;
}
