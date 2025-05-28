using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSelectButton : MonoBehaviour
{
    public void EasyGame()
    {
        SceneManager.LoadScene("_Scene_0");
    }
    public void NormalGame()
    {
        SceneManager.LoadScene("_Scene_0 1");
    }
    public void HardGame()
    {
        SceneManager.LoadScene("_Scene_0 2");
    }
}