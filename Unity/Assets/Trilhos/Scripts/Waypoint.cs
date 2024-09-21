using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypath waypath;

    private string waypointTag = "Waypoint";
    public Waypoint nextWaypoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(waypointTag) && waypath != null)
        {
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
            if (waypath != null && collider.waypath != null)
            {
                waypath.UpdateWaypath(collider);
            }
        }
    }
}

