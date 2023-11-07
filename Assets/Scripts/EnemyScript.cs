using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour, IHittable
{
    //simple ennemi qui avance pour le proto et quand il meurt ça donne de l'xp
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private GameObject _particleEffect;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Color _baseColor;
    private Rigidbody2D _rigidbody;
    private PlayerController _playerController;

    private int _baseHealth = 1;
    private bool _gotHit = false;
    private int _health;

    private void Start()
    {
        SetUpEnemy();
        _baseColor = _spriteRenderer.color;
    }


    public void SetUpEnemy()
    {
        this._rigidbody = GetComponent<Rigidbody2D>();
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
        _playerController = GameManager.Instance.PlayerController;
        Vector2 dir = (Vector2)_playerController.transform.position - this._rigidbody.position;
        this._rigidbody.velocity = dir.normalized * speed;
        _health = (int)GameManager.Instance.CurrentLevel * _baseHealth;
    }

    private void GameManager_OnGameStateChanged(GameState newState)
    {
        if(newState == GameState.StartGame)
        {
            
        }
    }

    private void Update()
    {
        Vector2 dir = (Vector2)_playerController.transform.position - this._rigidbody.position;
        this._rigidbody.velocity = dir.normalized * speed;

    }

    public void TryHit(IDamageSource source, int damage)
    {
        //recoil + feedback
        if (_gotHit)
            return;
        _gotHit = true;
        _rigidbody.AddForce((Vector2)source.Transform.position - this._rigidbody.position * 5.0f, ForceMode2D.Impulse);
        StartCoroutine(ChangeColorCoroutine());
        ChangeHealth(-damage);
    }

    private void ChangeHealth(int healthChange)
    {
        _health += healthChange;
        if (_health <= 0)
            Die();
        _gotHit = false;

    }

    private void Die()
    {
        StopAllCoroutines();
        GameManager.Instance.GrantXP(5);
        Instantiate(_particleEffect,this.transform.position,Quaternion.identity);
        _gotHit = false;
        Destroy(this.gameObject);
        return;
    }

    private IEnumerator ChangeColorCoroutine()
    {
        _spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(0.09f);

        _spriteRenderer.color = _baseColor;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.TryGetComponent<HealthScript>(out HealthScript healthScript))
        {
            healthScript.ChangeHealth(-1);
        }
    }
}
