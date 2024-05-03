using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<ItemSlot> itemSlots = new List<ItemSlot>();
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
}
