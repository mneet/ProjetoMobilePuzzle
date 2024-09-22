using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool Placed { get; private set; }
    public bool moveable = false;
    public bool rotatable = true;

    private void Start()
    {
        if (moveable)
        {
            Vector3 position = GridMap.Instance.SnapCoordinateToGrid(Vector3.zero);
            gameObject.AddComponent<TileDrag>();
        }
        if (rotatable) RandomizeRotation();
    }

    private void OnMouseDown()
    {
        if (rotatable && (GameManager.Instance != null && !GameManager.Instance.pause))
        {
            transform.Rotate(0, 60, 0);
        }
    }

    private void RandomizeRotation()
    {
        Waypath waypath = GetComponent<Waypath>();
        if (waypath != null && waypath.pathType == Waypath.PathType.START_LINE)
        {
            transform.Rotate(0, Random.Range(0, 6) * 60, 0); ;
        }      
    }
}

