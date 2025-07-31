using UnityEngine;

using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    [SerializeField] GameObject activeScreen;
    public static MenuNavigation Instance;

    private void Awake()
    {
        Instance = this;
    }

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
    /// <summary>
    /// Loads a Unity Scene with the given scene name
    /// </summary>
    /// <param name="scenelName"></param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
