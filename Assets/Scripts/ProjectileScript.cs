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
        _damage = damage;
        switch (_type)
        {
            case ProjectileType.Bullet:
                Invoke("Die", 2.0f);
                _rigidbody.velocity = dir * speed;

                break;
            case ProjectileType.Rocket:
                Invoke("Blow", 1.0f);
                _rigidbody.velocity = dir * speed;

                break;
            case ProjectileType.Drone:
                //Drone revolve around the module
                RevolveAroundModule(modTransform);
                break;
        }
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
        Vector3 offset = modTransform.position + (modTransform.up * 6.0f);
        while (Vector2.Distance(offset,this.transform.position) > 0.05f)
        {
            Vector2 dir = transform.position - offset;
            transform.position += (Vector3)(-dir.normalized) * 20.0f * Time.deltaTime;
            offset = modTransform.position + modTransform.up * 6.0f;
            yield return null;
        }

        //Revolve 

        float time = Time.time;
        float timeRevolve = 7.0f;

        while (Time.time < time + timeRevolve)
        {
            transform.parent.Rotate(Vector3.forward * 100.0f* Time.deltaTime);
            
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_type != ProjectileType.Drone)
            return;
        if (collision.gameObject.TryGetComponent<IHittable>(out IHittable objectHit))
        {
            objectHit.TryHit(this, (int)_damage);
        }
    }


}
