using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ProjectileScript : MonoBehaviour, IDamageSource
{

    public enum ProjectileType
    {
        Bullet,
        Rocket,
        Drone
    }
    [SerializeField] private ProjectileType _type;
    [SerializeField] private ParticleSystem _deathParticles;

    public Transform Transform => this.transform;

    public float RecoilMultiplier => 1.2f;

    private float _damage;

    public void Launch(Vector2 dir, float speed, float damage)
    {
        this.GetComponent<Rigidbody2D>().velocity = dir * speed;
        if (_type == ProjectileType.Bullet)
            Invoke("Die", 2.0f);
        else
            Invoke("Blow", 1.0f);
        _damage = damage;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void Blow()
    {
        var colls = Physics2D.OverlapCircleAll(this.transform.position, 6.0f);
        foreach (var coll in colls)
        {
            if(coll.TryGetComponent(out IHittable hittable))
            {
                hittable.TryHit(this, (int)_damage);
            }
        }
        //Play particles
        FXManager.Instance.PlayEffect("rocketBlow", this.transform.position);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<IHittable>(out IHittable objectHit))
        {
            objectHit.TryHit(this,(int)_damage);
            Destroy(gameObject);
        }
    }


}
