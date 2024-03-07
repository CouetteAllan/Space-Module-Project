using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoManager : MonoBehaviour
{
    [SerializeField] private Animator _tutoAnim;
    [SerializeField] private GameObject _quickTutoObject;

    private void Awake()
    {
        TutoManagerDataHandler.OnShowTuto += OnShowTuto;

    }

    private void OnEnable()
    {
        TutoManagerDataHandler.OnShowTuto += OnShowTuto;
    }
    public void OnShowTuto(bool showTuto)
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
        _tutoAnim.SetBool("IsInTuto",true);
        yield return new WaitUntil(() => GameManager.Instance.CurrentState == GameState.InGame);
        Destroy(_tutoAnim.gameObject);
        _quickTutoObject.SetActive(true);
        yield return new WaitUntil(() => GameManager.Instance.CurrentLevel >= 2);
        Destroy(_quickTutoObject);
    }

    private void OnDestroy()
    {
        TutoManagerDataHandler.OnShowTuto -= OnShowTuto;
    }
}
