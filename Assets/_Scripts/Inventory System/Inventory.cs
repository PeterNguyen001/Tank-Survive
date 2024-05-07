using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<ItemSlot> itemSlots = new List<ItemSlot>();
    private List<Item> items = new List<Item>();
    public InventoryType inventoryType;
    [SerializeField]
    GameObject inventoryPanel;

    void Start()
    {

        //Read all itemSlots as children of inventory panel
        itemSlots = new List<ItemSlot>(
            inventoryPanel.transform.GetComponentsInChildren<ItemSlot>()
            );
        
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
