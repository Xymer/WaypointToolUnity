using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private Waypoint waypoint;
    private List<Vector3> waypoints;
    [SerializeField] private bool isLooping = false;
    private int currentPath = 0;
    [SerializeField] private float moveSpeed = 100f;
    private float distanceToNextPos = 0.5f;

    private void OnEnable()
    {
        waypoint = GetComponent<Waypoint>();
        waypoints = waypoint.GetWaypointList();

    }
    private void Update()
    {
        if (currentPath > waypoints.Count - 1)
        {
            if (isLooping)
            {
                currentPath = 0;
            }
            else
            {
                return;
            }
        }
        else
        {

            if (Vector3.Distance(transform.position, waypoints[currentPath]) < distanceToNextPos)
            {
                currentPath++;

            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, waypoints[currentPath], Time.deltaTime * moveSpeed);
            }
        }
    }


}
