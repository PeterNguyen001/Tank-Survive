using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the crafting system, including recipe checking and item crafting
public class CraftingSystem : MonoBehaviour
{
    [SerializeField]
    Inventory craftingTable; // Reference to the crafting table inventory

    private List<Item> craftableItems = new List<Item>(); // List of craftable items

    private List<string> recipes = new List<string>(); // List of item recipes as strings

    [SerializeField]
    private ItemSlot resultSlot; // The result slot for crafted items

    private List<ItemSlot> slotOnTable = new List<ItemSlot>(); // Slots on the crafting table

    string currentIngridients; // Current ingredients on the crafting table

    private int numOfItemToCraft; // Number of items to craft

    // Start is called before the first frame update
    void Start()
    {
        ItemIDManager.AssignItemIDs();
        // Load all craftable items
        GetAllCraftableItem();
        currentIngridients = ""; // Initialize current ingredients
        // Get item slots from the crafting table
        slotOnTable = new List<ItemSlot>(craftingTable.GetInventoryPanel().transform.GetComponentsInChildren<ItemSlot>());
        // Process and store recipes for craftable items
        GetAllRecipe();
    }

    private void GetAllRecipe()
    {

        
        for (int i = 0; i < craftableItems.Count; i++)
        {
            Item item = craftableItems[i];
            List<Item> recipe = item.recipe;
            string recipeString = "";

            for (int j = 0; j < recipe.Count; j++)
            {
                recipeString += recipe[j] == null ? " " : recipe[j].ID.ToString();
            }
            recipeString = recipeString.Trim();// Trim whitespace
            recipes.Add(recipeString); // Add recipe string to the list
        }
    }

    private void GetAllCraftableItem()
    {
        Item[] allItems = Resources.LoadAll<Item>("Items"); // Load items from the specified folder
        foreach (Item item in allItems)
        {
            if (item.isCraftable)
            {
                craftableItems.Add(item); // Add craftable items to the list
            }
        }
    }

    // Checks for a valid recipe based on the current ingredients on the table
    public Item CheckForRecipe()
    {
        GetIngridientOnTable(); // Update current ingredients
        Debug.Log("Checking for recipe");

        for (int i = 0; i < recipes.Count; i++)
        {
            if (currentIngridients == recipes[i]) // Check if current ingredients match a recipe
            {
                numOfItemToCraft *= craftableItems[i].recipeMutiplier;
                CraftItem(craftableItems[i]); // Craft the item
                return craftableItems[i]; // Return the crafted item
            }
            else
            {
                RemoveCraftedItem(); // Remove crafted item if no valid recipe is found
            }
        }
        return null; // Return null if no recipe matches
    }

    // Updates the current ingredients based on the items on the crafting table
    public void GetIngridientOnTable()
    {
        FindMaxNumItemToCraft(); // Determine the max number of items to craft
        currentIngridients = "";
        for (int i = 0; i < slotOnTable.Count; i++)
        {
            currentIngridients += slotOnTable[i].item == null ? " " : slotOnTable[i].item.ID.ToString();
        }
        currentIngridients = currentIngridients.Trim(); // Trim whitespace
    }

    // Crafts the specified item
    public void CraftItem(Item item)
    {
        resultSlot.item = item; // Set the item in the result slot
        resultSlot.Count = numOfItemToCraft; // Set the number of crafted items
    }

    // Removes all ingredients from the crafting table
    public void RemoveUsedIgridient()
    {
        foreach (ItemSlot slot in slotOnTable)
        {
            if (slot.item != null)
            {
                slot.Count -= (resultSlot.Count/resultSlot.item.recipeMutiplier); // Clear the item count in the slot
            }
        }
    }

    // Removes a crafted item from the result slot
    public void RemoveCraftedItem()
    {
        if (resultSlot.item != null)
        {
            resultSlot.Count = 0; // Decrease the all of the crafted item
        }
    }
    public void FindMaxNumItemToCraft()
    {
        numOfItemToCraft = int.MaxValue;// Initialize to maximum value
        foreach (ItemSlot itemSlot in slotOnTable)  
        // Iterate through the item slots to find the minimum item to craft
        {
            if (itemSlot.item != null )
            {
                numOfItemToCraft = Math.Min(numOfItemToCraft, itemSlot.Count);
            }
 
        }
    }


}
