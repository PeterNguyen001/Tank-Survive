using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GunData gunData;
    private GunRotation gunRotation;
    private GameObject gunEnd;

    private AmmoLoader loader;
    private GameObject bulletPrefab; // Prefab for the bullet
    private List<GameObject> bulletPool = new List<GameObject>();
    public int currentAmmoCount;

    public bool hasAmmo = false;
    bool isDelaying = false;
    private bool isReloading = false;

    private void Start()
    {

        gunRotation = new GunRotation(this);
        FindGunlEnd();
    }

    public void InitializeBulletPool()
    {
        loader = FindObjectOfType<AmmoLoader>();
        bulletPrefab = loader.FindCorrectAmmunitionType(gunData.ammunitionData).GetbulletPrefab();
        if (bulletPrefab != null)
        {
            for (int i = 0; i < gunData.shotPerMinute + 1; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.SetActive(false);
                bulletPool.Add(bullet);
            }
            loader.ReloadGun(this);
        }
        else
            Debug.Log("Out of Ammo");
    }

    public void AimGunAtMouse(Vector3 posToLookTo)
    {
        gunRotation.GunLookAt(posToLookTo);
    }

    public void FindGunlEnd()
    {
        gunEnd = transform.Find("Gun End").gameObject;
    }

    public void FireGun(bool isPullingTheTrigger)
    {
        if (isPullingTheTrigger)
        {
            Debug.Log("gun");
            if (hasAmmo)
            {
                GameObject bullet = GetBulletFromPool();
                if (bullet != null)
                {
                    bullet.transform.position = gunEnd.transform.position;
                    bullet.transform.rotation = gunEnd.transform.rotation;

                    if (!gunData.isFullAuto)
                    {
                        FireSemiAuto(bullet);
                    }
                    else
                    {
                        if(!isDelaying) 
                            StartCoroutine(FireFullAuto(bullet));
                    }

                    
                }
            }
            if (currentAmmoCount == 0 && !isReloading)
            {
                hasAmmo = false;
                loader.ReloadGun(this);
            }
        }
    }

    private GameObject GetBulletFromPool()
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        return null; // If all bullets are currently in use, return null
    }

    private void FireSemiAuto(GameObject bullet)
    {
        if (hasAmmo)
        {
            bullet.GetComponent<BulletAndShellBehavior>().Fire();
            currentAmmoCount--;
        }
    }

    private IEnumerator FireFullAuto(GameObject bullet)
    {
        // Fire a bullet
        if (isDelaying)
        { yield break; }

        bullet.GetComponent<BulletAndShellBehavior>().Fire();
        currentAmmoCount--;
        isDelaying = true;
        // Calculate the delay between shots based on shotPerMinute
        float delayBetweenShots = 60f / gunData.shotPerMinute;
        yield return new WaitForSeconds(delayBetweenShots);
        isDelaying = false;

    }


    public IEnumerator Reload()
    {
        // Check if the gun is already reloading
        if (isReloading)
        {
            yield break; // Exit the coroutine if already reloading
        }

        // Set reloading flag to true
        isReloading = true;

        // Perform the reload process
        yield return new WaitForSeconds(gunData.reloadTime);

        // Reset ammo count and set hasAmmo flag to true
        currentAmmoCount = gunData.ammoCapacity;
        hasAmmo = true;

        // Reset the reloading flag
        isReloading = false;
    }
}
