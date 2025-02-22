using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public PlayerTank PlayerTank { get => playerTank; set => playerTank = value; }

    private PlayerTank playerTank;

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
        //UIStateMachine.Instance.TransitionToStateUsingName("Main Menu");
    }

    public void ActivatePlayer()
    {
        playerTank.enabled = true;
        playerTank.GetComponent<TankPartManager>().ActivateTank();
    }
}
