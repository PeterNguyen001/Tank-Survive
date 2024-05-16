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
