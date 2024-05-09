using UnityEngine;

[CreateAssetMenu(fileName = "NewTurretAndGunPort", menuName = "TurretAndGunPort", order = 1)]
public class TurretAndGunPortData : ScriptableObject
{
    public AmmunitionData ammunitionData;
    public new string name; // You can use the 'name' field from ScriptableObject
    public bool hasTurret;
    public float maxRotationAngle = 30f;
    public float rotationSpeed = 5f;
    public Sprite turretSprite; // Sprite for the Turret
    // Add other parameters as needed
}
