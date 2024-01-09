using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapMetal : MonoBehaviour
{
    [SerializeField] private CircleCollider2D _collider;
    private int _value = 1;
    private Rigidbody2D _rigidbody2D;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IGatherScrap player))
        {
            StartCoroutine(ScrapCoroutine(collision.gameObject.transform,player));
        }
    }

    private IEnumerator ScrapCoroutine(Transform playerPos, IGatherScrap player)
    {
        _collider.enabled = false;
        Vector2 startPos = _rigidbody2D.position;
        float startTime = Time.time;
        float endTime = startTime + 0.25f;

        //repel the scrap
        while(Time.time < endTime)
        {
            if (playerPos == null)
                break;
            _rigidbody2D.velocity = (startPos - (Vector2)playerPos.position) * 6.0f;
            yield return null;
        }

        endTime = Time.time + 0.4f;
        while (Time.time < endTime)
        {
            if (playerPos == null)
                break;
            _rigidbody2D.position = Vector2.Lerp(_rigidbody2D.position, playerPos.position,5.0f * Time.deltaTime);
            yield return null;
        }

        player.GatherScrapMetal(_value);
        Destroy(this.gameObject);
        yield break;
    }
    

    public void SetScrapValue(int value)
    {
        this._value = value;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
}
