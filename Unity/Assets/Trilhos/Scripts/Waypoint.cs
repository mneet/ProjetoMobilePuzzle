using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    // Path that this point belongs to
    public Waypath waypath;

    // Tag name for collision checking
    private string waypointTag = "Waypoint";

    #region Unity Methods
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(waypointTag) && waypath != null)
        {
            // Starting Line Check
            if (waypath.pathType == Waypath.PathType.START_LINE)
            {
                int myPoint = Array.IndexOf(waypath.waypath, transform);
                if (myPoint == 0)
                {
                    return;
                }
            }
            Waypoint collider = other.GetComponent<Waypoint>();
            if (waypath != null && collider.waypath != null)
            {
                waypath.ConnectWaypath(collider);
            }
            
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag(waypointTag))
        {
            Waypoint collider = other.GetComponent<Waypoint>();
            if (waypath != null && collider.waypath != null)
            {
                waypath.DisconnectWaypath(collider.waypath);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(waypointTag))
        {
            Waypoint collider = other.GetComponent<Waypoint>();
            if (waypath != null && collider.waypath != null && waypath.nextWaypath == null)
            {
                waypath.UpdateWaypath(collider);
            }
        }
    }
    #endregion
}

