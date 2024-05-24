using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    TankBuilder tankBuilder;
    PlayerMovementController playerMovementController;
    PlayerGunController playerGunController;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        playerMovementController = GameObject.FindObjectOfType<PlayerMovementController>();
        playerGunController = GameObject.FindObjectOfType<PlayerGunController>();
        tankBuilder = new TankBuilder();
        UIStateMachine.Instance.TransitionToStateUsingName("Main Menu");
        //DisablePlayerController();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuildTank()
    {
        GameObject EquipmentSlots = GameObject.FindGameObjectWithTag("Equipment Slots");

        foreach (Transform slotTransform in EquipmentSlots.transform)
        {
            ItemSlot slot = slotTransform.GetComponent<ItemSlot>();
            if (slot != null)
            {
                tankBuilder.BuildTankPart(slot);
            }
        }
    }

    public TankBuilder GetTankBuilder()
    { return tankBuilder; }

    public void EnablePlayerController()
    {
        playerMovementController.enabled = true;
        playerGunController.enabled = true;
    }

    public void DisablePlayerController()
    {
        playerMovementController.enabled = false;
        playerGunController.enabled = false;
    }
}
