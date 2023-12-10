using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Tools;

public class ScrapManager : MonoBehaviour
{
    [SerializeField] private Transform _scrapTransform;
    [SerializeField] private ScrapManagerUI _scrapManagerUI;

    [SerializeField] private int _numberOfScrap = 0;
    public int NumberOfScrap {  get { return _numberOfScrap; } }

    private void Awake()
    {
        //Listen to event whenever an enemy dies so we can have a chance to spawn scrap
        EnemyScript.OnDeath += OnEnemyDeath;
        ScrapManagerDataHandler.OnPickUpScrap += OnPickUpScrap;
        ScrapManagerDataHandler.OnSellScrap += SellScrapMetal;
        ScrapManagerDataHandler.OnAbleToBuyScrap += AbleToBuyScrap;
        this.UpdateScrap(_numberOfScrap);
    }

    private bool AbleToBuyScrap()
    {
        return _numberOfScrap >= 5;
    }

    private void OnPickUpScrap(int value)
    {
        _numberOfScrap += value;
        this.UpdateScrap(_numberOfScrap);

    }

    private void OnEnemyDeath(object sender, EnemyScript.EnemyStatsOnDeath enemyStats)
    {
        if (Utils.RollChance(chance: .4f)) //40% chances to drop scrap metal
        {
            var newScrap = SpawnScrapMetal(enemyStats.finalPos);
            newScrap.SetScrapValue(enemyStats.scrapGranted);
        }
    }

        

    public bool SellScrapMetal(int scrapSold)
    {
        if(scrapSold > _numberOfScrap)
        {
            UtilsClass.CreateWorldTextPopup("Don't have enough Scrap", UtilsClass.GetMouseWorldPosition());
            return false;
        }

        _numberOfScrap -= scrapSold;
        this.UpdateScrap(_numberOfScrap);
        return true;
    }


    private ScrapMetal SpawnScrapMetal(Vector2 pos) => Instantiate(_scrapTransform, pos, Quaternion.identity).GetComponent<ScrapMetal>();

    private void OnDisable()
    {
        EnemyScript.OnDeath -= OnEnemyDeath;
        ScrapManagerDataHandler.OnPickUpScrap -= OnPickUpScrap;
    }
}
