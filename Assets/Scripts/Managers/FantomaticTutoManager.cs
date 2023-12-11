using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FantomaticTutoManager : MonoBehaviour
{
    [SerializeField] private Animator _tutoAnim;

    private void Awake()
    {
        FantomaticTutoManagerDataHandler.OnShowTuto += OnShowTuto;

    }

    private void OnShowTuto(bool showTuto)
    {
        if (!GameManager.Instance.HasShownTutoOnce)
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

    private void OnDisable()
    {
        FantomaticTutoManagerDataHandler.OnShowTuto -= OnShowTuto;

    }
}
