using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TankPart", menuName = "Tank Parts/New Tank Part", order = 1)]
public class TankPartData : Item
{
    // Start is called before the first frame update
    public new string name;// You can use the 'name' field from ScriptableObject
    public TankPartType tankPartType;
}
