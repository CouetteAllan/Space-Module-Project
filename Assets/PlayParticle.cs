using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    private Module _module;
    public void SetUpPlayParticle(Module mod)
    {
        _module = mod;
        mod.OnModuleFire += Mod_OnModuleFire;
    }

    private void Mod_OnModuleFire()
    {
        _particleSystem.Stop();
        _particleSystem.Play();
    }

    private void OnDisable()
    {
        _module.OnModuleFire -= Mod_OnModuleFire;
    }
}
