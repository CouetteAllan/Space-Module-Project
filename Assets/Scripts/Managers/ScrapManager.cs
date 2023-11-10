using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class ScrapManager : MonoBehaviour
{
    [SerializeField] private Transform _scrapTransform;
    [SerializeField] private ScrapManagerUI _scrapManagerUI;

    private int _numberOfScrap = 0;
    public int NumberOfScrap {  get { return _numberOfScrap; } }

    private void Awake()
    {
        //Listen to event whenever an enemy dies so we can have a chance to spawn scrap
        EnemyScript.OnDeath += OnEnemyDeath;
        ScrapManagerDataHandler.OnPickUpScrap += OnPickUpScrap;
    }

    private void OnPickUpScrap(int value)
    {
        _numberOfScrap += value;
        this.UpdateScrap(_numberOfScrap);
    }

    private void OnEnemyDeath(EnemyScript.EnemyStat enemyStat)
    {
        if(RollChance(chance: .5f)) //50% chances to drop scrap metal
            SpawnScrapMetal(enemyStat.finalPos);
    }

    private ScrapMetal SpawnScrapMetal(Vector2 pos)
    {
        ScrapMetal newScrapMetal = Instantiate(_scrapTransform,pos,Quaternion.identity).GetComponent<ScrapMetal>();

        return newScrapMetal;
    }

    public void SellScrapMetal(int scrapSold)
    {
        if(scrapSold > _numberOfScrap)
        {
            UtilsClass.CreateWorldTextPopup("Don't have enough Scrap", UtilsClass.GetMouseWorldPosition());
            return;
        }
        _numberOfScrap -= scrapSold;
        this.SellScrap(scrapSold);
    }

    private bool RollChance(float chance)
    {
        float randomNumber = Random.value;
        return randomNumber < chance;
    }

    private void OnDisable()
    {
        EnemyScript.OnDeath -= OnEnemyDeath;
        ScrapManagerDataHandler.OnPickUpScrap -= OnPickUpScrap;
    }
}
