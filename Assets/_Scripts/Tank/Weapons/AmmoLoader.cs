using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoLoader : TankSubComponent
{
    public List<AmmoContainer> m_AmmoContainers = new List<AmmoContainer>();
    public LinkedList<GunBehaviour> gunList;
    public GameObject bulletPrefab1;
    public GameObject bulletPrefab2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void  ReloadGun(GunBehaviour gun)
    {

        AmmoContainer containerToTakeFrom = FindCorrectAmmunitionType(gun.gunData.ammunitionData);
        if (containerToTakeFrom != null) 
        {
            if(!containerToTakeFrom.IsEmpty()) 
            {
                containerToTakeFrom.RemoveOneAmmunitionCount();
                StartCoroutine(gun.Reload());
                
            }
            else if (containerToTakeFrom.IsEmpty())
            {
                m_AmmoContainers.Remove(containerToTakeFrom);
            }
        }
    }

    public AmmoContainer FindCorrectAmmunitionType( AmmunitionData ammunitionData)
    {
        foreach (var item in m_AmmoContainers)
        {
            if (item.GetAmmunitionType() == ammunitionData)
                return item;
        }
        return null;
    }

    public override void Init()
    {
        m_AmmoContainers.Add(new AmmoContainer(bulletPrefab1, 20));
        m_AmmoContainers.Add(new AmmoContainer(bulletPrefab2, 6));
        gunList = tankPartManager.GetListOfGun();
        foreach (GunBehaviour gun in gunList)
        {
            AmmoContainer correctAmmunitionType = FindCorrectAmmunitionType(gun.gunData.ammunitionData);
            GameObject bulletPrefab = correctAmmunitionType.GetbulletPrefab();

            gun.InitializeBulletPool(bulletPrefab, tankPartManager.GetListOfCollider2D());

        }
    }
}

public class AmmoContainer 
{
    // Start is called before the first frame update
    GameObject bulletPrefab;
    int ammunitionCount;
    public AmmoContainer(GameObject bulletPrefab, int ammunitionCount)
    {
        this.bulletPrefab = bulletPrefab;
        this.ammunitionCount = ammunitionCount;
    }   
    public AmmunitionData GetAmmunitionType()
    { return bulletPrefab.GetComponent<BulletBehavior>().GetAmmunitionData(); }

    public GameObject GetbulletPrefab()
    {
        return bulletPrefab; 
    }
    public int GetAmmunitionCount()
    { return ammunitionCount; }
    public void RemoveOneAmmunitionCount()
    { ammunitionCount--; }
    public bool IsEmpty()
    {
        return ammunitionCount == 0;
    }
}
