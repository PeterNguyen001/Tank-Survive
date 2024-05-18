using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoLoader : MonoBehaviour
{
    public List<AmmoContainer> m_AmmoContainers = new List<AmmoContainer>();
    public LinkedList<Gun> gunList = new LinkedList<Gun>();
    public GameObject bulletPrefab1;
    public GameObject bulletPrefab2;
    // Start is called before the first frame update
    void Start()
    {
        m_AmmoContainers.Add(new AmmoContainer(bulletPrefab1, 20));
        m_AmmoContainers.Add(new AmmoContainer(bulletPrefab2, 6));
        SetupGuns();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void  ReloadGun(Gun gun)
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

   public void SetupGuns()
    {
        gunList = TankStatus.Instance.GetListOfGun();
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
    { return bulletPrefab.GetComponent<BulletAndShellBehavior>().GetAmmunitionData(); }

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
