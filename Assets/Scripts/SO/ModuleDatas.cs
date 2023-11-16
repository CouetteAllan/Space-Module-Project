using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Mod",menuName = "Modules/Module")]
public class ModuleDatas : ScriptableObject
{
    [Header("General Module Settings")]
    public string ModuleName = "New Module Name";
    public Module.ModuleClass ModuleClass = Module.ModuleClass.Offense;
    public Sprite ModuleSprite = null;
    public GameObject ModulePrefab = null;

    [Space]
    [Header("Offensive")]
    public GameObject ProjectilePrefab = null;
    public OffensiveModuleDatas OffensiveModuleDatas = null;

    [Space]
    [Header("Buff")]
    public BuffDatas BuffDatas = null;
}
