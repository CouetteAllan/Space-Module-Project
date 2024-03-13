using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpObject : MonoBehaviour
{
    private BuffDatas _buffDatas;
    private PowerUpManager _powerUpManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<IPickUpObject>(out IPickUpObject player))
        {
            player.PickUpObject(_buffDatas);
            _powerUpManager.RemoveObjectFromList(this);
            Destroy(this.gameObject);
        }
    }

    public void SetUpObject(PowerUpManager manager, BuffDatas datas)
    {
        _powerUpManager = manager;
        _buffDatas = datas;

        Instantiate(_buffDatas.GraphPrefab,this.transform);

        //Set up graph 

    }

}
