using UnityEngine;
using System.Collections.Generic;

public static class ItemIDManager
{
    public static void AssignItemIDs()
    {
        // Load all items from the "Items" Resources folder
        Item[] allItems = Resources.LoadAll<Item>("Items");

        HashSet<string> usedIDs = new HashSet<string>();
        foreach (Item item in allItems)
        {
            if (item != null)
            {
                // Generate a unique ID for the item
                string uniqueID = GenerateUniqueID(item.name, usedIDs);
                item.ID = uniqueID; // Assuming each Item has a public ID field to set
                usedIDs.Add(uniqueID);
            }
        }
        Debug.Log("Assigned unique IDs to all items.");
    }

    private static string GenerateUniqueID(string itemName, HashSet<string> usedIDs)
    {
        string baseID = GetBaseID(itemName);

        // Ensure the base ID is unique by appending a number if necessary
        string uniqueID = baseID;
        int counter = 1;
        while (usedIDs.Contains(uniqueID))
        {
            uniqueID = baseID + counter.ToString();
            counter++;
        }

        return uniqueID;
    }

    private static string GetBaseID(string itemName)
    {
        // Take the first two characters of the item's name as the base ID
        return itemName.Substring(0, System.Math.Min(2, itemName.Length)).ToUpper();
    }
}
