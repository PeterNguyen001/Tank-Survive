using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<ItemSlot> itemSlots = new List<ItemSlot>();
    private List<Item> items = new List<Item>();
    public InventoryType inventoryType;

    GameObject equipmentSlotPrefab ;
    [SerializeField]
    GameObject inventoryPanel;

    void Start()
    {
        if (inventoryType == InventoryType.Equiping)
        {
            equipmentSlotPrefab = GameObject.Find("EquipmentSlotPrefab");
            BuildEquipementSlot();
        }
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
        // Clear existing item slots
        itemSlots.Clear();

        // Iterate through each TankPartSlot
        foreach (TankPartSlot slot in TankBuilder.GetSlotToBuildInList())
        {
            Debug.Log(slot.name);
            // Instantiate the equipment slot prefab
            GameObject newEquipmentSlot = Object.Instantiate(equipmentSlotPrefab);

            // Add the EquipmentSlot component to the new equipment slot
            EquipmentSlot equipmentSlotScript = newEquipmentSlot.GetComponent<EquipmentSlot>();

            // Set up the slot name
            equipmentSlotScript.SetUpEquipmentSlot(slot);

            // If there is a part in the slot, set the item and count
            if (slot.GetPartInSlot() != null)
            {
                equipmentSlotScript.item = slot.GetPartInSlot();
                equipmentSlotScript.Count = 1;
            }
            newEquipmentSlot.transform.SetParent(inventoryPanel.transform, false);


        }
    }

}
