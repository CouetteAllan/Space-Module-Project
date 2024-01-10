using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour, IObstacle
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private bool _isIndestructible = false;
    private ObstaclesManager _manager;
    private int _health;
    private int _baseHealth = 30;
    
    public void TryHit(IDamageSource source, int damage)
    {
        if (_isIndestructible)
            return;
        //Feedback hit
        //Add force ?
        Vector2 pushForceDir = _rb.position - (Vector2)source.Transform.position;
        float recoilForce = source.RecoilMultiplier * 2.0f;
        _rb.AddForce(pushForceDir * recoilForce);
        ChangeHealth(-damage);
    }


    private void ChangeHealth(int damage)
    {
        _health += damage;
        if (_health <= 0)
            DestroyObstacle();
    }

    public void SetUpObstacle(ObstaclesManager manager,int health)
    {
        _manager = manager;
        _health = health + _baseHealth;
        StartCoroutine(RotationCoroutine());
    }

    private void DestroyObstacle()
    {
        _manager.DestroyObstacle(this);
        ScrapManagerDataHandler.CreateScrap(this.transform.position, 8);
        //play fx
        FXManager.Instance.PlayEffect("explosion", this.transform.position, Quaternion.identity);
        StopAllCoroutines();
        Destroy(gameObject);
    }

    private IEnumerator RotationCoroutine()
    {
        while (true)
        {
            this.transform.Rotate(0, 0, 25.0f * Time.deltaTime);
            yield return null;
        }
    }
}
