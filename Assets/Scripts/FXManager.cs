using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : Singleton<FXManager>
{
    [SerializeField] private GameObject _explosionFX;
    [SerializeField] private GameObject _blowFX;

    private void Start()
    {
        Module.OnModuleDestroyed += Module_OnModuleDestroyed;
    }

    private void Module_OnModuleDestroyed(Module mod)
    {
        Instantiate(_explosionFX,mod.transform.position,Quaternion.identity);
    }

    public void PlayEffect(string effect, Vector2 pos)
    {
        switch(effect)
        {
            case "rocketBlow":
                Instantiate(_blowFX,pos,Quaternion.identity);
                break;
        }
    }
}
