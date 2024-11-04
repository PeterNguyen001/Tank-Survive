using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSubComponent : MonoBehaviour
{
    protected TankPartManager tankPartManager;
    // Start is called before the first frame update
    public virtual void Init()
    {
    }

    public void SetManager()
    {

        tankPartManager = gameObject.GetComponent<TankPartManager>();
    }
}
