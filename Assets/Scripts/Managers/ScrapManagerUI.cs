using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrapManagerUI : MonoBehaviour
{
    [SerializeField] ScrapManager _scrapManager;
    [SerializeField] TextMeshProUGUI _scrapText;



    private void Awake()
    {
        ScrapManagerDataHandler.OnUpdateScrap += OnUpdateScrapUI;
        _scrapText.text = "0";

    }

    private void OnUpdateScrapUI(int updatedValue)
    {
        _scrapText.text = updatedValue.ToString();
        Debug.Log("ssdfghknxsfgmlsfdgmlhsgdfqg");

    }

    private void OnDisable()
    {
        ScrapManagerDataHandler.OnUpdateScrap -= OnUpdateScrapUI;

    }

    private void OnEnable()
    {
        ScrapManagerDataHandler.OnUpdateScrap += OnUpdateScrapUI;
    }
}
