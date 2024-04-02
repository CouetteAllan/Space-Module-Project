
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
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
        _particleSystem.Play();
        if (_laserTargetTransform != null)
        {
            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);
            _particleSystem.Stop();
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
        _particleSystem.Stop();
        _laserTargetTransform.position = transform.parent.position;
        _particleSystem.Play();
        yield return null;
        yield return null;
        _laserTargetTransform.position = Vector3.Lerp(transform.parent.position,_laserTargetEndTransform.position,0.7f);
        yield return null;
        _laserTargetTransform.position = _laserTargetEndTransform.position;
        yield return null;
        yield return null;
        _particleSystem.Stop();
    }
}
