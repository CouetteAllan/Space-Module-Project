using System;
using UnityEngine;

public static class ScrapManagerDataHandler
{
    public static event Action<int> OnPickUpScrap;
    public static event Action<int> OnUpdateScrap;
    public static event Func<int,StatType,bool> OnSellScrap;
    public static event Func<bool> OnAbleToBuyScrap;
    public static event Action<Vector2,int> OnCreateScrap;
    public static event Action<StatType,int> OnSellScrapSuccess;
    public static void PickUpScrap(int number) => OnPickUpScrap?.Invoke(number);
    public static void UpdateScrap(this ScrapManager scrapManager, int updatedNumber) => OnUpdateScrap?.Invoke(updatedNumber);
    public static bool? SellScrap(int number,StatType buffType = StatType.Weight) => OnSellScrap?.Invoke(number,buffType);
    public static void CreateScrap(Vector2 scrapPos,int scrapNumber) => OnCreateScrap?.Invoke(scrapPos,scrapNumber);
    public static bool? AbleToBuyScrap() => OnAbleToBuyScrap?.Invoke();

    public static void SellScrapSuccess(this ScrapManager scrapManager,StatType buffSold, int nbSold) => OnSellScrapSuccess?.Invoke(buffSold,nbSold); 

}
