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
    [SerializeField] private int _maxScrapInWorld = 200;
    [SerializeField] private Transform _scrapParent;
    
    public int NumberOfScrap {  get { return _numberOfScrap; } }

    private Dictionary<StatType, int> _numberOfModulePurchased = new Dictionary<StatType, int>();
    private Queue<ScrapMetal> _scrapMetals = new Queue<ScrapMetal>();
    private void Awake()
    {
        EnemyScript.OnDeath += OnEnemyDeath;
        ScrapManagerDataHandler.OnCreateScrap += OnCreateScrap;
        ScrapManagerDataHandler.OnPickUpScrap += OnPickUpScrap;
        ScrapManagerDataHandler.OnSellScrap += SellScrapMetal;
        ScrapManagerDataHandler.OnAbleToBuyScrap += AbleToBuyScrap;
        ScrapManagerDataHandler.OnCheckScrap += OnCheckScrap;

        GameManager.OnGameStateChanged += OnGameStateChanged;
        this.UpdateScrap(_numberOfScrap);
    }

    private void OnCheckScrap()
    {
        ScrapManagerDataHandler.EnoughScrap(AbleToBuyScrap());
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.StartGame)
            this.UpdateScrap(_numberOfScrap);
    }

    private void OnCreateScrap(Vector2 pos,int scrapGranted)
    {
        for (int i = 0; i < scrapGranted; i++)
        {
            if(_scrapMetals.Count > _maxScrapInWorld)
            {
                var lastScrap = _scrapMetals.Dequeue();
                Destroy(lastScrap);
            }
            var randomDistance = Random.Range(0.6f, 2.0f);
            var newScrap = SpawnScrapMetal(pos + (Vector2)UtilsClass.GetRandomDir() * randomDistance);
            newScrap.SetScrapMetal(1,this); //to change
            _scrapMetals.Enqueue(newScrap);
        }
    }

    private void Start()
    {
        this.UpdateScrap(_numberOfScrap);
    }

    private bool AbleToBuyScrap()
    {
        this.UpdateScrap(_numberOfScrap);
        return _numberOfScrap >= 15;
    }

    private void OnPickUpScrap(int value)
    {
        _numberOfScrap += value;
        ScrapManagerDataHandler.EnoughScrap(AbleToBuyScrap());
    }

    private void OnEnemyDeath(object sender, EnemyScript.EnemyStatsOnDeath enemyStats)
    {
        if (Utils.RollChance(chance: .4f) || enemyStats.tier == 3) //40% chances to drop scrap metal
        {
            OnCreateScrap(enemyStats.finalPos, enemyStats.scrapGranted);
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
    private ScrapMetal SpawnScrapMetal(Vector2 pos) => Instantiate(_scrapTransform, pos, Quaternion.identity,_scrapParent).GetComponent<ScrapMetal>();
    public void RemoveScrapFromQueue(ScrapMetal scrap)
    {
        _scrapMetals.Enqueue(scrap);
    }

    private void OnDestroy()
    {
        EnemyScript.OnDeath -= OnEnemyDeath;
        ScrapManagerDataHandler.OnPickUpScrap -= OnPickUpScrap;

        ScrapManagerDataHandler.OnCreateScrap -= OnCreateScrap;
        ScrapManagerDataHandler.OnSellScrap -= SellScrapMetal;
        ScrapManagerDataHandler.OnAbleToBuyScrap -= AbleToBuyScrap;
        ScrapManagerDataHandler.OnCheckScrap -= OnCheckScrap;


        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }
}
