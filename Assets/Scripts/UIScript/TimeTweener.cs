using DG.Tweening;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeTweener : MonoBehaviour
{
    public static event Action OnChronoIntroDone;

    [SerializeField] private Transform _textTransform;
    [SerializeField] private Transform _panelTransform;
    [SerializeField] private CanvasGroup _textCanvaGroup;
    [SerializeField] private TextMeshProUGUI _chronoText;
    [SerializeField] private MMF_Player _feedbackChrono;

    private Transform _startTransform;

    private void Start()
    {
        _startTransform = _textTransform.transform;
    }
    public void BounceTime()
    {
        _textTransform.DOScale(1.2f, .5f).SetRelative().SetEase(Ease.InOutSine).SetLoops(6,LoopType.Yoyo);
        _textTransform.DOMoveY(60.0f,.5f).SetRelative().SetEase(Ease.Linear).SetLoops(6, LoopType.Yoyo).OnComplete(() => _textTransform = _startTransform);
        _chronoText.DOColor(Color.red,1f).SetLoops(2,LoopType.Yoyo).OnComplete(() => _chronoText.color = Color.white);
    }

    public void IntroCinematic()
    {
        var sequence = DOTween.Sequence().SetDelay(1.0f)
            .Append(_textCanvaGroup.DOFade(1.0f, .8f))
            .Join(_textTransform.DOMoveY(-80.0f, .6f).SetRelative())
            .Join(_textTransform.DOBlendableScaleBy(Vector2.one * 1.1f, 1.0f))
            .AppendInterval(.6f)
            .Append(_textTransform.DOMoveY(_panelTransform.position.y, 1.0f).SetEase(Ease.OutCubic))
            .Join(_textTransform.DOScale(1.0f, .7f))
            .OnComplete(() => OnChronoIntroDone?.Invoke());
        
    }

    public void AlertTime()
    {
        _feedbackChrono.PlayFeedbacks();
    }
}
