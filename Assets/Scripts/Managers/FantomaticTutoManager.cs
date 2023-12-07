using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FantomaticTutoManager : MonoBehaviour
{
    [SerializeField] private Animator _tutoAnim;

    private void Awake()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState)
    {
        if(newState == GameState.BeforeGameStart && !GameManager.Instance.HasShownTutoOnce)
        {
            GameManager.Instance.HasShownTutoOnce = true;
            //Show anim of tuto
            StartCoroutine(TutoCoroutine());
        }
    }

    IEnumerator TutoCoroutine()
    {
        _tutoAnim.SetTrigger("Fantom");
        yield return new WaitUntil(() => GameManager.Instance.CurrentState == GameState.InGame);
        Destroy(_tutoAnim.gameObject);
    }
}
