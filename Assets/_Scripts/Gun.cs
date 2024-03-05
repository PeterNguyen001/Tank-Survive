using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update
    public GunData gunData;

    private GunRotation gunRotation;

    private GameObject gunEnd;

    private void Start()
    {
        gunRotation = new GunRotation(this);
        FindGunlEnd();
    }

    public void AimGunAtMouse(Vector3 posToLookTo)
    {
        gunRotation.GunLookAt(posToLookTo);
    }
    public void FindGunlEnd()
    {
        gunEnd = transform.Find("Gun End").gameObject;
    }
}
