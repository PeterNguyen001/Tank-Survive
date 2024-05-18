using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankStatus : MonoBehaviour
{
    // Start is called before the first frame update
    private static TankStatus _instance;
    public static TankStatus Instance { get { return _instance; } }
    private LinkedList<Gun> gunList = new LinkedList<Gun>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public LinkedList<Gun> GetListOfGun()
    {
        gunList.Clear();
        FindGunsRecursively(transform);
        return gunList;
    }
    private void FindGunsRecursively(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Gun gun = child.GetComponent<Gun>();
            if (gun != null)
            {
                gunList.AddLast(gun);
            }

            // Recursively search in the children of the current child
            FindGunsRecursively(child);
        }
    }
    //public float GetSpeedKMH()
    //{
    //    // Calculate the speed in meters per second (m/s)
    //    float speedMS = ;

    //    // Convert speed from meters per second (m/s) to kilometers per hour (km/h)
    //    float speedKMH = speedMS * 3.6f; // 1 m/s = 3.6 km/h

    //    return speedKMH;
    //}

}
