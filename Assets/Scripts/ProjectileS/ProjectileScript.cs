using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ProjectileScript : MonoBehaviour, IDamageSource, IProjectile
{

    [SerializeField] private ProjectileBehaviour _projectileBehaviour;
    [SerializeField] private AudioSource _audio;

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
        _projectileBehaviour.LaunchProjectile(this,parameters);

    }

    public void PlaySound()
    {
        _audio?.Play();
    }
    

    public void RevolveAroundModule(ProjectileParameter parameters)
    {
        StartCoroutine(RevolveCoroutine(parameters));
    }

    IEnumerator RevolveCoroutine(ProjectileParameter parameters)
    {
        Vector3 offset = parameters.modTransform.position + (parameters.modTransform.up * 6.0f);
        while (Vector2.Distance(offset,this.transform.position) > 0.05f)
        {
            Vector2 dir = transform.position - offset;
            transform.position += (Vector3)(-dir.normalized) * 20.0f * Time.deltaTime;
            offset = parameters.modTransform.position + parameters.modTransform.up * 6.0f;
            yield return null;
        }

        //Revolve 

        float time = Time.time;
        float timeRevolve = 7.0f;

        while (Time.time < time + timeRevolve)
        {
            transform.parent.Rotate(Vector3.forward * 12.0f * parameters.speed * Time.deltaTime);
            
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
            _projectileBehaviour.ProjectileEnd(this,_parameter);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<IHittable>(out IHittable objectHit))
        {
            if (this._projectileBehaviour is DroneProjectile)
                objectHit.TryHit(this, (int)_damage);
        }
    }


}
