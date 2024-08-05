using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour
{
    [SerializeField]
    float rotationAngle = 90f;
    [SerializeField]
    float rotationDuration = 0.1f;

    bool isRotating = false;

    [SerializeField]
    bool isSource = false;

    [SerializeField]
    List<SpriteRenderer> linesRenderer = new List<SpriteRenderer>();

    public bool isConnected = false;

    [SerializeField]
    bool canRotate = true;

    [SerializeField]
    Color connectedColor = Color.green;

    [SerializeField]
    Color unconnectedColor = Color.red;

    AudioSource audioSource = null;

    [SerializeField]
    AudioClip rotationAudioClip = null;

    [SerializeField]
    Vector4 directionsToCheck = Vector4.zero;

    [SerializeField]
    List<CellScript> connectedCells = new List<CellScript>();

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = rotationAudioClip;
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        CheckAreasAround();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSource)
        {
            isConnected = true;
        }

        if (isConnected)
        {
            foreach (SpriteRenderer item in linesRenderer)
            {
                item.color = connectedColor;
            }
        }
        else
        {
            foreach (SpriteRenderer item in linesRenderer)
            {
                item.color = unconnectedColor;
            }
        }
    }

    private void OnMouseDown()
    {
        RotateCell();
    }

    public void RotateCell()
    {
        if (isRotating || !canRotate) { return; }

        isRotating = true;

        audioSource.pitch = Random.Range(0.5f, 1f);
        audioSource.Play();

        Vector3 targetRotation = transform.rotation.eulerAngles;

        targetRotation += Vector3.forward * rotationAngle;

        transform.DORotate(targetRotation, rotationDuration).onComplete = onRotationComplete;
    }

    void onRotationComplete()
    {
        isRotating = false;
        try
        {
            CheckAreasAround();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            throw;
        }
        LevelManager.instance.CheckIfHasClearedLevel();
    }

    [SerializeField]
    float hitRadius = 0.5f;

    [SerializeField]
    float hitDistance = 0.5f;


    [SerializeField]
    LayerMask lineLayer;

    void UnconnectTheWholeLine()
    {
        foreach (CellScript cell in connectedCells)
        {
            if (cell.sourceCell == this && cell.isConnected)
            {
                cell.isConnected = false;
                cell.UnconnectTheWholeLine();
            }
        }
    }

    void CheckAreasAround()
    {

        UnconnectTheWholeLine();

        connectedCells.Clear();

        List<Collider2D> hit = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.useLayerMask = true;
        filter.layerMask = lineLayer;
        if (0 < directionsToCheck.x)
        {
            Physics2D.OverlapCircle(transform.position + transform.right * hitDistance, hitRadius, filter, hit);

            foreach (Collider2D item in hit)
            {
                if (item.transform.parent.GetComponent<CellScript>() != this)
                {
                    connectedCells.Add(item.transform.parent.GetComponent<CellScript>());
                }
            }
        }
        if (0 < directionsToCheck.y)
        {
            Physics2D.OverlapCircle(transform.position - transform.right * hitDistance, hitRadius, filter, hit);

            foreach (Collider2D item in hit)
            {
                if (item.transform.parent.GetComponent<CellScript>() != this)
                {
                    connectedCells.Add(item.transform.parent.GetComponent<CellScript>());
                }
            }
        }
        if (0 < directionsToCheck.z)
        {
            Physics2D.OverlapCircle(transform.position + transform.up * hitDistance, hitRadius, filter, hit);

            foreach (Collider2D item in hit)
            {
                if (item.transform.parent.GetComponent<CellScript>() != this)
                {
                    connectedCells.Add(item.transform.parent.GetComponent<CellScript>());
                }
            }
        }
        if (0 < directionsToCheck.w)
        {
            Physics2D.OverlapCircle(transform.position - transform.up * hitDistance, hitRadius, filter, hit);

            foreach (Collider2D item in hit)
            {
                if (item.transform.parent.GetComponent<CellScript>() != this)
                {
                    connectedCells.Add(item.transform.parent.GetComponent<CellScript>());
                }
            }
        }

        for (int i = 0; i < connectedCells.Count; i++)
        {
            if (connectedCells[i].isConnected)
            {
                OnFoundConnection(connectedCells[i]);
                return;
            }
        }

        isConnected = false;
        sourceCell = null;
        foreach (CellScript cell in connectedCells)
        {
            if (cell.isConnected)
                cell.CheckAreasAround();
        }
    }

    [SerializeField]
    CellScript sourceCell = null;

    [SerializeField]
    ParticleSystem connectionPS = null;

    void OnFoundConnection(CellScript _sourceCell)
    {
        isConnected = true;

        sourceCell = _sourceCell;

        foreach (CellScript cell in connectedCells)
        {
            if (cell != sourceCell || !cell.isConnected)
                cell.CheckAreasAround();
        }

        if (connectionPS != null)
            connectionPS.Play();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        if (directionsToCheck.x > 0)
            Gizmos.DrawWireSphere(transform.position + (transform.right * hitDistance), hitRadius);
        if (directionsToCheck.y > 0)
            Gizmos.DrawWireSphere(transform.position - (transform.right * hitDistance), hitRadius);
        if (directionsToCheck.z > 0)
            Gizmos.DrawWireSphere(transform.position + (transform.up * hitDistance), hitRadius);
        if (directionsToCheck.w > 0)
            Gizmos.DrawWireSphere(transform.position - (transform.up * hitDistance), hitRadius);
    }
}
