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
            AddNewEquipmentSlots(TankBuilder.GetSlotToBuildInList());
        
    }




    public void AddNewEquipmentSlots(LinkedList<TankPartSlot> slotsToAdd)
    {
        // Clear existing item slots

        // Iterate through each TankPartSlot
        foreach (TankPartSlot slot in slotsToAdd)
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

    public void RemoveEquipmentSlots(LinkedList<TankPartSlot> slotsToRemove)
    {
        // Create a list to store the slots that need to be removed
        List<EquipmentSlot> slotsToBeRemoved = new List<EquipmentSlot>();

        // Iterate through each TankPartSlot
        foreach (TankPartSlot slot in slotsToRemove)
        {
            // Find the corresponding equipment slot in itemSlots
            foreach (EquipmentSlot equipmentSlot in itemSlots)
            {
                if (equipmentSlot.name == slot.name)
                {
                    // Add the equipment slot to the list of slots to be removed
                    slotsToBeRemoved.Add(equipmentSlot);
                }
            }
        }

        // Remove and destroy the slots
        foreach (EquipmentSlot slot in slotsToBeRemoved)
        {
            Debug.Log(slot.name);
            itemSlots.Remove(slot);
            Destroy(slot.gameObject);
        }
    }
}
