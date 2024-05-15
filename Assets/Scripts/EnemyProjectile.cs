using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyProjectile : MonoBehaviour, IDamageSource, IHittable
{

    private Rigidbody2D _rigidbody;

    public Transform Transform => this.transform;

    public float RecoilMultiplier { get; set; } = 1.2f;

    private float _damage;

    public void Launch(Vector2 dir, float speed, float damage, Transform modTransform = null)
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _damage = damage;
        Invoke("Die", 3.0f);
        _rigidbody.velocity = dir * speed;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<HealthScript>(out HealthScript objectHit))
        {
            objectHit.ChangeHealth(-(int)_damage);
            Destroy(gameObject);
        }

        if(collision.gameObject.TryGetComponent<ProjectileScript>(out ProjectileScript projectileScript))
        {
            Destroy(gameObject);
        }
    }

    public void TryHit(IDamageSource source, int damage)
    {
        Destroy(gameObject);
    }
}