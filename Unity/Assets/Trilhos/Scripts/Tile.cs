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
    }

    private void OnMouseDown()
    {
        if (rotatable)
        {
            transform.Rotate(0, 60, 0);
        }
    }
}
