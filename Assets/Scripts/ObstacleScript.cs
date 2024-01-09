using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour, IObstacle
{
    [SerializeField] private Rigidbody2D _rb;
    private ObstaclesManager _manager;
    private int _health;
    
    public void TryHit(IDamageSource source, int damage)
    {
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
        StartCoroutine(RotationCoroutine());
        _health = health;
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
            this.transform.Rotate(0, 0, 20.0f * Time.deltaTime);
            yield return null;
        }
    }
}
