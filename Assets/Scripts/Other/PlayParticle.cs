
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particleSystems;
    [SerializeField] private Transform _laserTargetTransform;
    [SerializeField] private Transform _laserTargetEndTransform;
    private Module _module = null;
    private Coroutine _moveCoroutine;
    public void SetUpPlayParticle(Module mod)
    {
        _module = mod;
        mod.OnModuleFire += Mod_OnModuleFire;
    }

    private void Mod_OnModuleFire()
    {
        /*_particleSystem.Stop();*/
        foreach (var particles in  _particleSystems)
        {
            particles.Play();

        }
        if (_laserTargetTransform != null)
        {
            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);
            foreach (var particles in _particleSystems)
            {
                particles.Stop();
            }
            _moveCoroutine = StartCoroutine(MoveTargetCoroutine());
        }
    }


    private void OnDisable()
    {
        if(_module != null)
            _module.OnModuleFire -= Mod_OnModuleFire;
    }

    IEnumerator MoveTargetCoroutine()
    {
        foreach (var particles in _particleSystems)
        {
            particles.Stop();
        }
        _laserTargetTransform.position = transform.parent.position;
        foreach (var particles in _particleSystems)
        {
            particles.Play();

        }
        yield return null;
        yield return null;
        _laserTargetTransform.position = Vector3.Lerp(transform.parent.position,_laserTargetEndTransform.position,0.7f);
        yield return null;
        _laserTargetTransform.position = _laserTargetEndTransform.position;
        yield return null;
        yield return null;
        foreach (var particles in _particleSystems)
        {
            particles.Stop();
        }
    }
}
