using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ProjectileScript : MonoBehaviour, IDamageSource, IProjectile
{

    [SerializeField] private ParticleSystem _deathParticles;
    [SerializeField] private SpriteRenderer[] _sprites;
    [SerializeField] private GameObject[] _graphs;
    [SerializeField] private ProjectileBehaviour _projectileBehaviour;

    public struct ProjectileParameter
    {
        public Vector2 dir;
        public float speed;
        public float damage;
        public float duration;
        public Transform modTransform;
    }


    private Rigidbody2D _rigidbody;

    public Transform Transform => this.transform;

    public float RecoilMultiplier => 1.3f;

    private float _damage;
    private ProjectileParameter _parameter;
    public void Launch(ProjectileParameter parameters)
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _damage = parameters.damage;
        _parameter = parameters;
        _projectileBehaviour.LaunchProjectile(this.gameObject,parameters);

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
            _rigidbody.velocity = Vector3.zero;
            _projectileBehaviour.ProjectileEnd(this.gameObject,_parameter);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<IHittable>(out IHittable objectHit))
        {
            if (!(this._projectileBehaviour is DroneProjectile))
                _projectileBehaviour.ProjectileEnd(this.gameObject, _parameter);
            else
                objectHit.TryHit(this, (int)_damage);

        }
    }


}
