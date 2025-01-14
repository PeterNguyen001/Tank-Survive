using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chassis : TankPart
{
    public ChassisData chassisData;

    [SerializeField]
    private Armor frontArmor;
    [SerializeField]
    private Armor rightArmor;
    [SerializeField]
    private Armor leftArmor;
    
  


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Init()
    {
        tankPart = chassisData;
        armorList = new Armor[] {frontArmor, rightArmor, leftArmor };
        SetTankPartForArmor();
        SetUpChildrenBelongToChassis();
    }

    public void InitializeCollider()
    {
        PolygonCollider2D meshCollider = GetComponent<PolygonCollider2D>();
        if (meshCollider == null)
        {
            meshCollider = gameObject.AddComponent<PolygonCollider2D>();
        }
    }

    public void SetUpChildrenBelongToChassis()
    {
        LinkedList<TankPart> childrenPartList = new LinkedList<TankPart>();
        Tools.FindComponentsRecursively(this.transform, childrenPartList);
        foreach (TankPart child in childrenPartList)
        {
            child.belongToOrIsChassis = true;
        }
    }
}
