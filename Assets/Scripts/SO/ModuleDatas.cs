using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Mod",menuName = "Modules/Module")]
public class ModuleDatas : ScriptableObject
{
    [Header("General Module Settings")]
    public string ModuleName = "New Module Name";
    public string ModuleDescription = "New Module description";
    public Module.ModuleClass ModuleClass = Module.ModuleClass.Offense;
    public Sprite ModuleSprite = null;
    public GameObject ModulePrefab = null;
    public enum WeightType
    {
        None = 0,
        Light = 10,
        Medium = 20,
        Heavy = 40,
    }
    public WeightType Weight = WeightType.None;
    public int ScrapCost = 0;
    public int Health = 20;

    [Space]
    [Header("Offensive")]
    public GameObject ProjectilePrefab = null;
    public OffensiveModuleDatas OffensiveModuleDatas = null;

    [Space]
    [Header("Buff")]
    public BuffDatas BuffDatas = null;

    [Space]
    [Header("Other")]
    public SecondaryModuleDatas SecondaryModuleDatas = null;
}
