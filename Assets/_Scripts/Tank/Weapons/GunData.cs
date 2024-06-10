using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Tank Parts/Gun", order = 1)]
public class GunData : TankPartData
{
    public AmmunitionData ammunitionData;
     
    public bool isFullAuto;
    public int ammoCapacity = 1;
    public float reloadTime = 2;
    public int shotPerMinute;
    public Sprite gunSprite; //  Sprite for the gun
    public Sprite turretSprite; // Sprite for the Turret
    // Add other parameters as needed
}
