using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    [SerializeField] private GameObject _explosionFX;

    private void Start()
    {

        Module.OnModuleDestroyed += Module_OnModuleDestroyed;
    }

    private void Module_OnModuleDestroyed(Module mod)
    {
        Instantiate(_explosionFX,mod.transform.position,Quaternion.identity);
    }
}
