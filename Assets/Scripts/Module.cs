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
    private AttachPointScript _attachPoint;

    public static Module CreateMod(Vector2 position, ModuleDatas datas, Transform parentTransform)
    {
        GameObject go = new GameObject();
        Module module = Instantiate(datas.ModulePrefab.GetComponent<Module>(),position,parentTransform.rotation,parentTransform);
        module.SetUpModule(datas, parentTransform.GetComponent<AttachPointScript>());
        return module;
    }

    private void SetUpModule( ModuleDatas datas, AttachPointScript attachPointScript)
    {
        _data = datas;
        _moduleClass = datas.ModuleClass;
        this.gameObject.name = datas.ModuleName;
        _attachPoint = attachPointScript;
        if(_moduleClass == ModuleClass.Offense)
            TimeTickSystemDataHandler.OnTick += TimeTickSystemDataHandler_OnTick;
        if(_moduleClass == ModuleClass.Placement)
        {
            var healthScript = this.gameObject.AddComponent<HealthScript>();
            healthScript.SetAttachPoint(attachPointScript);
        }
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
