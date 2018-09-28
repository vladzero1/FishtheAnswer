using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void ChangeSceneInto(string SceneName)
    { SceneManager.LoadScene(SceneName); }

    public void StoryModeButton()
    {
        GameManager.Mode = 0;
    }

    public void ArcadeModeButton()
    {
        GameManager.Mode = 1;
    }
    public void Exit()
    {
        Application.Quit();
    }
}
