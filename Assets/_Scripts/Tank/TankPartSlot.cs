using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPartSlot : MonoBehaviour
{
    public TankPartType tankPartType;
    private Item tankPart;
    // Start is called before the first frame update

    public void PutPartInSlot(GameObject newTankPart, Item part )
    {
        tankPart = part;
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        newTankPart = Object.Instantiate(newTankPart, position, rotation);
        newTankPart.transform.SetParent(transform);
    }
    public void RemovePartFromSlot()
    {
        tankPart = null;
        foreach (Transform child in transform)
        {
            Object.Destroy(child.gameObject);
        }
    }

    public Item GetPartInSlot() 
    {
        return tankPart;
    }
}
