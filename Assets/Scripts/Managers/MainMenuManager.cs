using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel = null;
    public GameObject levelPanel = null;


    public void ContinueLastGame()
    {
        int lastLevelCleared = PlayerPrefs.GetInt("LastLevelCleared", 0);

        Debug.Log(lastLevelCleared);

        lastLevelCleared++;

        if (lastLevelCleared <= 4)
        {
            LoadLevel(lastLevelCleared);
        }
        else
        {
            PlayerPrefs.SetInt("LastLevelCleared", 0);
            LoadLevel(1);
        }
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void OpenLevelScreen()
    {
        mainMenuPanel.SetActive(false);
        levelPanel.SetActive(true);
    }

    public void GoBack()
    {
        mainMenuPanel.SetActive(true);
        levelPanel.SetActive(false);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    [ContextMenu("Clear Prefs For Testing")]
    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
