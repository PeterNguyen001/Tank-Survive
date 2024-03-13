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
    private int currentAmmoCount;

    private bool canFire = true;

    private void Start()
    {
        loader = FindObjectOfType<AmmoLoader>();
        gunRotation = new GunRotation(this);
        FindGunlEnd();

        // Initialize the bullet pool
        bulletPrefab = loader.FindCorrectAmmunitionType(gunData.ammunitionData);
        InitializeBulletPool();
    }

    private void InitializeBulletPool()
    {
        if (bulletPrefab != null)
            for (int i = 0; i < gunData.shotPerMinute + 1; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.SetActive(false);
                bulletPool.Add(bullet);
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

    public void FireGun(bool isFiring)
    {
        if (isFiring)
        {
            if (canFire)
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
                        StartCoroutine(FireFullAuto(bullet));
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
    }

    private GameObject GetBulletFromPool()
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }

        return null; // If all bullets are currently in use, return null
    }

    public void FireSemiAuto(GameObject bullet)
    {
        if (canFire)
        {
            bullet.GetComponent<BulletAndShellBehavior>().Fire();
        }
    }

    private IEnumerator FireFullAuto(GameObject bullet)
    {
        while (canFire)
        {
            bullet.GetComponent<BulletAndShellBehavior>().Fire();

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
