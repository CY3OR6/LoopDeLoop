using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public List<CellScript> cellsInThisLevel = new List<CellScript>();

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

        Debug.Log("Level Cleared");
    }

}
