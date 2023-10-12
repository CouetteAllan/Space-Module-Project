using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour,IDamageSource
{
    public void Launch(Vector2 dir, float speed)
    {
        this.GetComponent<Rigidbody2D>().velocity = dir * speed;
        Invoke("Die", 2.0f);
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<IHittable>(out IHittable objectHit))
        {
            objectHit.TryHit(this);
            Destroy(gameObject);
        }
    }


}
