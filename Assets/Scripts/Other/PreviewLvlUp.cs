using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PreviewLvlUp : MonoBehaviour
{
    [SerializeField] private TextMeshPro _textMeshPro;

    public void SetText(string text)
    {
        _textMeshPro.text = text;
        StartCoroutine(FloatCoroutine());
    }

    public static PreviewLvlUp InstantiateTextObject(Vector2 pos, PreviewLvlUp preview)
    {
        var newPreview = Instantiate(preview.gameObject,pos,Quaternion.identity).GetComponent<PreviewLvlUp>();
        return newPreview;
    }

    IEnumerator FloatCoroutine()
    {
        Vector2 offsetPos = (Vector2)this.transform.position + Vector2.up;
        Vector2 startPos = (Vector2)this.transform.position;
        float startTime = Time.time;
        float timer = Time.time;
        float offsetTime = 2.0f;
        while (true)
        {
            bool inverse = timer > startTime + offsetTime/2;
            if(!inverse)
            {
                this.transform.position = Vector2.Lerp(this.transform.position, offsetPos, 1.2f * 0.07f);
            }
            else
            {
                this.transform.position = Vector2.Lerp(this.transform.position, startPos, 1.2f * 0.07f);
                if(timer > startTime + offsetTime)
                {
                    startTime = startTime + offsetTime;
                    timer = startTime;
                }
            }
            timer += 0.07f;
            yield return new WaitForSecondsRealtime(0.07f);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
