using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Attributes")]
    [SerializeField] private float moveSpeed = 5f;
    // Flag to enable/disable movement
    public bool followWaypoints = false;

    [Header("Waypaths")]
    // Waypath that the player is currently following
    [SerializeField] private Waypath currentWaypath;
    // Queue of waypoints that the player will follow
    private Queue<Transform> path;

    // Private flags
    private bool startingPathFound = false;

    #region Path Following Methods
    // Follow waypoints queue
    private void followWaypoint()
    {
        if (path != null && path.Count > 0)
        {
            Transform point = path.Peek();

            transform.position = Vector3.MoveTowards(transform.position, point.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, point.position) < 0.1f)
            {
                path.Dequeue();
                if (path.Count <= 0)
                {
                    if (currentWaypath.pathType == Waypath.PathType.GOAL_LINE && LevelManager.Instance != null)
                    {
                        followWaypoints = false;
                        LevelManager.Instance.GoalReached();
                    }

                    if (currentWaypath.nextWaypath != null)
                    {
                        currentWaypath = currentWaypath.nextWaypath;
                        BuildPath();                      
                    }
                }
            }
        }
    }

    // Build waypoints queue with waypoints from current waypath
    private void BuildPath()
    {
        if (currentWaypath != null)
        {
            path = new Queue<Transform>(currentWaypath.waypath);
            //Debug.Log($"Building path with {currentWaypath.name} waypoints");
        }
    }
    #endregion

    #region Unity Methods
    private void Start()
    {
        BuildPath();
    }

    private void Update()
    {
        if (followWaypoints)
        {
            followWaypoint();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Waypoint"))
        {
            Waypoint waypoint = other.GetComponent<Waypoint>();
            if (waypoint.waypath != null && !startingPathFound && waypoint.waypath.pathType == Waypath.PathType.START_LINE)
            {
                currentWaypath = waypoint.waypath;
                startingPathFound = true;
                BuildPath();
                //transform.position = waypoint.transform.position;
            }
        }
    }
    #endregion
}
