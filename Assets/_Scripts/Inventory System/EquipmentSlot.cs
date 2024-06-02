using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquipmentSlot : ItemSlot
{
    private TextMeshProUGUI slotNameText;
    private TankPartType tankPartType;

    // Constructor to initialize the slot name and create the TextMeshPro box
    public EquipmentSlot(TankPartSlot tankPartSlot)
    {
        name = tankPartSlot.name;
        tankPartType = tankPartSlot.tankPartType;
        CreateSlotNameText();
        UpdateSlotNameText();
    }

    public override int Count
    {
        get { return base.Count; }
        set
        {
            base.Count = value;
            UpdateTankPart(); // Call UpdateTankPart whenever the count changes
        }
    }

    public void UpdateTankPart()
    {
        TankBuilder.BuildTankPart(this);
    }

    // Method to create the TextMeshPro box
    private void CreateSlotNameText()
    {
        // Create a new GameObject for the TextMeshPro element
        GameObject textObject = new GameObject("SlotNameText");
        textObject.transform.SetParent(this.transform);

        // Add a TextMeshProUGUI component to the GameObject
        slotNameText = textObject.AddComponent<TextMeshProUGUI>();

        // Set the position and other properties of the TextMeshPro element
        slotNameText.rectTransform.anchoredPosition = new Vector2(0, 0);
        slotNameText.fontSize = 24;
        slotNameText.alignment = TextAlignmentOptions.Center;

        // Optional: Set other TextMeshPro properties as needed
    }

    // Method to update the TextMeshPro text with the slot name
    private void UpdateSlotNameText()
    {
        if (slotNameText != null)
        {
            slotNameText.text = name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Ensure the slot name text is always updated (if needed)
        UpdateSlotNameText();
    }
}
