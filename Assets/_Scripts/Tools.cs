
using System.Collections.Generic;
using UnityEngine;

public static class Tools
{
    // Generic function to find components of type T recursively and add them to the linked list
    public static void FindComponentsRecursively<T>(Transform parent, LinkedList<T> componentList) where T : Component
    {
        foreach (Transform child in parent)
        {
            T component = child.GetComponent<T>();
            if (component != null)
            {
                componentList.AddLast(component);
            }

            // Recursively search in the children of the current child
            FindComponentsRecursively(child, componentList);
        }
    }
}
