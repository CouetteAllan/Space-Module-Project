using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpObject : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private Rigidbody2D _rb;
    private BuffDatas _buffDatas;
    private PowerUpManager _powerUpManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<IPickUpObject>(out IPickUpObject player))
        {
           StartCoroutine(PickUpObjectCoroutine(GameManager.Instance.PlayerController.transform, player));
        }
    }

    public void SetUpObject(PowerUpManager manager, BuffDatas datas)
    {
        _powerUpManager = manager;
        _buffDatas = datas;

        Instantiate(_buffDatas.GraphPrefab,this.transform);

        //Set up graph 

    }

    private IEnumerator PickUpObjectCoroutine(Transform playerPos, IPickUpObject player)
    {
        _collider.enabled = false;
        Vector2 startPos = _rb.position;
        float startTime = Time.time;
        float endTime = startTime + 0.25f;

        //repel the scrap
        while (Time.time < endTime)
        {
            if (playerPos == null)
                break;
            _rb.velocity = (startPos - (Vector2)playerPos.position) * 6.0f;
            yield return null;
        }

        endTime = Time.time + 0.4f;
        while (Time.time < endTime)
        {
            if (playerPos == null)
                break;
            _rb.position = Vector2.Lerp(_rb.position, playerPos.position, 5.0f * Time.deltaTime);
            yield return null;
        }

        player.PickUpObject(_buffDatas);
        string fxName = _buffDatas.Stat.Type == StatType.ReloadSpeed ? "attackSpeed" : "damageUp";
        float statValue = _buffDatas.GetStat().Value * 100.0f;
        string fxValue = statValue.ToString("0") + '%';
        FXManager.Instance.PlayEffect(fxName, playerPos.position, Quaternion.identity, playerPos, fxValue);
        _powerUpManager.RemoveObjectFromList(this);
        Destroy(this.gameObject);
        yield break;
    }

}
