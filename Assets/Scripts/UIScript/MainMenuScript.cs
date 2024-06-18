using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject _titleScreen;
    [SerializeField] private GameObject _credits;
    [SerializeField] private InputSystemUIInputModule _inputSystem;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private GameObject _playButton;
    [SerializeField] private GameObject _creditsBackButton;
    [SerializeField] private Button _cheatButton;

    private void Start()
    {
        Credits(false);
        _inputSystem.cancel.action.performed += Action_performed;
        _cheatButton.onClick.AddListener(() => ChangeSceneWithTimerLow());
    }

    private void Action_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (_credits.activeSelf)
        {
            Credits(false);
        }
    }

    public void ChangeScene(int scene)
    {
        if (scene == 1)
            GameManager.Instance.ChangeGameState(GameState.BeforeGameStart);
        SceneManager.LoadScene(scene);
    }

    public void ChangeSceneWithTimerLow()
    {
        GameManager.Instance.SetTimerLow();
        ChangeScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Credits(bool showCredits)
    {
        _titleScreen.SetActive(!showCredits);
        _credits.SetActive(showCredits);
        GameObject selectedButton = showCredits ? _creditsBackButton : _playButton;
        _eventSystem.SetSelectedGameObject(selectedButton);
    }

    public void OnDisable()
    {
        _inputSystem.cancel.action.performed -= Action_performed;
    }
}
