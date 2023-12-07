using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeBehaviour : IEnemyBehaviour
{
    private float _speed;
    private Rigidbody2D _rigidbody;
    public EnemyMeleeBehaviour(float speed,Rigidbody2D rb)
    {
        _speed = speed;
        _rigidbody = rb;
    }

    public void DoAttack(PlayerController player)
    {

    }

    public void DoMovement(PlayerController player, bool isStopped = false)
    {
        if (!isStopped)
        {
            Vector2 dir = (Vector2)player.transform.position - this._rigidbody.position;
            _rigidbody.velocity = dir.normalized * _speed;
        }
        else
        {
            Vector2 dir = (Vector2)player.transform.position - this._rigidbody.position;
            _rigidbody.AddForce(-dir.normalized * 10.0f);
        }
        
    }
}
