using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TankBuilder
{
    List<GameObject> slotToBuildInList = new List<GameObject>();

    // Start is called before the first frame update
    public void FindAllSlot()
    {
        slotToBuildInList = GameObject.FindGameObjectsWithTag("Player Tank Part Slot").ToList<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildTankPart(ItemSlot itemSlot)
    {
        FindAllSlot();

        foreach (GameObject slot in slotToBuildInList)
        {
            if (slot.name == itemSlot.name)
            {
                // Collect all children to remove
                List<Transform> childrenToRemove = new List<Transform>();
                foreach (Transform child in slot.transform)
                {
                    Object.Destroy(child.gameObject);
                }


                // Instantiate the new tank part and set it as a child of the slot
                if (itemSlot.item != null)
                {
                    GameObject newTankPart = GameObject.Find(itemSlot.item.name);
                    if (newTankPart != null)
                    {
                        Vector3 position = slot.transform.position;
                        Quaternion rotation = slot.transform.rotation;

                        newTankPart = Object.Instantiate(newTankPart, position, rotation);
                        newTankPart.transform.SetParent(slot.transform);

                        // Update the slotToBuildInList
                    }
                }
            }
        }
    }
}
