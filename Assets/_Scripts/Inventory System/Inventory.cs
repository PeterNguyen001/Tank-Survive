using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    protected List<ItemSlot> itemSlots = new List<ItemSlot>();
    private List<Item> items = new List<Item>();
    public InventoryType inventoryType;

    
    [SerializeField]
    protected GameObject inventoryPanel;

    void Start()
    {

        //Read all itemSlots as children of inventory panel
        //Tools.FindComponentsRecursively(inventoryPanel.transform, itemSlots);
    }

    public GameObject GetInventoryPanel()
    {
        return inventoryPanel;
    }

    public List<Item> GetCurrentItems()
    {
        foreach (ItemSlot slot in itemSlots)
        {
            if (slot.GetItem() != null)
                items.Add(slot.GetItem());
        }
        return items;
    }

    

}
