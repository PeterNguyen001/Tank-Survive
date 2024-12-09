
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
public static class Tools
{
    // Generic function to find components of type T recursively and add them to the linked list
    public static void FindComponentsRecursively<T>(Transform parent, ICollection<T> componentCollection, bool ignoreParent = false) where T : Component
    {
        // Check if the parent transform itself has the component(s)
        if (!ignoreParent)
        {
            T[] parentComponents = parent.GetComponents<T>();
            foreach (T component in parentComponents)
            {
                if (!componentCollection.Contains(component))
                {
                    componentCollection.Add(component);
                }
            }
        }
        // Iterate through each child of the parent transform
        foreach (Transform child in parent)
        {

            // Check if the child has the component(s)
            T[] childComponents = child.GetComponents<T>();
            foreach (T component in childComponents)
            {
                if (!componentCollection.Contains(component))
                {
                    componentCollection.Add(component);
                }
            }

            // Recursively search in the children of the current child
            FindComponentsRecursively(child, componentCollection);
        }
    }



    public static T FindComponentRecursively<T>(Transform parent) where T : Component
    {
        foreach (Transform child in parent)
        {
            T component = child.GetComponent<T>();
            if (component != null)
            {
                return component;
            }

            // Recursively search in the children of the current child
            T foundComponent = FindComponentRecursively<T>(child);
            if (foundComponent != null)
            {
                return foundComponent;
            }
        }

        // Return null if no component is found
        return null;
    }

    public static void PauseEditor()
    {
#if UNITY_EDITOR
        // Check if the application is currently playing and not paused
        if (EditorApplication.isPlaying && !EditorApplication.isPaused)
        {
            EditorApplication.isPaused = true; // Pause the editor
            Debug.Log("Editor paused!");
        }
#endif
    }
}
