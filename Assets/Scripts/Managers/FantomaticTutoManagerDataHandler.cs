using System;
using UnityEngine;

public static class FantomaticTutoManagerDataHandler
{
    public static event Action<bool> OnShowTuto;
    public static void ShowTuto(bool showTuto) => OnShowTuto?.Invoke(showTuto);
}
