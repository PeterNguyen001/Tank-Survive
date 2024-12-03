using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankPartManager : MonoBehaviour
{
    // Start is called before the first frame update;

    private List<TankSubComponent> subComponentList = new List<TankSubComponent>();
    private LinkedList<TankPart> tankPartList = new LinkedList<TankPart>();
    private LinkedList<GunBehaviour> gunList = new LinkedList<GunBehaviour>();
    private LinkedList<TurretAndPortBehaviour> turretAndGunPortList = new LinkedList<TurretAndPortBehaviour>();
    private LinkedList<Collider2D> collider2DList = new LinkedList<Collider2D>();
    private LinkedList<TankPart> disabledTanlPartList = new LinkedList<TankPart>();


    private AmmoLoader loader;
    private TurretController turretController;
    private PlayerTankMovementController movementController;
    private AISensor aISensor;
    private AITankNavigation aITankNavigation;

    public AmmoLoader Loader { get => loader; set => loader = value; }
    public TurretController TurretController { get => turretController; set => turretController = value; }
    public PlayerTankMovementController MovementController { get => movementController; set => movementController = value; }
    public AISensor AISensor { get => aISensor; set => aISensor = value; }
    public AITankNavigation AITankNavigation { get => aITankNavigation; set => aITankNavigation = value; }

    private void Awake()
    {

        //InitializeAllTankPartsAndComponentForTankBuilder();
        //StartBattle();
    }

    private void InitializeAllTankPartsAndComponentForTankBuilder()
    {
        Tools.FindComponentsRecursively(transform, collider2DList);
        Tools.FindComponentsRecursively(transform, subComponentList);
        Tools.FindComponentsRecursively(transform, tankPartList);
    }

    public LinkedList<Collider2D> GetListOfCollider2D()
    {
       return collider2DList;
    }

    public void StartBattle()
    {

        loader = GetComponent<AmmoLoader>();
        turretController = GetComponent<TurretController>();
        movementController = GetComponent<PlayerTankMovementController>();
        aISensor = GetComponent<AISensor>();
        AITankNavigation = GetComponent<AITankNavigation>();

        foreach (TankSubComponent subComponent in subComponentList)
        {
            subComponent.SetManager();
            subComponent.Init();
        }
        foreach (TankPart tankPart in tankPartList)
        {
            tankPart.SetManager(this);
        }

        //loader.Init();
        //turretController.Init();
        //movementController.Init();

    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public LinkedList<GunBehaviour> GetListOfGun()
    {
        gunList.Clear();
        Tools.FindComponentsRecursively(transform, gunList);
        return gunList;
    }
    public LinkedList<TurretAndPortBehaviour> GetListOfTurretAndPort()
    {
        turretAndGunPortList.Clear();
        Tools.FindComponentsRecursively(transform, turretAndGunPortList);
        return turretAndGunPortList;
    }
    //public float GetSpeedKMH()
    //{
    //    // Calculate the speed in meters per second (m/s)
    //    float speedMS = ;

    //    // Convert speed from meters per second (m/s) to kilometers per hour (km/h)
    //    float speedKMH = speedMS * 3.6f; // 1 m/s = 3.6 km/h

    //    return speedKMH;
    //}

}
