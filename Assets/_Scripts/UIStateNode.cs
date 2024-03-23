using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UIStateMachine;

[Serializable]
public class UIStateNode : MonoBehaviour
{
    public GameObject uiState;
    public List<UIStateNode> subStates;

    public UIStateNode(GameObject stateObject)
    {
        uiState = stateObject;
        subStates = new List<UIStateNode>();
    }
}
