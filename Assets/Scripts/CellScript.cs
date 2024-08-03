using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CellScript : MonoBehaviour
{
    [SerializeField]
    float rotationAngle = 90f;
    [SerializeField]
    float rotationDuration = 0.1f;

    bool isRotating = false;

    [SerializeField]
    List<Image> lineImages = new List<Image>();

    [SerializeField]
    bool isConnected = false;

    [SerializeField]
    Color connectedColor = Color.green;

    [SerializeField]
    Color unconnectedColor = Color.red;

    [SerializeField]
    List<CellScript> connectedCells = new List<CellScript>();

    [SerializeField]
    List<Vector3> WrongRotations = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        CheckConnectedCell();
    }

    // Update is called once per frame
    void Update()
    {
        if (isConnected)
        {
            foreach (Image i in lineImages)
            {
                i.color = connectedColor;
            }
        }
        else
        {
            foreach (Image i in lineImages)
            {
                i.color = unconnectedColor;
            }
        }


    }

    public void RotateCell()
    {
        if (isRotating) { return; }

        isRotating = true;

        Vector3 targetRotation = transform.rotation.eulerAngles;

        targetRotation += Vector3.forward * rotationAngle;

        transform.DORotate(targetRotation, rotationDuration).onComplete = onRotationComplete;
    }

    void onRotationComplete()
    {
        isRotating = false;
        CheckConnectedCell();

        foreach (CellScript c in connectedCells)
        {
            c.CheckConnectedCell();
        }
    }

    void CheckConnectedCell()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;

        for (int i = 0; i < WrongRotations.Count; i++)
        {
            if (WrongRotations[i] == currentRotation)
            {
                isConnected = false;

                foreach (CellScript c in connectedCells)
                {
                    if (c.isConnected)
                        c.CheckConnectedCell();
                }

                return;
            }
        }

        for (int i = 0; i < connectedCells.Count; i++)
        {
            if (connectedCells[i].isConnected)
            {
                isConnected = true;
                return;
            }
        }
    }
}
