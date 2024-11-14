using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    Collider2D armorCollider2D;
    [SerializeField]
    int thickness;

    public bool isBeingHit;
    bool isPenetrated;

    public bool IsBeingHit { get => isBeingHit; set => isBeingHit = value; }

    private void Start()
    {
       armorCollider2D = GetComponent<Collider2D>();
    }

    public bool CheckForPenetration(BulletBehavior bullet)
    {
        Debug.Log(gameObject.name);
        if (bullet.GetAmmunitionData().penetrationPower > thickness)
        {
            Debug.Log("Pen");
            return true;
        }
        else
        {
            bullet.DeactivateBullet();
            Debug.Log("NonPen");
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Projectile")
        {
            Debug.Log("Projectile hit armor!");
            isBeingHit = true;
        }
    }
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Projectile"))
    //    {
    //        Debug.Log("Projectile left the armor!");
    //        isBeingHit = false;
    //    }
    //}

}

