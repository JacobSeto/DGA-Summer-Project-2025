using UnityEngine;

using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    [SerializeField] GameObject activeScreen;

    /// <summary>
    /// Disables the active screen and sets a new active screen
    /// </summary>
    /// <param name="The GameObject that holds all screen UI"></param>
    public void ChangeActiveScreen(GameObject screen)
    {
        activeScreen.SetActive(false);
        screen.SetActive(true);
        activeScreen = screen;
    }

    public void LoadScene(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
