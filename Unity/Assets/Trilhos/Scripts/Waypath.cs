using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypath : MonoBehaviour
{
    public bool waypathActive = false;
    public bool startingPoint = false;

    public Waypath nextWaypath = null;
    public Waypath previousWaypath = null;

    public Transform[] waypath;

    public void ConnectWaypath(Waypoint collider)
    {
        Waypath colliderWaypath = collider.waypath;
        if (waypathActive && colliderWaypath != previousWaypath) { 
            nextWaypath = colliderWaypath;
            colliderWaypath.previousWaypath = this;
            colliderWaypath.waypathActive = true;
            InvertWaypath(collider);
            Debug.Log($"Connected waypath {this.name} to {colliderWaypath.name}");
        }
    }

    public void DisconnectWaypath(Waypath collider)
    {
        if (nextWaypath != null && nextWaypath == collider)
        {
            nextWaypath.previousWaypath = null;
            nextWaypath = null;
        }
    }

    public void UpdateWaypath(Waypoint collider)
    {
        Waypath connectedPath = collider.waypath;
        if (waypathActive && collider.waypath != previousWaypath && collider.waypath != nextWaypath)
        {
            connectedPath.previousWaypath = this;
            nextWaypath = connectedPath;
            InvertWaypath(collider);
            Debug.Log($"Updated waypath {this.name} to {connectedPath.name}");
        }
    }

    public void InvertWaypath(Waypoint connectedPoint)
    {
        Waypath connectedWaypath = connectedPoint.waypath;
        int collisionPoint = Array.IndexOf(connectedWaypath.waypath, connectedPoint.transform);
        if (collisionPoint == connectedWaypath.waypath.Length - 1)
        {
            Debug.Log($"Inverting waypath {connectedWaypath.name}");
            Array.Reverse(connectedWaypath.waypath);
        }
    }

    private void Update()
    {
        if (previousWaypath == null)
        {
            if (!startingPoint) waypathActive = false;
        }
        else
        {
            if (previousWaypath.waypathActive) waypathActive = true;
            else waypathActive = false;
        }
    }
}

