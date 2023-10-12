using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour, IHittable
{
    //simple ennemi qui avance pour le proto et quand il meurt ça donne de l'xp
    private Rigidbody2D _rigidbody;
    private PlayerController _playerController;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private GameObject _particleEffect;

    private void Start()
    {
        SetUpEnemy();
    }


    public void SetUpEnemy()
    {
        this._rigidbody = GetComponent<Rigidbody2D>();
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
        _playerController = GameManager.Instance.PlayerController;
        Vector2 dir = (Vector2)_playerController.transform.position - this._rigidbody.position;
        this._rigidbody.velocity = dir.normalized * speed;
    }

    private void GameManager_OnGameStateChanged(GameState newState)
    {
        if(newState == GameState.StartGame)
        {
            
        }
    }

    public void TryHit(IDamageSource source)
    {
        GameManager.Instance.GrantXP(5);
        Die();
    }

    private void Die()
    {
        Instantiate(_particleEffect,this.transform.position,Quaternion.identity);
        Destroy(this.gameObject);

    }

}
