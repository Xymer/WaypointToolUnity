using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Waypoint : MonoBehaviour
{    
    [SerializeField]private List<Vector3> waypoints = new List<Vector3>();
    [SerializeField]private int intIndex = 0;
    [SerializeField] private bool isLooping = false;
    
  
    public void AddWaypoint()
    {
        
        if (waypoints.Count == 0)
        {
        waypoints.Add(transform.position);
        }
        else
        {
         waypoints.Add(waypoints[waypoints.Count - 1]);
        }
    }


    public void RemoveWaypoint()
    {
        if (waypoints.Count == 0)
        {
            return;
        }
        else
        {
            waypoints.RemoveAt(waypoints.Count - 1);
        }        
    }

    public List<Vector3> GetWaypointList()
    {
        return waypoints;
    }
}
