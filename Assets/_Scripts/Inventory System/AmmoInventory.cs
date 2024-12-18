using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoInventory : Inventory
{
    List<ItemSlot> slots = new List<ItemSlot>();
    // Start is called before the first frame update

        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<ItemSlot> GetListOfAmmoSlot()
    {
        Tools.FindComponentsRecursively(this.transform, slots, true);
        return slots;
    }
}
