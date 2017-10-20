using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridManager))] //Asigns this to the GridManager script
public class GridBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridManager gridManager = (GridManager)target;
        if (GUILayout.Button("Build Grid")) //Returns true if button has been pressed, also creates the button by writing this line
        {
            gridManager.buildGrid();
        }
    }
}
