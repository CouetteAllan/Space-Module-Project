using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedBehaviour : IEnemyBehaviour
{
    private float _speed;
    private EnemyProjectile _projectile;
    private float _damages;
    private Rigidbody2D _rigidbody;
    private bool _hasReachedTargetRanged = false;
    private bool _canShoot = true;

    public EnemyRangedBehaviour(float speed, Rigidbody2D rigidbody,EnemyProjectile projectile, float damages)
    {
        _speed = speed;
        _projectile = projectile;  
        _rigidbody = rigidbody;
        _damages = damages;
    }

    public void DoAttack(PlayerController player)
    {
        //Stop and Fire Projectile
    }

    public void DoMovement(PlayerController player, bool isStopped = true)
    {
        //advance toward the player
        Vector2 dir = (Vector2)player.transform.position - this._rigidbody.position;
        if (!isStopped && !_hasReachedTargetRanged)
        {
            _rigidbody.velocity = dir.normalized * _speed;
        }
        else
        {
            _rigidbody.AddForce(-dir.normalized * 10.0f);
        }

        var colliders = Physics2D.OverlapCircleAll(_rigidbody.position, 12.0f);
        foreach (var collider in colliders)
        {
            _hasReachedTargetRanged = false;
            if (collider.TryGetComponent<PlayerController>(out PlayerController playercollider))
            {
                //Shoot, we are in range.
                _hasReachedTargetRanged = true;
                if(_canShoot)
                    MonoBehaviourOnScene.Instance.StartCoroutine(ShootCoroutine(dir));
                break;
            }
        }
    }

    IEnumerator ShootCoroutine(Vector2 dir)
    {
        Shoot(dir);
        _canShoot = false;
        yield return new WaitForSeconds(1.5f);
        _canShoot = true;
    }

    private void Shoot(Vector2 dir)
    {
        EnemyProjectile projectile = Object.Instantiate(_projectile, _rigidbody.position + dir.normalized, Quaternion.identity);
        projectile.Launch(dir.normalized, 5.0f, _damages);
    }
}
