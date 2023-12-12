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
    }

    public static PreviewLvlUp InstantiateTextObject(Vector2 pos, PreviewLvlUp preview)
    {
        var newPreview = Instantiate(preview.gameObject,pos,Quaternion.identity).GetComponent<PreviewLvlUp>();
        return newPreview;
    }
}
