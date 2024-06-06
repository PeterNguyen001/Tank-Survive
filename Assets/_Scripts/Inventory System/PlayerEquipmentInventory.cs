using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentInventory : Inventory
{
    // Start is called before the first frame update
    GameObject equipmentSlotPrefab;
    void Start()
    {

            equipmentSlotPrefab = GameObject.Find("EquipmentSlotPrefab");
            BuildEquipementSlot();
        
    }




    public void BuildEquipementSlot()
    {
        // Clear existing item slots
        itemSlots.Clear();

        // Iterate through each TankPartSlot
        foreach (TankPartSlot slot in TankBuilder.GetSlotToBuildInList())
        {
            // Instantiate the equipment slot prefab
            GameObject newEquipmentSlot = Object.Instantiate(equipmentSlotPrefab);

            // Add the EquipmentSlot component to the new equipment slot
            EquipmentSlot equipmentSlotScript = newEquipmentSlot.GetComponent<EquipmentSlot>();

            // Set up the slot name
            equipmentSlotScript.SetUpEquipmentSlot(slot);

            // If there is a part in the slot, set the item and count

            newEquipmentSlot.transform.SetParent(inventoryPanel.transform, false);
            itemSlots.Add(equipmentSlotScript);

        }
    }
}
