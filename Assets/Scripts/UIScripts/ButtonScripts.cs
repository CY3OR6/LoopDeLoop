using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScripts : MonoBehaviour
{
    [SerializeField]
    string goToLevel = "";


    private void Start()
    {
        if (PlayerPrefs.GetString(goToLevel, "Uncleared") != "Cleared")
        {
            GetComponent<Button>().interactable = false;
        }
    }

    public void GoToLevelScene()
    {
        int level = int.Parse(goToLevel);

        SceneManager.LoadScene(level);
    }
}
