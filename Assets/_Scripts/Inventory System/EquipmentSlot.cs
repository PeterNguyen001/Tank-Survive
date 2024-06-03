using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquipmentSlot : ItemSlot
{
    private TextMeshProUGUI slotNameText;
    private TankPartType tankPartType;

    // Override Count property to update tank part when it changes
    public override int Count
    {
        get { return base.Count; }
        set
        {
            base.Count = value;
            UpdateTankPart(); // Call UpdateTankPart whenever the count changes
        }
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
        slotNameText.rectTransform.anchorMin = new Vector2(0.5f, 0); // Anchor to bottom center
        slotNameText.rectTransform.anchorMax = new Vector2(0.5f, 0); // Anchor to bottom center
        slotNameText.rectTransform.pivot = new Vector2(0.5f, 1);     // Pivot at the top center
        slotNameText.rectTransform.anchoredPosition = new Vector2(0, -2); // 20 units below the slot
        slotNameText.fontSize = 10;
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

    // Method to set up the equipment slot
    public void SetUpEquipmentSlot(TankPartSlot tankPartSlot)
    {
        name = tankPartSlot.name;
        tankPartType = tankPartSlot.tankPartType;
        item = tankPartSlot.GetPartInSlot();
        CreateSlotNameText();
        UpdateSlotNameText();
    }

    // Method to update the tank part
    public void UpdateTankPart()
    {
        TankBuilder.BuildTankPart(this);
    }
}
