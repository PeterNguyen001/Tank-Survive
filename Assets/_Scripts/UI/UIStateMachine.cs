using System.Collections.Generic;
using UnityEngine;

public class UIStateMachine : MonoBehaviour
{
    private static UIStateMachine _instance;
    public static UIStateMachine Instance { get { return _instance; } }

    public PlayerEquipmentInventory PlayerEquipmentInventory { get => playerEquipmentInventory; set => playerEquipmentInventory = value; }
    public UIStateNode CurrentState { get => currentState; }

    [SerializeField] private UIStateNode currentState;

    private UIStateNode currentSubNode;
    private Stack<UIStateNode> subNodeStack;
    private Dictionary<string, UIStateNode> stateMap;

    private PlayerEquipmentInventory playerEquipmentInventory;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        stateMap = new Dictionary<string, UIStateNode>();
        subNodeStack = new Stack<UIStateNode>();

        UIStateNode[] uiStateNodes = FindObjectsOfType<UIStateNode>();

        playerEquipmentInventory = GameObject.FindObjectOfType<PlayerEquipmentInventory>();

        foreach (UIStateNode stateNode in uiStateNodes)
        {
            stateMap[stateNode.name] = stateNode;
            stateNode.Deactivate(); // Deactivate all UI state objects initially
        }

        // Set initial state (the first UIStateNode found)
        if (uiStateNodes.Length > 0)
        {
            currentState = uiStateNodes[0];
            currentState.Activate(); // Activate the initial UI state object
        }

        
    }

    public void TransitionToStateUsingName(string newStateName)
    {
        if (stateMap.ContainsKey(newStateName))
        {
            UIStateNode nextState = stateMap[newStateName];
            TransitionToStateUsingObject(nextState);
        }
        else
        {
            Debug.LogError("UI state not found: " + newStateName);
        }
    }

    public void TransitionToStateUsingObject(UIStateNode nextState)
    {
        if (currentState != null)
        {
            // Deactivate the current UI state object
            currentState.Deactivate();
        }

        currentState = nextState;
       

        // Activate the new UI state object
        currentState.Activate();
        if (currentState.subStates != null && currentState.subStates.Count > 0)
        {
            subNodeStack.Clear();
            // Automatically push the first substate if available
            PushSubstate();
        }
    }

    public void PushSubstate()
    {
        if (currentState != null && currentState.subStates != null && currentState.subStates.Count > 0)
        {
            int currentSubNodeIndex = subNodeStack.Count;
            if (currentSubNodeIndex < currentState.subStates.Count)
            {
                if (currentSubNode != null)
                {
                    currentSubNode.Deactivate();
                }


               
                currentSubNode = currentState.subStates[currentSubNodeIndex++];
                subNodeStack.Push(currentSubNode);
                currentSubNode = subNodeStack.Peek();
                currentSubNode.Activate();
            }
        }
    }

    public void PopSubstate()
    {
        if (subNodeStack.Count > 1 )
        {
            subNodeStack.Pop();
            // Deactivate the current substate object
            currentSubNode.Deactivate();
            currentSubNode = subNodeStack.Peek();
            // Activate the previous state object
            currentSubNode.Activate();
        }
    }

}
