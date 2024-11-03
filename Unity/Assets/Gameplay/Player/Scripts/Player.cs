using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Attributes")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    // Flag to enable/disable movement
    public bool followWaypoints = false;

    [Header("Waypaths")]
    // Waypath that the player is currently following
    [SerializeField] private Waypath currentWaypath;
    // Queue of waypoints that the player will follow
    private Queue<Transform> path;

    [Header("Wobble")]
    [SerializeField] private float wobbleIntensity = 0.2f;
    [SerializeField] private float wobbleFrequency = 1f;
    [SerializeField] private bool wobbleCar = false;
    [SerializeField] private float wobbleDecay = 0.045f;
    private float wobbleIntenisityMeter = 0.2f;

    // Private flags
    private bool startingPathFound = false;

    #region Path Following Methods

    private void lookAtWaypoint(Transform point)
    {
        // Calcular a direção apenas horizontalmente
        Vector3 direction = new Vector3(point.position.x - transform.position.x, 0, point.position.z - transform.position.z).normalized;

        // Criar a rotação alvo apenas no eixo Y
        if (direction != Vector3.zero)
        {
            // LookAt não funciona apenas com o eixo Y (Y = 0)
            // Quaternion targetRotation = Quaternion.LookRotation(direction)
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Manter a rotação atual em X e Z e aplicar a nova rotação em Y
            Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, newRotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        }
    }
    // Follow waypoints queue
    private void followWaypoint()
    {
        if (path != null && path.Count > 0)
        {
            Transform point = path.Peek();

            Vector3 targetPosition = new Vector3(point.position.x, transform.position.y, point.position.z);
            transform.position = Vector3.MoveTowards(transform.position, point.position, moveSpeed * Time.deltaTime);
            lookAtWaypoint(point);

            if (Vector3.Distance(transform.position, point.position) < 0.1f)
            {
                path.Dequeue();
                if (path.Count <= 0)
                {
                    if (currentWaypath.pathType == Waypath.PathType.GOAL_LINE)
                    {
                        followWaypoints = false;
                        LevelManager.Instance.GoalReached();
                    }

                    if (currentWaypath.nextWaypath != null)
                    {
                        currentWaypath = currentWaypath.nextWaypath;
                        BuildPath();   
                      
                    }
                    else
                    {
                        if (currentWaypath.pathType != Waypath.PathType.GOAL_LINE)
                        {
                            ShakeCar();
                            LevelManager.Instance.LoseLevel();
                        }
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

        if (wobbleCar)
        {
            WobbleUpdate();
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
                waypoint.waypath.InvertWaypath(waypoint);
                BuildPath();

                transform.position = new Vector3(waypoint.transform.position.x, transform.position.y, waypoint.transform.position.z);

                path.Dequeue();

                Transform[] pathQueue = path.ToArray();
                Transform point = pathQueue[0];

                // Calcular a direção apenas horizontalmente
                Vector3 direction = new Vector3(point.position.x - transform.position.x, 0, point.position.z - transform.position.z).normalized;
                
                // Criar a rotação alvo apenas no eixo Y
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                Debug.Log(targetRotation);
                transform.rotation = targetRotation;
            }
        }
    }
    #endregion

    private void WobbleUpdate()
    {
        float wobbleAmount = Mathf.Sin(Time.time * wobbleFrequency) * wobbleIntenisityMeter;
        transform.Rotate(0, 0, wobbleAmount - transform.rotation.eulerAngles.z);

        wobbleIntenisityMeter = Mathf.Lerp(wobbleIntenisityMeter, 0, wobbleDecay);
        if (wobbleIntenisityMeter < 0.01f)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            wobbleIntenisityMeter = wobbleIntensity;
            wobbleCar = false;
        }
    }
    public void ShakeCar()
    {
        wobbleIntenisityMeter = wobbleIntensity;
        wobbleCar = true;
    }
}
