using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    [SerializeField]
    private MainMenu mainMenu;
    [SerializeField]
    private LevelMenu levelMenu;
    [SerializeField]
    private Menu currentMenu;

	void Awake ()
    {
        if (!levelMenu) levelMenu = GetComponentInChildren<LevelMenu>();
        if (!mainMenu) mainMenu = GetComponentInChildren<MainMenu>();
        currentMenu = mainMenu;
        levelMenu.gameObject.SetActive(false);
	}
	
	public void SwitchToMainMenu()
    {
        ChangeCurrentMenu(mainMenu);
    }

    public void SwitchToLevelMenu()
    {
        ChangeCurrentMenu(levelMenu);
    }

    public void QuitGame()
    {
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }

    // levels managed in LevelMenu maybe or a LevelManager
    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    private void ChangeCurrentMenu(Menu menu)
    {
        if (currentMenu) currentMenu.gameObject.SetActive(false);
        currentMenu = menu;
        menu.gameObject.SetActive(true);
    }
}
