using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour, IObstacle
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Color _lowHealthColor;
    [SerializeField] private bool _isIndestructible = false;
    [SerializeField] private Animator _animator;
    [SerializeField] private Sprite[] _sprites;
    private ObstaclesManager _manager;
    private int _health;
    private int _maxHealth;
    private int _baseHealth = 30;
    private Color _startColor;
    
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
        _animator.SetTrigger("Damage");
    }


    private void ChangeHealth(int damage)
    {
        _health = Mathf.Clamp(_health + damage, 0, _maxHealth);
        _renderer.color = Color.Lerp(_lowHealthColor,_startColor, (float)_health/(float)_maxHealth);
        ChangeSprite(_health);
        if (_health <= 0)
            DestroyObstacle();
    }

    private void ChangeSprite(int health)
    {
        if(health < _maxHealth / 4)
        {
            _renderer.sprite = _sprites[2];
            return;
        }
        else if (health < (_maxHealth/4) * 2)
        {
            _renderer.sprite = _sprites[1];
            return;
        }
        else
        {
            _renderer.sprite = _sprites[0];
            return;
        }
    }

    public void SetUpObstacle(ObstaclesManager manager,int health)
    {
        _manager = manager;
        _health = health + _baseHealth;
        _maxHealth = _health;
        StartCoroutine(RotationCoroutine());
        _startColor = _renderer.color;
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
            this.transform.Rotate(0, 0, 30.0f * Time.deltaTime);
            yield return null;
        }
    }
}
