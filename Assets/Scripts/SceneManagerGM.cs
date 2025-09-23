using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerGM : MonoBehaviour
{

    private int _currentSceneIndex;
    private const int MAIN_MENU_INDEX = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(_currentSceneIndex+1);
    }

    public void LoadMainMenu()
    {
        if(Time.timeScale <= 0f)
        {
            Time.timeScale = 1f;
        }
        SceneManager.LoadScene(MAIN_MENU_INDEX);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
