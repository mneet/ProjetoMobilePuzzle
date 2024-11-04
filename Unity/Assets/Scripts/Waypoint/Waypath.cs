using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypath : MonoBehaviour
{
    public enum PathType
    {
        START_LINE,
        PATH,
        GOAL_LINE
    }
    
    // Waypath state
    public PathType pathType = PathType.PATH;
    public bool waypathActive = false;

    // Waypaths connections
    public Waypath nextWaypath = null;
    public Waypath previousWaypath = null;

    // Waypoints array
    public Transform[] waypath;

    // Debug flags
    [Header("Debug Flags")]
    [SerializeField] private bool verboseConnections = false;

    #region Path Connection Methods

    // Connect next waypath to this when collider enters
    public void ConnectWaypath(Waypoint collider)
    {
        Waypath colliderWaypath = collider.waypath;
        if (waypathActive && colliderWaypath != previousWaypath) { 
            nextWaypath = colliderWaypath;
            colliderWaypath.previousWaypath = this;
            colliderWaypath.waypathActive = true;
                   
            if (verboseConnections) Debug.Log($"Connected waypath {this.name} to {colliderWaypath.name}");
            InvertWaypath(collider);
        }
    }

    // Disconnect next waypath when collider exit
    public void DisconnectWaypath(Waypath collider)
    {
        if (nextWaypath != null && nextWaypath == collider)
        {
            nextWaypath.previousWaypath = null;
            nextWaypath = null;
        }
    }

    // Update waypath when collider is already colliding
    public void UpdateWaypath(Waypoint collider)
    {
        Waypath connectedPath = collider.waypath;
        if (waypathActive && collider.waypath != previousWaypath && collider.waypath != nextWaypath)
        {
            connectedPath.previousWaypath = this;
            nextWaypath = connectedPath;
            
            if (verboseConnections) Debug.Log($"Updated waypath {this.name} to {connectedPath.name}");
            InvertWaypath(collider);
        }
    }

    // Invert next waypath points if connected point is the last point
    public void InvertWaypath(Waypoint connectedPoint)
    {
        Waypath connectedWaypath = connectedPoint.waypath;
        int collisionPoint = Array.IndexOf(connectedWaypath.waypath, connectedPoint.transform);
        if (collisionPoint == connectedWaypath.waypath.Length - 1)
        {
            if (verboseConnections) Debug.Log($"Inverting waypath {connectedWaypath.name}");
            Array.Reverse(connectedWaypath.waypath);
        }
    }
    #endregion

    #region Unity Methods
    private void Update()
    {
        if (previousWaypath == null)
        {
            if (pathType != PathType.START_LINE) waypathActive = false;
        }
        else
        {
            if (previousWaypath.waypathActive)
            {
                waypathActive = true;
            }
            else waypathActive = false;
        }
    }
    #endregion endregion
}

