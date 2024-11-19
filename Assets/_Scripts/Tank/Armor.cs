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
    private TankPart partAttachedTo;

    public bool IsBeingHit { get => isBeingHit; set => isBeingHit = value; }
    public TankPart TankPartAttachedTo { get => partAttachedTo; set => partAttachedTo = value; }

    private void Start()
    {
       armorCollider2D = GetComponent<Collider2D>();

    }

    // Method to generate normals along the edges of the polygon collider
  
    public bool CheckForPenetration(BulletBehavior bullet)
    {
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            Debug.Log("Projectile left the armor!");
            isBeingHit = false;
        }
    }

}

