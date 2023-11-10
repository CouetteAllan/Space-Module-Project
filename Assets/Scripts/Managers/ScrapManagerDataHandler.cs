using System;
using UnityEngine;

public static class ScrapManagerDataHandler 
{
    public static event Action<int> OnPickUpScrap;
    public static event Action<int> OnUpdateScrap;
    public static event Action<int> OnSellScrap;
    public static event Action<Transform> OnCreateScrap;
    public static void PickUpScrap(int number) => OnPickUpScrap?.Invoke(number);
    public static void UpdateScrap(this ScrapManager scrapManager, int updatedNumber) => OnUpdateScrap?.Invoke(updatedNumber);
    public static void SellScrap(this ScrapManager scrapManager, int number) => OnSellScrap?.Invoke(number);
    public static void CreateScrap(Transform scrapObject) => OnCreateScrap?.Invoke(scrapObject);

}
