using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    public bool trailActive = false;

    public GameObject[] trailMarkers;
    public Transform[] trailPath;

    private void BuildTrailPath()
    {
        trailPath = new Transform[trailMarkers.Length];
        for (int i = 0; i < trailMarkers.Length; i++)
        {
            trailPath[i] = trailMarkers[i].transform;
        }
    }

    private void Start()
    {
        BuildTrailPath();
    }
}

