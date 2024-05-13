using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    TankBuilder tankBuilder;
    void Start()
    {
        tankBuilder = new TankBuilder();
        tankBuilder.Init();
        UIStateMachine.Instance.TransitionToStateUsingName("Main Menu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildTank()
    {
        GameObject EquipmentSlots = GameObject.Find("Equipment Slots");

        foreach (Transform slotTransform in EquipmentSlots.transform)
        {
            ItemSlot slot = slotTransform.GetComponent<ItemSlot>();
            if (slot != null) 
            {
                tankBuilder.BuildTankPart(slot);
            }
        }
    }
}
