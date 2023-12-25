using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour, IObstacle
{
    [SerializeField] private Rigidbody2D _rb;
    private ObstaclesManager _manager;
    private int _health = 30;
    
    public void TryHit(IDamageSource source, int damage)
    {
        //Feedback hit
        //Add force ?
        Vector2 pushForceDir = _rb.position - (Vector2)source.Transform.position;
        float recoilForce = source.RecoilMultiplier * 2.0f;
        _rb.velocity += pushForceDir.normalized * recoilForce;
        ChangeHealth(-damage);
    }


    private void ChangeHealth(int damage)
    {
        _health += damage;
        if (_health <= 0)
            DestroyObstacle();
    }

    public void SetUpObstacle(ObstaclesManager manager)
    {
        _manager = manager;
    }

    private void DestroyObstacle()
    {
        _manager.DestroyObstacle(this);
    }
}
