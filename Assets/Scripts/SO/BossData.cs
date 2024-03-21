using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Boss Datas", menuName = "Enemy/Boss")]
public class BossData : EnemyDatas
{
    //some datas about the boss behaviour (range, attack rate, attack type, behaviour)

    [Header("Boss Stats")]
    public float TimeNextAttack = 8.0f;
    public int NbEnemiesToInstantiate = 10;

}
