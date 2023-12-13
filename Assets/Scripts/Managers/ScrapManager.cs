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

    private Dictionary<StatType, int> _numberOfModulePurchased = new Dictionary<StatType, int>();

    private void Awake()
    {
        //Listen to event whenever an enemy dies so we can have a chance to spawn scrap
        EnemyScript.OnDeath += OnEnemyDeath;
        ScrapManagerDataHandler.OnPickUpScrap += OnPickUpScrap;
        ScrapManagerDataHandler.OnSellScrap += SellScrapMetal;
        ScrapManagerDataHandler.OnAbleToBuyScrap += AbleToBuyScrap;
        this.UpdateScrap(_numberOfScrap);
    }

    private void Start()
    {
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
        if (Utils.RollChance(chance: .4f) || enemyStats.tier == 3) //40% chances to drop scrap metal
        {
            for (int i = 0; i < enemyStats.scrapGranted; i++)
            {
                var newScrap = SpawnScrapMetal(enemyStats.finalPos + (Vector2)UtilsClass.GetRandomDir() * 0.6f);
                newScrap.SetScrapValue(1); //to change
            }
        }
    }

        

    public bool SellScrapMetal(int scrapSold, StatType buffTypeSold = StatType.Weight)
    {
        if(scrapSold > _numberOfScrap)
        {
            UtilsClass.CreateWorldTextPopup("Don't have enough Scrap", UtilsClass.GetMouseWorldPosition());
            return false;
        }

        _numberOfScrap -= scrapSold;
        this.UpdateScrap(_numberOfScrap);
        if(buffTypeSold != StatType.Weight)
        {
            if (_numberOfModulePurchased.ContainsKey(buffTypeSold))
            {
                _numberOfModulePurchased[buffTypeSold]++;
            }
            else
            {
                _numberOfModulePurchased.Add(buffTypeSold, 1);
            }
            this.SellScrapSuccess(buffTypeSold, GetNumberOfBuffPurchased(buffTypeSold));
        }
        
        return true;
    }

    public int GetNumberOfBuffPurchased(StatType buffTypeSold)
    {
        if (_numberOfModulePurchased.TryGetValue(buffTypeSold, out int nb))
        {
            return nb;
        }
        else
            return 0;
    }
    private ScrapMetal SpawnScrapMetal(Vector2 pos) => Instantiate(_scrapTransform, pos, Quaternion.identity).GetComponent<ScrapMetal>();

    private void OnDisable()
    {
        EnemyScript.OnDeath -= OnEnemyDeath;
        ScrapManagerDataHandler.OnPickUpScrap -= OnPickUpScrap;
    }
}
