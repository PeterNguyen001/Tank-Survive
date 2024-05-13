using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBuilder
{
    LinkedList<GameObject> partsToBuildList = new LinkedList<GameObject>();

    // Start is called before the first frame update
    public void Init()
    {
        GameObject chassis = GameObject.Find("Player Tank Chassis");
        partsToBuildList.AddLast(GameObject.Find("Player Tank Chassis"));

        GameObject turretsAndGuns = GameObject.Find("Turrets and Guns");
        foreach ( Transform turretsAndGunsToBuild in turretsAndGuns.transform)
        {
            partsToBuildList.AddLast(turretsAndGunsToBuild.GetComponent<GameObject>());
            Debug.Log(turretsAndGunsToBuild.name);
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
