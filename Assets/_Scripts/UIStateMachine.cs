using System.Collections.Generic;
using UnityEngine;

public class UIStateMachine : MonoBehaviour
{
    private static UIStateMachine _instance;
    public static UIStateMachine Instance { get { return _instance; } }
    [SerializeField] private UIStateNode currentState;
    private Stack<UIStateNode> stateStack;
    private Dictionary<string, UIStateNode> stateMap;

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
        stateStack = new Stack<UIStateNode>();

        UIStateNode[] uiStateNodes = FindObjectsOfType<UIStateNode>();

        foreach (UIStateNode stateNode in uiStateNodes)
        {
            stateMap[stateNode.name] = stateNode;
            stateNode.gameObject.SetActive(false); // Deactivate all UI state objects initially
        }

        // Set initial state (the first UIStateNode found)
        if (uiStateNodes.Length > 0)
        {
            currentState = uiStateNodes[0];
            currentState.gameObject.SetActive(true); // Activate the initial UI state object
        }
    }

    public void TransitionToState(string newStateName)
    {
        if (stateMap.ContainsKey(newStateName))
        {
            UIStateNode nextState = stateMap[newStateName];
            if (currentState != null)
            {
                // Deactivate the current UI state object
                currentState.gameObject.SetActive(false);
            }
            currentState = nextState;
            // Activate the new UI state object
            currentState.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("UI state not found: " + newStateName);
        }
    }

    public void PushSubstate(string substateName)
    {
        if (currentState != null)
        {
            foreach (UIStateNode subNode in currentState.subStates)
            {
                if (subNode.name == substateName)
                {
                    stateStack.Push(currentState);
                    currentState = subNode;
                    // Activate the new substate object
                    currentState.gameObject.SetActive(true);
                    return;
                }
            }
        }
    }

    public void PopSubstate()
    {
        if (stateStack.Count > 0)
        {
            UIStateNode prevState = stateStack.Pop();
            // Deactivate the current substate object
            currentState.uiState.SetActive(false);
            currentState = prevState;
            // Activate the previous state object
            currentState.uiState.SetActive(true);
        }
    }
}
