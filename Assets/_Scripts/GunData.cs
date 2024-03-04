using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Gun", order = 1)]
public class GunData : ScriptableObject
{
    public new string name; // You can use the 'name' field from ScriptableObject
    public int damage = 10;
    public float maxRotationAngle = 30f;
    public float rotationSpeed = 5f;
    public Sprite gunSprite; // Assuming you want to store a sprite for the gun
    // Add other parameters as needed
}
