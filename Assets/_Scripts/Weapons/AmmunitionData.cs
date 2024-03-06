using UnityEngine;

[CreateAssetMenu(fileName = "NewAmmunition", menuName = "Ammunition", order = 1)]
public class AmmunitionData : ScriptableObject
{
    public new string name; // You can use the 'name' field from ScriptableObject
    public int damage = 10;
    public float velocity = 200;
    public Sprite ammoSprite; // Assuming you want to store a sprite for the gun
    // Add other parameters as needed
}