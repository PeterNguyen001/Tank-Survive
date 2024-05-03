using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    SerializedProperty recipeProp;
    SerializedProperty gridSizeProp;

    void OnEnable()
    {
        // Cache the SerializedProperties
        recipeProp = serializedObject.FindProperty("recipe");
        gridSizeProp = serializedObject.FindProperty("recipeGridSize");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update(); // Always call this before drawing the inspector

        // Draw the default inspector elements
        DrawDefaultInspector();

        // Now draw the recipe grid if the item is craftable
        Item item = (Item)target;
        if (item.isCraftable)
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(gridSizeProp, new GUIContent("Grid Size"));
            int gridSize = gridSizeProp.intValue;

            // Resize the recipe list to match the specified grid size
            while (recipeProp.arraySize < gridSize)
            {
                recipeProp.InsertArrayElementAtIndex(recipeProp.arraySize);
            }
            while (recipeProp.arraySize > gridSize)
            {
                recipeProp.DeleteArrayElementAtIndex(recipeProp.arraySize - 1);
            }

            // Custom layout for the recipe list
            EditorGUILayout.LabelField("Recipe Grid");
            EditorGUILayout.BeginVertical();
            int rowLength = Mathf.FloorToInt(Mathf.Sqrt(gridSize)); // For a square grid

            for (int i = 0; i < gridSize; i++)
            {
                if (i % rowLength == 0) EditorGUILayout.BeginHorizontal();

                SerializedProperty itemProp = recipeProp.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(itemProp, GUIContent.none);

                if ((i + 1) % rowLength == 0 || i == gridSize - 1) EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        serializedObject.ApplyModifiedProperties(); // Apply changes to all serializedProperties - always call this at the end of OnInspectorGUI
    }
}
