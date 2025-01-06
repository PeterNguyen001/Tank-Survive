using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoLoader : TankSubComponent
{
    public GameObject ammoPefabContainer;
    private List<BulletBehavior> ammoPrefabList = new List<BulletBehavior>();
    [SerializeField]
    public List<AmmoContainer> m_AmmoContainers = new List<AmmoContainer>();
    public LinkedList<GunBehaviour> gunList;
    public GameObject bulletPrefab1;
    public GameObject bulletPrefab2;
    // Start is called before the first frame update

    private void Start()
    {
        if (tag != "Enemy")
            Tools.FindComponentsRecursively(ammoPefabContainer.transform,ammoPrefabList, true);
    }
    public override void Init()
    {
        if (tag == "Enemy")
        {
            m_AmmoContainers.Add(new AmmoContainer(bulletPrefab1, 20));
            m_AmmoContainers.Add(new AmmoContainer(bulletPrefab2, 6));
        }
        gunList = tankPartManager.GetListOfGun();
        foreach (GunBehaviour gun in gunList)
        {
            AmmoContainer correctAmmunitionType = FindCorrectAmmunitionType(gun.gunData.ammunitionData);
            if (correctAmmunitionType != null)
            {
                GameObject bulletPrefab = correctAmmunitionType.GetbulletPrefab();

                gun.InitializeBulletPool(bulletPrefab, tankPartManager.GetListOfCollider2D());
            }
        }
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
        foreach (AmmoContainer item in m_AmmoContainers)
        {
            if (item.GetAmmunitionType() == ammunitionData)
                return item;
        }
        return null;
    }

    public void AddAmmos(List<ItemSlot> itemSlots)
    {
        foreach (ItemSlot item in itemSlots)
        {
            if (item.Count != 0 && item.GetItem() is AmmunitionData ammunitionData)
            {
                bool foudExistingAmmoType = false;
                if (m_AmmoContainers.Count != 0 )
                {
                    foreach (AmmoContainer ammo in m_AmmoContainers)
                    {
                        if (ammo.GetAmmunitionType().name == ammunitionData.name)
                        {
                            ammo.AddAmmunitionByCount(item.Count);
                            foudExistingAmmoType = true;
                        }

                    }
                }
                else if (!foudExistingAmmoType)
                {
                    foreach (BulletBehavior bulletBehavior in ammoPrefabList)
                    {
                        if(bulletBehavior.GetAmmunitionData().name == ammunitionData.name)
                        {
                            m_AmmoContainers.Add(new AmmoContainer(bulletBehavior.gameObject, item.Count));
                            break;
                        }
                    }
                }
            }

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
    { 
        return bulletPrefab.GetComponent<BulletBehavior>().GetAmmunitionData(); 
    }

    public GameObject GetbulletPrefab()
    {
        return bulletPrefab; 
    }
    public int GetAmmunitionCount()
    { return ammunitionCount; }
    public void RemoveOneAmmunitionCount()
    { ammunitionCount--; }
    public void AddAmmunitionByCount(int count)
    {
        ammunitionCount += count;
    }
    public bool IsEmpty()
    {
        return ammunitionCount == 0;
    }
}
