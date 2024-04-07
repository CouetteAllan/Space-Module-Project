using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBossParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particleSystems;
    [SerializeField] private BossScript _bossScript;

    public void PlayParticles()
    {
        foreach (var particleSystem in _particleSystems)
        {
            particleSystem.Play();
        }
    }

    public void ActivateBossHitBoxes()
    {
        _bossScript.ActivateHitBoxes(true);
    }public void DeactivateBossHitBoxes()
    {
        _bossScript.ActivateHitBoxes(false);
    }
}
