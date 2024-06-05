using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Gun", order = 1)]
public class GunData : Item
{
    public AmmunitionData ammunitionData;
    public new string name; // You can use the 'name' field from ScriptableObject
    public bool isFullAuto;
    public int ammoCapacity = 1;
    public float reloadTime = 2;
    public int shotPerMinute;
    public float maxRotationAngle = 30f;
    public float rotationSpeed = 5f;
    public Sprite gunSprite; //  Sprite for the gun
    public Sprite turretSprite; // Sprite for the Turret
    // Add other parameters as needed
}
