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
        EnemyManager.OnEndBossCinematic += OnEndBossCinematic;
    }

    private void OnEndBossCinematic()
    {
        _upBorder.DOPlayBackwards();
        _downBorder.DOPlayBackwards();
    }

    private void OnTriggerBossCinematic(System.Action obj)
    {
        PlayBossIntro();
    }

    public void PlayBossIntro()
    {
        _upBorder.DOLocalMoveY(-170.0f, .5f).SetEase(Ease.OutQuad).SetRelative().SetAutoKill(false);
        _downBorder.DOLocalMoveY(170.0f, .5f).SetEase(Ease.OutQuad).SetRelative().SetAutoKill(false);

        _bossIntroFeedback.PlayFeedbacks();
    }

    private void OnDisable()
    {
        EnemyManagerDataHandler.OnTriggerBossCinematic -= OnTriggerBossCinematic;
        EnemyManager.OnEndBossCinematic -= OnEndBossCinematic;


    }
}
