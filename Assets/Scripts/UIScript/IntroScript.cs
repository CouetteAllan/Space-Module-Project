using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScript : MonoBehaviour
{
    public static event Action OnBeginningIntroFinished;

    [SerializeField] private RectTransform _upBorder, _downBorder;
    [SerializeField] private CanvasGroup _textCanvaGroup;
    [SerializeField] private CanvasGroup _wholeCanvaGroup;
    [SerializeField] private Transform _textTransform;
    [SerializeField] private RectTransform _textBackgroundRectTransform;
    [SerializeField] private GameObject _mainHUD;
    [SerializeField] private TimeTweener _timeTweener;

    private Sequence _sequence = null;

    private Vector2 _startSizeDelta;
    private void Awake()
    {
        _startSizeDelta = _textBackgroundRectTransform.sizeDelta;
        _textBackgroundRectTransform.sizeDelta = new Vector2(-1920, _startSizeDelta.y);
        GameManager.OnGameStateChanged += OnGameStateChanged;
        TimeTweener.OnChronoIntroDone += OnChronoIntroDone;
    }

    private void OnChronoIntroDone()
    {
        RewindIntro();
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
        TimeTweener.OnChronoIntroDone -= OnChronoIntroDone;

    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.StartGame)
        {
            if (!GameManager.Instance.HasShownTutoOnce)
                PlayIntro();
            else
                _timeTweener.IntroCinematic();

        }
    }

    private void PlayIntro()
    {
        _mainHUD.SetActive(false);
        //display borders and the warning text then rewind it
       _sequence = DOTween.Sequence().SetAutoKill(false).SetDelay(0.5f)
            .Append(_upBorder.DOLocalMoveY(-170.0f, .5f).SetEase(Ease.OutQuad).SetRelative())
            .Join(_downBorder.DOLocalMoveY(170.0f, .5f).SetEase(Ease.OutQuad).SetRelative())
            .Join(_textBackgroundRectTransform.DOSizeDelta(_startSizeDelta, 1.0f).SetEase(Ease.InOutSine))
            .Append(_textBackgroundRectTransform.DOSizeDelta(new Vector2(_startSizeDelta.x, _startSizeDelta.y * .7f), .5f).SetEase(Ease.OutQuint))
            .Join(_textCanvaGroup.DOFade(1.0f, .2f))
            .Join(_textTransform.DOPunchScale(Vector3.one * 1.7f, .5f, vibrato: 1, elasticity: 0))
            .AppendCallback(() => { 
                if(!_sequence.IsBackwards())
                    _timeTweener.IntroCinematic(); 
            })
            .Append(_textCanvaGroup.DOFade(.5f, .3f).SetLoops(8, LoopType.Yoyo));


    }

    private void RewindIntro()
    {
        if (_sequence == null)
            return;
        _sequence.SetAutoKill(true);
        _sequence.PlayBackwards();
        _sequence.OnRewind(() =>
        {
            _mainHUD.SetActive(true);
            OnBeginningIntroFinished?.Invoke();
            TutoManagerDataHandler.ShowTuto(true);
        });
    }
}
