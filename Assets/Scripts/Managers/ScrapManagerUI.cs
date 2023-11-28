using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrapManagerUI : MonoBehaviour
{
    [SerializeField] ScrapManager _scrapManager;
    [SerializeField] TextMeshProUGUI _scrapText;

    private Coroutine _animCoroutine = null;
    private float _baseFontSize;

    private void Awake()
    {
        ScrapManagerDataHandler.OnUpdateScrap += OnUpdateScrapUI;
        _scrapText.text = "0";
        _baseFontSize = _scrapText.fontSize;
        ScrapManagerDataHandler.OnUpdateScrap += OnUpdateScrapUI;

    }

    private void OnUpdateScrapUI(int updatedValue)
    {
        _scrapText.text = updatedValue.ToString();
        if (_animCoroutine != null)
        {
            StopCoroutine(_animCoroutine);
        }
        _animCoroutine = StartCoroutine(ScrapTextCoroutine());
    }

    private void OnDisable()
    {
        ScrapManagerDataHandler.OnUpdateScrap -= OnUpdateScrapUI;
    }


    private IEnumerator ScrapTextCoroutine()
    {
        _scrapText.fontSize = _baseFontSize;
        _scrapText.transform.parent.gameObject.SetActive(true);
        float timer = 0.95f;
        float startTime = Time.time;
        float dir = 1.0f;
        while (Time.time < startTime + timer)
        {
            if (Time.time > (startTime + (timer/2))  && dir > 0)
                dir = -1.0f;

            _scrapText.fontSize += (dir * 60.0f) * Time.deltaTime;
            yield return null;
        }
        _scrapText.fontSize = _baseFontSize;

        yield return new WaitForSecondsRealtime(1.0f);
        _scrapText.transform.parent.gameObject.SetActive(false);

    }
}
