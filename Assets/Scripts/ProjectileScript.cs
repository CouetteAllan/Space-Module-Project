using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour,IDamageSource
{
    public Transform Transform => this.transform;

    private float _damage;
    private int _baseDamage = 10;

    public void Launch(Vector2 dir, float speed, float damageMult = 1)
    {
        this.GetComponent<Rigidbody2D>().velocity = dir * speed;
        Invoke("Die", 2.0f);
        _damage = _baseDamage * damageMult;
    }

    private void Die()
    {
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
