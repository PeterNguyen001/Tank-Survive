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

    public void BuildEquipementSlot()
    {
        itemSlots.Clear();
        foreach (TankPartSlot slot in TankBuilder.GetSlotToBuildInList())
        {
            EquipmentSlot equipmentSlot = new EquipmentSlot(slot);
            if(slot.GetPartInSlot() != null) 
            {
                equipmentSlot.item = slot.GetPartInSlot();
                equipmentSlot.Count = 1;
            }
            equipmentSlot.transform.SetParent(inventoryPanel.transform, false);
        }
    }
}
