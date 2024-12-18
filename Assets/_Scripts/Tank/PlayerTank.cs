using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTank : MonoBehaviour
{
    private TankPartManager partManager;

    public TankPartManager PartManager { get => partManager; set => partManager = value; }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.PlayerTank = this;
        partManager = GetComponent<TankPartManager>();
        enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        partManager.MovementController.MovePlayerTankBaseOnInput();
        partManager.TurretController.ControlTurretAndWeaponBaseOnInput();
    }
}
