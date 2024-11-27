using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    Collider2D armorCollider2D;
    [SerializeField]
    int thickness; // Base armor thickness in millimeters

    public bool isBeingHit;
    bool isPenetrated;
    private TankPart partAttachedTo;

    public bool IsBeingHit { get => isBeingHit; set => isBeingHit = value; }
    public TankPart TankPartAttachedTo { get => partAttachedTo; set => partAttachedTo = value; }

    private void Start()
    {
        armorCollider2D = GetComponent<Collider2D>();
    }

    /// <summary>
    /// Calculates the effective armor thickness based on the angle of incidence.
    /// </summary>
    /// <param name="hitAngle">The angle between the bullet's direction and the armor's surface normal in degrees.</param>
    /// <returns>The effective armor thickness.</returns>
    public float CalculateEffectiveArmor(float hitAngle)
    {
        // Convert angle to radians for the cosine calculation
        float angleInRadians = Mathf.Deg2Rad * hitAngle;

        // Effective thickness formula: baseThickness / cos(angle)
        float effectiveThickness = thickness / Mathf.Cos(angleInRadians);

        // Prevent extreme values for grazing angles
        if (float.IsInfinity(effectiveThickness) || effectiveThickness > thickness * 10)
        {
            effectiveThickness = thickness * 10;
        }

        Debug.Log($"Effective armor thickness: {effectiveThickness} (angle: {hitAngle} degrees)");
        return effectiveThickness;
    }

    public bool CheckForPenetration(BulletBehavior bullet)
    {
        float hitAngle = bullet.CastRayConeAndCalculateAverageHitAngle(this);
        float effectiveThickness = CalculateEffectiveArmor(hitAngle);

        if (bullet.PenetrationPower > effectiveThickness)
        {
            bullet.RemovePenetratedPower(Convert.ToInt16(effectiveThickness));
            bullet.AddArmorToPenetratedList(this);
            Debug.Log("Penetrated!");
            return true;
        }
        else
        {
            bullet.DeactivateBullet();
            Debug.Log("Non-Penetration!");
            return false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            isBeingHit = false;
        }
    }
}
