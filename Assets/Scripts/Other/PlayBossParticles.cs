using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBossParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particleSystems;

    public void PlayParticles()
    {
        foreach (var particleSystem in _particleSystems)
        {
            particleSystem.Play();
        }
    }
}
