using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public List<CellScript> cellsInThisLevel = new List<CellScript>();

    [SerializeField]
    string currentLevel = "";

    [SerializeField]
    GameObject wonPanel = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
            instance = this;
        }

        currentLevel = SceneManager.GetActiveScene().buildIndex.ToString();
        wonPanel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        cellsInThisLevel = FindObjectsOfType<CellScript>().ToList<CellScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckIfHasClearedLevel()
    {
        for (int i = 0; i < cellsInThisLevel.Count; i++)
        {
            if (!cellsInThisLevel[i].isConnected)
            {
                return;
            }
        }

        PlayerPrefs.SetString(currentLevel, "Cleared");

        if (PlayerPrefs.GetInt("LastLevelCleared", 0) < SceneManager.GetActiveScene().buildIndex)
        {
            PlayerPrefs.SetInt("LastLevelCleared", SceneManager.GetActiveScene().buildIndex);
        }

        wonPanel.SetActive(true);
    }

    public void GoToNextLevel()
    {
        int sc = SceneManager.GetActiveScene().buildIndex;

        sc++;

        SceneManager.LoadScene(sc);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
