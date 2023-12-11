using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public void ChangeScene(int scene)
    {
        if (scene == 1)
            GameManager.Instance.ChangeGameState(GameState.BeforeGameStart);
        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
