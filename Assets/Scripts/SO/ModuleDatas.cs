using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Mod",menuName = "Modules/Module")]
public class ModuleDatas : ScriptableObject
{
    public string ModuleName = "New Module Name";
    public Module.ModuleClass ModuleClass = Module.ModuleClass.Offense;
    public Sprite ModuleSprite = null;
    public GameObject ModulePrefab = null;
    public GameObject ProjectilePrefab = null;
    public BuffDatas BuffDatas = null;
}
