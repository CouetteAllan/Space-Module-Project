using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New wave Setting",menuName = "Wave")]
public class WaveSO : ScriptableObject
{
    public Vector2 OffsetPosFromPlayer;
    public float TimeInSeconds;
    public WaveComponent[] WaveComponents;

}

[Serializable]
public class WaveComponent
{
    public EnemyDatas enemy;
    public int number;
}
