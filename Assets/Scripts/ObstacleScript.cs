using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour, IObstacle
{
    [SerializeField] private Rigidbody2D _rb;
    private ObstaclesManager _manager;
    private int _health = 3;
    
    public void TryHit(IDamageSource source, int damage)
    {
        //Feedback hit
        //Add force ?
        _rb.AddForce(((Vector2)source.Transform.position - _rb.position) * source.RecoilMultiplier);
        _health--;
        if(_health <=0)
            Destroy(gameObject);
    }

    public void SetUpObstacle(ObstaclesManager manager)
    {
        _manager = manager;
    }
}
