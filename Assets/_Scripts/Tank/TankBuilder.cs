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


    public static void BuildTankPart(ItemSlot itemSlot)
    {
        FindAllSlot();
        foreach (TankPartSlot tankPartSlot in tankSlotToBuildIn) 
        {
            if (tankPartSlot.name == itemSlot.name)
            {
                tankPartSlot.RemovePartFromSlot();
                if (itemSlot.item != null)
                {
                    GameObject newTankPart = GameObject.Find(itemSlot.item.name);
                    if (newTankPart != null)
                    {
                        tankPartSlot.PutPartInSlot(newTankPart, itemSlot.item);
                    }
                }
            }
        }
    }
}
