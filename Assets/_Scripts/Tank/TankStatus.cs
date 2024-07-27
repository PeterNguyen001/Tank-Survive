using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankStatus : MonoBehaviour
{
    // Start is called before the first frame update
    private static TankStatus _instance;

    private List<TankSubComponent> subComponentList = new List<TankSubComponent>();
    private LinkedList<TankPart> tankPartList = new LinkedList<TankPart>();
    private LinkedList<GunBehaviour> gunList = new LinkedList<GunBehaviour>();
    private LinkedList<TurretAndPortBehaviour> turretAndGunPortList = new LinkedList<TurretAndPortBehaviour>();
    private LinkedList<Collider2D> collider2DList = new LinkedList<Collider2D>();

    public PlayerInput playerInput;

    private AmmoLoader loader;
    private TurretController turretController;
    private PlayerTankMovementController playerMovementController;

    private void Awake()
    {

        InitializeAllTankPartsAndComponentForTankBuilder();
        StartBattle();
    }

    private void InitializeAllTankPartsAndComponentForTankBuilder()
    {

        Tools.FindComponentsRecursively(transform, subComponentList);
        Tools.FindComponentsRecursively(transform, tankPartList);
        foreach (TankPart tankPart in tankPartList)
        {

        }

    }

    public LinkedList<Collider2D> GetListOfCollider2D()
    {
        collider2DList.Clear();
        Tools.FindComponentsRecursively(transform, collider2DList);
       return collider2DList;
    }

    public void StartBattle()
    {
        loader = GetComponent<AmmoLoader>();
        turretController = GetComponent<TurretController>();
        playerMovementController = GetComponent<PlayerTankMovementController>();

        foreach (TankSubComponent subComponent in subComponentList)
        {
            subComponent.SetStatus();
            subComponent.Init();
        }
        //loader.Init();
        //turretController.Init();
        //playerMovementController.Init();

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
