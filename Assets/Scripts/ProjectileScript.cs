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

    private Rigidbody2D _rigidbody;

    public Transform Transform => this.transform;

    public float RecoilMultiplier => 1.2f;

    private float _damage;

    public void Launch(Vector2 dir, float speed, float damage, Transform modTransform = null)
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = dir * speed;
        switch (_type)
        {
            case ProjectileType.Bullet:
                Invoke("Die", 2.0f);
                break;
            case ProjectileType.Rocket:
                Invoke("Blow", 1.0f);
                break;
            case ProjectileType.Drone:
                //Drone revolve around the module
                RevolveAroundModule(modTransform);
                break;
        }
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

    private void RevolveAroundModule(Transform modTransform)
    {
        StartCoroutine(RevolveCoroutine(modTransform));
    }

    IEnumerator RevolveCoroutine(Transform modTransform)
    {
        Vector3 offset = modTransform.position + modTransform.forward * 3.0f;

        while ((this.transform.position - offset).sqrMagnitude > 0.5f)
        {
            _rigidbody.AddForce((offset - transform.position) * 2.0f,ForceMode2D.Impulse);
            yield return null;
        }

        //Revolve 

        float time = Time.time;
        float timeRevolve = 5.0f;

        while (Time.time < time + timeRevolve)
        {
            transform.RotateAround(modTransform.position, new Vector3(0, 0, 1), 40.0f * Time.deltaTime);
            yield return null;
        }

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
