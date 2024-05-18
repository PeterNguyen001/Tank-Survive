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
    public void Init()
    {
        slotToBuildInList = GameObject.FindGameObjectsWithTag("Player Tank Part Slot").ToList<GameObject>();

        foreach (GameObject part in slotToBuildInList) 
        {
            Debug.Log(part.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildTankPart(ItemSlot ItemSlot)
    {


        foreach (GameObject slot in slotToBuildInList)
        {
            if (slot.name == ItemSlot.name)
            {
                GameObject oldTankPart = slot.transform.GetChild(0).GameObject();
                GameObject newTankPart = GameObject.Find(ItemSlot.item.name);
                if (newTankPart != null)
                {

                    Vector3 position = oldTankPart.transform.position;
                    Quaternion rotation = oldTankPart.transform.rotation;
     
                    newTankPart = Object.Instantiate(newTankPart, position, rotation);
                    Object.Destroy(oldTankPart);
                    newTankPart.transform.parent = slot.transform;
                }
            }
        }
    }
}
