using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIStateMachine.Instance.TransitionToStateUsingName("Main Menu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildTank()
    {
        //RectTransform EquipmentSlots = gameObject.get
        //Debug.Log(EquipmentSlots.name);
        //foreach (ItemSlot slot in EquipmentSlots) 
        //{
        //    Debug.Log(slot.name);
        //}
    }
}
