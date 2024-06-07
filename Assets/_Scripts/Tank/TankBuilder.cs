using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public static class TankBuilder
{
    static LinkedList<TankPartSlot> tankSlotToBuildIn = new LinkedList<TankPartSlot>();


    // Start is called before the first frame update
    public static void FindAllSlot()
    {
        tankSlotToBuildIn.Clear();
        Tools.FindComponentsRecursively(GameObject.FindGameObjectWithTag("Player").transform, tankSlotToBuildIn);
    }

    public static LinkedList<TankPartSlot> GetSlotToBuildInList()
    {
        FindAllSlot();
        return tankSlotToBuildIn;
    }


    public static void BuildTankPart(ItemSlot itemSlot, TankPartSlot partSlot)
    {
        
        if (partSlot.name == itemSlot.name)
        {
            if (itemSlot.item == partSlot.GetPartInSlot())
            { return; }
            
            partSlot.RemovePartFromSlot();
            if (itemSlot.item != null)
            {
                GameObject newTankPart = GameObject.Find(itemSlot.item.name);
                if (newTankPart != null)
                {
                    partSlot.PutPartInSlot(newTankPart, itemSlot.item);
                }
            }
        }

    }
}
