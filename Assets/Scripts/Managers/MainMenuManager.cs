using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ContinueLastGame()
    {
        int lastLevelCleared = PlayerPrefs.GetInt("LastLevelCleared", 0);

        Debug.Log(lastLevelCleared);

        switch (lastLevelCleared)
        {
            case 0:
                LoadLevel(1);
                break;
            case 1:
                LoadLevel(2);
                break;

        }
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }
}
