using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        //foreach ( GameObject part in partsToBuildList ) 
        //{
        //    if( part.name == slot.name ) 
        //    {

        //    }
        //}
    }
}
