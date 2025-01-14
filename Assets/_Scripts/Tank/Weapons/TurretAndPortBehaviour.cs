using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAndPortBehaviour : TankPart
{
    public TurretAndGunPortData turretAndGunPortData;
    private GunRotation gunRotation;
    private LinkedList<GunBehaviour> gunUnderTurretControl = new LinkedList<GunBehaviour> ();

    public bool isAIControl;
    public bool isMainGun;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame

    public override void Init()
    {
        gunRotation = new GunRotation(this);
        tankPart = turretAndGunPortData;
        gunUnderTurretControl.Clear();
        Tools.FindComponentsRecursively(transform, gunUnderTurretControl);
        SetUpChildrenBelongToTurret();
    }

    public LinkedList<GunBehaviour> GetGunUnderTurretControl() 
    {
        return gunUnderTurretControl;
    }

    public void AimGunAt(Vector3 posToLookTo)
    {
        gunRotation.GunLookAt(posToLookTo);
    }

    public float GetTurretLocalInitialAngle()
    { return gunRotation.localInitialAngle; }

    public void SetUpChildrenBelongToTurret()
    {
        LinkedList<TankPart> childrenPartList = new LinkedList<TankPart>();
        Tools.FindComponentsRecursively(this.transform, childrenPartList);
        foreach (TankPart child in childrenPartList)
        {
            child.belongToOrIsChassis = false;
            child.belongToOrIsTurret = true;
        }
    }
}
