using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Waypoint))]
public class WaypointInspector : Editor
{

    Waypoint point;
    static bool insertMenu = true;

    SerializedObject waypointObject;
    SerializedProperty waypointList;
    SerializedProperty intIndex;
    SerializedProperty isLooping;
    private Color lineColor = Color.red;
    private Color buttonColor = Color.blue;
    private float buttonSize = 5f;

    private void OnEnable()
    {
        point = (Waypoint)target;
        waypointObject = new SerializedObject(point);
        waypointList = waypointObject.FindProperty("waypoints");
        intIndex = waypointObject.FindProperty("intIndex");
        isLooping = waypointObject.FindProperty("isLooping");

    }


    public override void OnInspectorGUI()
    {
        waypointObject.Update();

        if (intIndex.intValue > waypointList.arraySize - 1)
        {
            intIndex.intValue = Mathf.Max(0, waypointList.arraySize - 1);
        }
        else
        {
            intIndex.intValue = Mathf.Max(0, intIndex.intValue);
        }

        insertMenu = EditorGUILayout.Foldout(insertMenu, "Waypoint Settings");
        if (insertMenu)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Waypoint", GUILayout.Width(120)))
            {
                if (waypointList.arraySize == 0)
                {
                    AddWaypoint();
                    waypointList.GetArrayElementAtIndex(0).vector3Value = point.transform.position;
                }
                else
                {
                    AddWaypoint();
                }
            }
            if (GUILayout.Button("Remove Waypoint", GUILayout.Width(120)))
            {
                if (waypointList.arraySize > 0)
                {
                    RemoveWaypoint();
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Insert Waypoint at index", GUILayout.Width(200)))
            {
                InsertWaypointAtIndex(intIndex.intValue);
                intIndex.intValue++;
            }
            EditorGUILayout.PropertyField(intIndex);
            GUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(isLooping);
            EditorGUILayout.PropertyField(waypointList);        
        }

        waypointObject.ApplyModifiedProperties();
        if (waypointObject.ApplyModifiedProperties())
        {
            SceneView.RepaintAll();
        }
    }

    private void RemoveWaypoint()
    {
        waypointList.arraySize -= 1;
        intIndex.intValue--;
    }

    private void AddWaypoint()
    {
        waypointList.arraySize += 1;
        intIndex.intValue++;
    }

    private void InsertWaypointAtIndex(int index)
    {
        if (waypointList.arraySize == 0)
        {
            return;
        }
        else if (index > waypointList.arraySize - 1)
        {
            return;
        }
        else
        {
            if (index == waypointList.arraySize - 1)
            {
                waypointList.InsertArrayElementAtIndex(index);
                Vector3 midPoint = waypointList.GetArrayElementAtIndex(waypointList.arraySize - 1).vector3Value.GetMidpoint(waypointList.GetArrayElementAtIndex(index + 1).vector3Value);
                waypointList.GetArrayElementAtIndex(index + 1).vector3Value = midPoint;
            }

            if (index == 0)
            {
                waypointList.InsertArrayElementAtIndex(index);
                Vector3 midPoint = point.transform.position.GetMidpoint(waypointList.GetArrayElementAtIndex(index + 1).vector3Value);
                waypointList.GetArrayElementAtIndex(index).vector3Value = midPoint;
            }

            else
            {
                waypointList.InsertArrayElementAtIndex(index);
                Vector3 midPoint = waypointList.GetArrayElementAtIndex(index - 1).vector3Value.GetMidpoint(waypointList.GetArrayElementAtIndex(index + 1).vector3Value);
                waypointList.GetArrayElementAtIndex(index).vector3Value = midPoint;
            }
        }
    }
    private void OnSceneGUI()
    {
        waypointObject.Update();
        Vector3 targetPosition = Vector3.zero;

        DrawHandles();

        waypointObject.ApplyModifiedProperties();
        if (waypointObject.ApplyModifiedProperties())
        {
            SceneView.RepaintAll();
        }
    }
    private void DrawHandles()
    {


        for (int i = 0; i < waypointList.arraySize; i++)
        {
            Vector3 midPoint = point.transform.position.GetMidpoint(waypointList.GetArrayElementAtIndex(0).vector3Value);

            if (waypointList.arraySize == 1)
            {

                Handles.color = lineColor;
                Handles.DrawLine(point.transform.position, waypointList.GetArrayElementAtIndex(0).vector3Value);
            }

            Handles.color = buttonColor;
            Handles.Label(midPoint + Vector3.up * 2f, "Insert Waypoint");
            if (Handles.Button(midPoint, Quaternion.identity, buttonSize, buttonSize, Handles.CircleHandleCap))
            {
                InsertWaypointAtIndex(0);
            }

            if (i + 1 < waypointList.arraySize)
            {

                midPoint = waypointList.GetArrayElementAtIndex(i).vector3Value.GetMidpoint(waypointList.GetArrayElementAtIndex(i + 1).vector3Value);
                Handles.color = buttonColor;
                Handles.Label(midPoint + Vector3.up * 2f, "Insert Waypoint");
                if (Handles.Button(midPoint, Quaternion.identity, buttonSize, buttonSize, Handles.CircleHandleCap))
                {
                    InsertWaypointAtIndex(i + 1);
                }
                Handles.color = lineColor;
                Handles.DrawLine(point.transform.position, waypointList.GetArrayElementAtIndex(0).vector3Value);
                Handles.DrawLine(waypointList.GetArrayElementAtIndex(i).vector3Value, waypointList.GetArrayElementAtIndex(i + 1).vector3Value);
            }
            if (isLooping.boolValue)
            {
                midPoint = waypointList.GetArrayElementAtIndex(waypointList.arraySize - 1).vector3Value.GetMidpoint(waypointList.GetArrayElementAtIndex(0).vector3Value);
                Handles.color = buttonColor;
                Handles.Label(midPoint + Vector3.up * 2f, "Insert Waypoint");
                if (Handles.Button(midPoint, Quaternion.identity, buttonSize, buttonSize, Handles.CircleHandleCap))
                {
                    InsertWaypointAtIndex(waypointList.arraySize - 1);
                }
                Handles.color = lineColor;
                Handles.DrawLine(waypointList.GetArrayElementAtIndex(waypointList.arraySize - 1).vector3Value, waypointList.GetArrayElementAtIndex(0).vector3Value);
            }
            else if (!isLooping.boolValue)
            {
                Handles.color = buttonColor;
                if (Handles.Button(waypointList.GetArrayElementAtIndex(waypointList.arraySize - 1).vector3Value, Quaternion.identity, buttonSize, buttonSize, Handles.CircleHandleCap))
                {
                    AddWaypoint();
                }
                Handles.Label(waypointList.GetArrayElementAtIndex(waypointList.arraySize - 1).vector3Value + Vector3.up * 3f, "Add Waypoint");
            }

            waypointList.GetArrayElementAtIndex(i).vector3Value = Handles.PositionHandle(waypointList.GetArrayElementAtIndex(i).vector3Value, Quaternion.identity);
            Handles.Label(waypointList.GetArrayElementAtIndex(i).vector3Value + Vector3.down * 2f, "Waypoint - " + i);
        }
    }
}
