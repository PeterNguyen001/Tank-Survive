using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update
    public GunData gunData;

    private GunRotation gunRotation;

    private GameObject gunEnd;

    public BulletAndShellBehavior bullet;

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
    public void FireGun()
    {
        bullet.transform.position = gunEnd.transform.position;
        bullet.transform.rotation = gunEnd.transform.rotation;
        bullet.Fire();
    }
}
