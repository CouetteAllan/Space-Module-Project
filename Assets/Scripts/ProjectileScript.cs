using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ProjectileScript : MonoBehaviour, IDamageSource
{

    [SerializeField] private ProjectileType _type;
    [SerializeField] private ParticleSystem _deathParticles;
    [SerializeField] private SpriteRenderer[] _sprites;
    [SerializeField] private GameObject[] _graphs;

    public enum ProjectileType
    {
        Bullet,
        Rocket,
        Drone
    }

    private Rigidbody2D _rigidbody;

    public Transform Transform => this.transform;

    public float RecoilMultiplier => 1.3f;

    private float _damage;
    private int _currentModuleLevel;
    private float _speed;

    public void Launch(Vector2 dir, float speed, float damage,float duration = 1.0f, Transform modTransform = null, int currentModuleLevel = 1)
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _damage = damage;
        _currentModuleLevel = currentModuleLevel;
        _speed = speed;
        switch (_type)
        {
            case ProjectileType.Bullet:
                Invoke("Die", 2.0f);
                _rigidbody.velocity = dir * speed;

                break;
            case ProjectileType.Rocket:
                if(_currentModuleLevel >= 5)
                {
                    Destroy(_graphs[0]);
                    Instantiate(_graphs[1],this.transform);
                }
                Invoke("Blow", duration);
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
        StartCoroutine(BulletFade());
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
        FXManager.Instance.PlayEffect("rocketBlow", this.transform.position,Quaternion.identity);
        if(_currentModuleLevel >= 5)
        {
            for(int i = 0; i < 2; i++)
            {
                Vector3 dir = i > 0 ? -this.transform.right : this.transform.right;
                Vector3 lookRotation = i> 0 ? -this.transform.up : this.transform.up;
                ProjectileScript proj = Instantiate(this.gameObject, this.transform.position, Quaternion.LookRotation(this.transform.forward,dir)).GetComponent<ProjectileScript>();
                proj.Launch(dir, _speed * 5.0f, _damage / 2,0.3f);
            }
        }
        Destroy(gameObject);
    }

    private void RevolveAroundModule(Transform modTransform)
    {
        StartCoroutine(RevolveCoroutine(modTransform));
    }
    IEnumerator BulletFade()
    {
        float startTime = Time.time;
        float timeDuration = 0.25f;
        while (Time.time < startTime + timeDuration)
        {
            foreach(SpriteRenderer sprite in _sprites)
            {
                Color alpha = sprite.color;
                alpha.a -= 3f * Time.deltaTime;
                sprite.color = alpha;

                sprite.transform.localScale += (new Vector3(2.8f,2.8f,2.8f) * Time.deltaTime);
            }
            yield return null;
        }

        Destroy(this.gameObject);
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
            _rigidbody.velocity = Vector3.zero;
            StartCoroutine(BulletFade());

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
