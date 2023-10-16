using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{

    [SerializeField] private Transform[] _firePoints;

    public enum ModuleClass
    {
        Offense,
        SpecialOffense,
        Defense,
        Engine,
        Placement
    }
    private ModuleClass _moduleClass;
    private ModuleDatas _data;

    public static Module CreateMod(Vector2 position, ModuleDatas datas, Transform parentTransform)
    {
        GameObject go = new GameObject();
        Module module = Instantiate(datas.ModulePrefab.GetComponent<Module>(),position,parentTransform.rotation,parentTransform);
        module.SetUpModule(datas);
        return module;
    }

    private void SetUpModule( ModuleDatas datas)
    {
        _data = datas;
        _moduleClass = datas.ModuleClass;
        this.gameObject.name = datas.ModuleName;
        if(_moduleClass == ModuleClass.Offense)
            TimeTickSystemDataHandler.OnTick += TimeTickSystemDataHandler_OnTick;
    }

    private void TimeTickSystemDataHandler_OnTick(uint tick)
    {
        if(tick % 4 == 0)
        {
            Fire();
        }
    }

    private void Fire()
    {
        //Instantiate projectile prefab from module prefab
        foreach(Transform t in _firePoints)
        {
            var projectile = Instantiate(_data.ProjectilePrefab, t.position,this.transform.rotation).GetComponent<ProjectileScript>();
            projectile.Launch((t.position - this.transform.position).normalized, 6.0f);

        }
    }


    private void OnDisable()
    {
        TimeTickSystemDataHandler.OnTick -= TimeTickSystemDataHandler_OnTick;

    }
}
