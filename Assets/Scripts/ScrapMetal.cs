using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapMetal : MonoBehaviour
{
    public int _value = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.GatherScrapMetal(_value);
            Destroy(this.gameObject);
        }
    }

    public void SetScrapValue(int value)
    {
        this._value = value;
    }
}
