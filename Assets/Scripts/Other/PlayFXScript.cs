using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayFXScript : MonoBehaviour
{
    [SerializeField] private ParticleSystem _fxParticle;
    [SerializeField] private Animator _animator;
    [SerializeField] private TextMeshPro _textMeshPro;

    public void PlayFX(string valueFX)
    {
        _fxParticle.Play();
        _animator.SetTrigger("PickUp");
        _textMeshPro.text = valueFX;
        Invoke("Die", 3.0f);
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
