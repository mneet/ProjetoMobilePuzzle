using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDrag : MonoBehaviour
{
    private Vector3 offset;

    private void OnMouseDown()
    {
        offset = transform.position = GridMap.Instance.GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        GridMap.Instance.tileDragged = gameObject;
        Vector3 pos = GridMap.Instance.GetMouseWorldPosition();
        pos.y = 0;
        transform.position = pos;
    }

    private void OnMouseUpAsButton()
    {
        transform.position = GridMap.Instance.SnapCoordinateToGrid(transform.position);
    }
}
