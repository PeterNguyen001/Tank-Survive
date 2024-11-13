using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    Collider2D armorCollider2D;
    [SerializeField]
    int thickness;

    bool isBeingHit;
    bool isPenetrated;

    public bool IsBeingHit { get => isBeingHit; set => isBeingHit = value; }

    private void Start()
    {
       armorCollider2D = GetComponent<Collider2D>();
    }

    public bool CheckForPenetration(AmmunitionData ammunitionData)
    {
        if(ammunitionData.penetrationPower > thickness)
            return true;
        else return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.tag == "Projectile")
        {
            Debug.Log("A");
            isBeingHit = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            Debug.Log("Projectile left the armor!");
            isBeingHit = false;
        }
    }

}

