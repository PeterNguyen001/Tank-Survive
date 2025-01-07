using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehaviour : TankPart
{
    public GunData gunData;
    private GameObject gunEnd;

    private AmmoLoader loader;
    private LinkedList<GameObject> bulletPool = new LinkedList<GameObject>();
    [SerializeField]
    private int currentAmmoCount;

    private bool hasAmmo = false;
    private bool isDelaying = false;
    private bool isReloading = false;


    private void Start()
    {
        //Init();
    }

    public override void Init()
    {
        tankPart = gunData;
        loader = FindObjectOfType<AmmoLoader>();
        FindGunlEnd();
        loader.ReloadGun(this);
    }

    public void InitializeBulletPool(GameObject prefab, LinkedList<Collider2D> ListOfOwnTankCollider)
    {
        
        if (prefab != null)
        {
            
            for (int i = 0; i <= gunData.shotPerMinute; i++)
            {
                GameObject bulletClone = Instantiate(prefab);
                BulletBehavior bulletBehavior = bulletClone.GetComponent<BulletBehavior>();
                bulletBehavior.SetupBullet(ListOfOwnTankCollider);
                bulletPool.AddLast(bulletClone);
            }
        }
        else
            Debug.Log("Out of Ammo");
        
    }


    public GameObject FindGunlEnd()
    {
        gunEnd = transform.Find("Gun End").gameObject;
        return gunEnd;
    }

    public void FireGun(bool isPullingTheTrigger)
    {
        if (isPullingTheTrigger)
        {
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
            bullet.GetComponent<BulletBehavior>().Fire(ownerObject);
            currentAmmoCount--;
        }
    }

    private IEnumerator FireFullAuto(GameObject bullet)
    {
        // Fire a bulletClone
        if (isDelaying)
        { yield break; }

        bullet.GetComponent<BulletBehavior>().Fire(ownerObject);
        currentAmmoCount--;
        isDelaying = true;
        // Calculate the delay between shots based on shotPerMinute
        float delayBetweenShots = 60f / gunData.shotPerMinute;
        yield return new WaitForSeconds(delayBetweenShots);
        isDelaying = false;

    }

    public IEnumerator Reload()
    {
        //Debug.Log("reloading");
        // Check if the gun is already reloading
        if (isReloading)
        {
            yield break; // Exit the coroutine if already reloading
        }

        // Set reloading flag to true
        isReloading = true;

        // Perform the reload process
        yield return new WaitForSeconds(gunData.reloadTime);

        // Reset ammoData count and set hasAmmo flag to true
        currentAmmoCount = gunData.ammoCapacity;
        hasAmmo = true;

        // Reset the reloading flag
        isReloading = false;
        //Debug.Log("reloaded");
    }

}
