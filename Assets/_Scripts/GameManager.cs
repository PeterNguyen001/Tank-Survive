using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    PlayerTankMovementController playerMovementController;
    TurretController playerGunController;
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
        playerMovementController = GameObject.FindObjectOfType<PlayerTankMovementController>();
        playerGunController = GameObject.FindObjectOfType<TurretController>();
        UIStateMachine.Instance.TransitionToStateUsingName("Main Menu");
    }

    // Update is called once per frame
    void Update()
    {

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
