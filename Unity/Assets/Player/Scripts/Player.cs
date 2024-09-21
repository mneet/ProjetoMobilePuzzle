using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    [SerializeField] private Waypath currentWaypath;
    private Queue<Transform> path;

    [SerializeField] private bool followWaypoints = false;

    private void followWaypoint()
    {
        if (path.Count > 0)
        {
            Transform point = path.Peek();

            transform.position = Vector3.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, point.position) < 0.1f)
            {
                path.Dequeue();
                if (path.Count <= 0 && currentWaypath.nextWaypath != null)
                {
                    currentWaypath = currentWaypath.nextWaypath;
                    BuildPath();
                }
            }
        }
    }

    private void BuildPath()
    {
        if (currentWaypath != null)
        {
            path = new Queue<Transform>(currentWaypath.waypath);
            Debug.Log($"Building path with {currentWaypath.name} waypoints");
        }
    }

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
}
