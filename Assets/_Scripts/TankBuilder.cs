using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class TankBuilder
{
    List<GameObject> partsToBuildList = new List<GameObject>();

    // Start is called before the first frame update
    public void Init()
    {
        partsToBuildList = GameObject.FindGameObjectsWithTag("Player Tank Part").ToList<GameObject>();

        foreach (GameObject part in partsToBuildList) 
        {
            Debug.Log(part.name);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildTankPart(ItemSlot slot)
    {
        foreach (GameObject part in partsToBuildList)
        {
            if (part.name == slot.name)
            {
                GameObject newTankPart = GameObject.Find(slot.item.name);
                if (newTankPart != null) 
                {

                    Vector3 position = part.transform.position;
                    Quaternion rotation = part.transform.rotation;
                    Transform parent = part.transform.parent;
                    newTankPart = Object.Instantiate(newTankPart, position, rotation);

                    newTankPart.transform.parent = parent;

                    partsToBuildList.Remove(part);
                    partsToBuildList.Add(newTankPart);
                    Object.Destroy(part);

                }
            }
        }
        //BUG create a new list for the for each
    }
}
