using UnityEngine;
using UnityEngine.SceneManagement;
public class StartButton : MonoBehaviour
{
    public void StartGame() {
        SceneManager.LoadScene("Start_Scene_0 1");
    }

    public void GoHome()
    {
        SceneManager.LoadScene("Start_Scene_0");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Start_Scene_0 1");
    }
}
