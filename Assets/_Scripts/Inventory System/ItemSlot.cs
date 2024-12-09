using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Manages individual item slots in the inventory, including interactions and UI updates
public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    public Item item = null; // The item in the slot
    [SerializeField]
    private TextMeshProUGUI descriptionText; // Text field for the item's description
    [SerializeField]
    private TextMeshProUGUI nameText; // Text field for the item's name
    private ClickType clickType; // Type of click interaction (e.g., normal, shift-click, ctrl-click)

    public SlotType slotType;

    [SerializeField]
    private int count = 0; // The number of items in the slot
    public virtual int Count
    {
        get { return count; }
        set
        {
            count = value;
            UpdateGraphic(); // Update the slot's graphics when the count changes
        }
    }

    [SerializeField]
    private Image itemIcon; // Icon representing the item
    [SerializeField]
    private TextMeshProUGUI itemCountText; // Text field for the item's count

    private InventoryManager inventoryManager; // Reference to the InventoryManager
    private bool isSource; // Indicates if the slot is the source of an item transfer

    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        UpdateGraphic(); // Initialize the slot's graphics
    }

    // Updates the item slot's icon and count visibility
    public void UpdateGraphic()
    {
        itemIcon.gameObject.SetActive(true);
        if (count < 1)
        {
            item = null;
            Color color = itemIcon.color;
            color.a = 0f; // Make icon transparent
            itemIcon.color = color;
            itemCountText.gameObject.SetActive(false);
        }
        else
        {
            Color color = itemIcon.color;
            color.a = 1f; // Make icon opaque
            itemIcon.color = color;
            itemIcon.sprite = item.icon; // Set the icon sprite
            itemCountText.gameObject.SetActive(true);
            itemCountText.text = count.ToString(); // Display the item count
        }
    }

    //Uses the item in the slot or processes the item click
    public void UseItemInSlot()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            clickType = ClickType.TakeHalf;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            clickType = ClickType.TakeAll;
        }
        else
        {
            clickType = ClickType.Default;
        }

        if (HasMultipleInventoriesOpened())
        {
            inventoryManager.ProcessItemSlot(this, clickType); // Process item slot in the context of multiple inventories
        }
        else
        {
            if (CanUseItem())
            {
                item.Use(); // Use the item if possible
                if (item.isConsumable)
                {
                    Count--; // Decrease count for consumable items
                }
            }
        }
    }

    // Checks if the item can be used
    private bool CanUseItem()
    {
        return (item != null && count > 0); // Item is usable if it exists and count is greater than 0
    }

    // Handles pointer enter event for showing item details
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            descriptionText.text = item.description; // Show item description
            nameText.text = item.name; // Show item name
        }
    }

    // Handles pointer exit event for hiding item details
    public void OnPointerExit(PointerEventData eventData)
    {
        if (item != null)
        {
            descriptionText.text = ""; // Hide item description
            nameText.text = ""; // Hide item name
        }
    }

    // Checks if multiple inventories are opened
    public bool HasMultipleInventoriesOpened()
    {
        Inventory[] inventories = FindObjectsOfType<Inventory>();
        return inventories.Length >= 2; // True if two or more inventories are open
    }

    // Handles right-click on an item slot
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Add right-click functionality here if needed
        }
    }

    public void TakeInItem( Item item, int count)
    {

    }

public Item GetItem()
    {
            return item;
    }
}

public enum SlotType
{ 
   Inventory,
   Shop,
   EquipmentSlot,
   AmmoSlot
}

