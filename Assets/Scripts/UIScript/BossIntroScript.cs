using DG.Tweening;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIntroScript : MonoBehaviour
{
    [SerializeField] private MMF_Player _bossIntroFeedback;
    [SerializeField] private RectTransform _upBorder, _downBorder;


    private void Awake()
    {
        EnemyManagerDataHandler.OnTriggerBossCinematic += OnTriggerBossCinematic;
    }

    private void OnTriggerBossCinematic(System.Action obj)
    {
        PlayBossIntro();
    }

    public void PlayBossIntro()
    {
        _upBorder.DOLocalMoveY(-170.0f, .5f).SetEase(Ease.OutQuad).SetRelative();
        _downBorder.DOLocalMoveY(170.0f, .5f).SetEase(Ease.OutQuad).SetRelative();

        _bossIntroFeedback.PlayFeedbacks();
    }

    private void OnDisable()
    {
        EnemyManagerDataHandler.OnTriggerBossCinematic -= OnTriggerBossCinematic;

    }
}
