using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Inventory sourceInventory;
    public Inventory targetInventory;

    [SerializeField]
    private ItemSlot itemSlotTemp;

    private ItemSlot sourceSlot;
    private ItemSlot targetSlot;

    Transform targetParent;
    Transform targetGrandParent;

    Transform sourceParent;
    Transform sourceGrandParent;

    private void Start()
    {
        itemSlotTemp = GetComponent<ItemSlot>();
    }

    public void TransferItemToInventory(ItemSlot sourceSlot, ItemSlot targetSlot, ClickType clickType)
    {
        // Set the current source and target inventories based on the slots
        //SetInventory(sourceSlot, targetSlot);

        // Check if the source slot has an item to transfer
        if (sourceSlot.item != null)
        {
            if (targetSlot is EquipmentSlot equipmentSlot)
            {
                if (!(sourceSlot.item is TankPartData tankPartData))
                {
                    Debug.LogWarning("Cannot transfer item. Target slot only accepts TankPartData items.");
                    return;
                }

                if (equipmentSlot.TankPartType != tankPartData.tankPartType)
                {
                    Debug.LogWarning("Cannot transfer item. Target slot only accepts TankPartData items of the same type.");
                    return;
                }

            }

            int transferAmount = 0;

            // Determine the amount to transfer based on the click type
            switch (clickType)
            {
                case ClickType.TakeHalf:
                    // Calculate half of the source slot's count, rounding up if it's an odd number
                    transferAmount = (sourceSlot.Count + 1) / 2;
                    break;
                case ClickType.TakeAll:
                    // Transfer all items from the source slot
                    transferAmount = sourceSlot.Count;
                    break;
                case ClickType.Default:
                    // Default behavior, e.g., moving one item at a time
                    transferAmount = 1;
                    break;
            }

            // Prevent transferring items into the ResultSlot
            //if (targetSlot.transform.name == "ResultSlot")
            //{
            //    return; // Exit the method without performing any transfer
            //}

           
            // Transfer logic when the target slot is empty
            if (targetSlot.item == null)
            {
                // Assign the item from the source to the target slot
                targetSlot.item = sourceSlot.item;

               
                targetSlot.Count += transferAmount;
                sourceSlot.Count -= transferAmount;


            }
            // Transfer logic when the target slot has the same item type
            else if (sourceSlot.item == targetSlot.item)
            {
                // Same as above, adjust the item counts for both slots
                targetSlot.Count += transferAmount;
                sourceSlot.Count -= transferAmount;
            }

        }

    }


    public bool IsTempEmpty()
    {
        // Returns true if the temporary item slot has no item, otherwise false
        return itemSlotTemp.item == null;
    }

    // Processes an item slot based on the click type
    public void ProcessItemSlot(ItemSlot itemSlot, ClickType clickType)
    {
        Debug.Log(clickType.ToString());
        // If the temporary slot is empty, set the source and target to the clicked slot and temporary slot
        if (IsTempEmpty())
        {
            Debug.Log("empty");
            sourceSlot = itemSlot;
            targetSlot = itemSlotTemp;
        }
        // If the clicked item slot is empty, transfer item from the temp slot to the clicked slot
        else if (itemSlot.item == null)
        {
            sourceSlot = itemSlotTemp;
            targetSlot = itemSlot;
        }
        // If the source slot is not the clicked item slot, transfer item from temp to clicked slot
        else if (sourceSlot != itemSlot)
        {
            sourceSlot = itemSlotTemp;
            targetSlot = itemSlot;
        }

        // Transfer item between source and target slots based on click type
        TransferItemToInventory(sourceSlot, targetSlot, clickType);
    }

    // Sets the source and target inventories based on the item slots
    //public void SetInventory(ItemSlot sourceSlot, ItemSlot targetSlot)
    //{
    //    // Get the parent and grandparent of the target slot
    //    targetParent = targetSlot.transform.parent;
    //    targetGrandParent = targetParent.transform.parent;
    //    // If the grandparent of the target slot exists, get its Inventory component
    //    if (targetGrandParent != null)
    //    {
    //        targetInventory = targetGrandParent.GetComponent<Inventory>();
    //    }

    //    // Get the parent and grandparent of the source slot
    //    sourceParent = sourceSlot.transform.parent;
    //    sourceGrandParent = sourceParent.transform.parent;
    //    // If the grandparent of the source slot exists, get its Inventory component
    //    if (sourceGrandParent != null)
    //    {
    //        sourceInventory = sourceGrandParent.GetComponent<Inventory>();
    //    }
    //}


}

public enum ClickType
{
    Default = 0,
    TakeHalf = 1,
    TakeAll = 2
}


