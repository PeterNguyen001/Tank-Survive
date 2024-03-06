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

    private int currentAmmoCount;

    private bool canFire = true;

    private void Start()
    {
        gunRotation = new GunRotation(this);
        FindGunlEnd();

        currentAmmoCount = gunData.ammoCapacity;
    }

    public void AimGunAtMouse(Vector3 posToLookTo)
    {
        gunRotation.GunLookAt(posToLookTo);
    }
    public void FindGunlEnd()
    {
        gunEnd = transform.Find("Gun End").gameObject;
    }
    public void FireGun(bool isFiring)
    {
        if (isFiring)
        {
            if (canFire)
            {
                bullet.transform.position = gunEnd.transform.position;
                bullet.transform.rotation = gunEnd.transform.rotation;
                if (!gunData.isFullAuto)
                {
                    FireSemiAuto();
                }
                else
                {
                    StartCoroutine(FireFullAuto());
                }
                currentAmmoCount--;
                if (currentAmmoCount == 0)
                {
                    canFire = false;
                    StartCoroutine(Reload());
                }
            }
        }
    }
    public void FireSemiAuto()
    {
        if (canFire)
        {
            bullet.Fire();
        }
    }

    private IEnumerator FireFullAuto()
    {
        while (canFire)
        {
            bullet.Fire();

            // Calculate the delay between shots based on shotPerMinute
            float delayBetweenShots = 60f / gunData.shotPerMinute;
            yield return new WaitForSeconds(delayBetweenShots);
        }
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(gunData.reloadTime);
        currentAmmoCount = gunData.ammoCapacity;
        canFire = true;
    }
}
