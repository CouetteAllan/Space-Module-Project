using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpFeedback : MonoBehaviour
{
    [SerializeField] private MMF_Player _levelUpFeedback;

    private void Awake()
    {
        GameManager.OnLevelUp += OnLevelUp;
    }

    private void OnLevelUp(uint level)
    {
        _levelUpFeedback.PlayFeedbacks();
    }

    private void OnDisable()
    {
        GameManager.OnLevelUp -= OnLevelUp;
    }
}
