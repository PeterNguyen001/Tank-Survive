using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TankPartSlot : MonoBehaviour
{
    public TankPartType tankPartType;
    private TankPartData tankPart;
    // Start is called before the first frame update

    public void PutPartInSlot(GameObject newTankPart, TankPartData part )
    {
        LinkedList<TankPartSlot> slotsToAdd = new LinkedList<TankPartSlot>();
        

        tankPart = part;
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        newTankPart = Object.Instantiate(newTankPart, position, rotation);
        newTankPart.transform.SetParent(transform);

        Tools.FindComponentsRecursively(newTankPart.transform, slotsToAdd);

        UIStateMachine.Instance.PlayerEquipmentInventory.AddNewEquipmentSlots(slotsToAdd);
    }
    public void RemovePartFromSlot()
    {
        LinkedList<TankPartSlot> slotsToRemove = new LinkedList<TankPartSlot>();
        Tools.FindComponentsRecursively(transform, slotsToRemove);
        tankPart = null;
        foreach (Transform child in transform)
        {
            Object.Destroy(child.gameObject);
        }
        UIStateMachine.Instance.PlayerEquipmentInventory.RemoveEquipmentSlots(slotsToRemove);
    }

    public TankPartData GetPartInSlot()
    {
        foreach (Transform child in transform)
        {
            TankPart tankPartComponent = child.GetComponent<TankPart>();
            if (tankPartComponent != null)
            {
                TankPartData part = tankPartComponent.GetTankPart();
                if (part != null)
                {
                    tankPart = part;
                }
            }
        }
        return tankPart;
    }


}
