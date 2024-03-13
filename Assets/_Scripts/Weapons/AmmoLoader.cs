using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoLoader : MonoBehaviour
{
    public List<AmmoContainer> m_AmmoContainers = new List<AmmoContainer>();
    public GameObject bulletPrefab1;
    public GameObject bulletPrefab2;
    // Start is called before the first frame update
    void Start()
    {
        m_AmmoContainers.Add(new AmmoContainer(bulletPrefab1, 5));
        m_AmmoContainers.Add(new AmmoContainer(bulletPrefab2, 2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void  ReloadGun(Gun gun)
    {

    }

    public GameObject FindCorrectAmmunitionType( AmmunitionData ammunitionData)
    {
        foreach (var item in m_AmmoContainers)
        {
            if (item.GetAmmunitionType().name == ammunitionData.name)
                return item.GetbulletPrefab();
        }
        return null;
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
        return bulletPrefab.GetComponent<BulletAndShellBehavior>().GetAmmunitionData();
    }

    public GameObject GetbulletPrefab()
    { return bulletPrefab; }
}
