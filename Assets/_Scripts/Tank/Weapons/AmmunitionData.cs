using UnityEngine;

[CreateAssetMenu(fileName = "NewAmmunition", menuName = "Ammunition", order = 1)]
public class AmmunitionData : Item
{
    public new string name; // You can use the 'name' field from ScriptableObject
    public int damage = 10;
    public float velocity = 200;
    public Sprite ammoSprite; // Assuming you want to store a sprite for the gun
                              // Add other parameters as needed


    public static bool operator ==(AmmunitionData a, AmmunitionData b)
    {
        // Check if both references are null or if they point to the same object
        if (ReferenceEquals(a, b))
            return true;

        // Check if either reference is null
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        // Compare only the 'name' field for equality
        return a.name == b.name;
    }

    // Override the != operator by negating the result of ==
    public static bool operator !=(AmmunitionData a, AmmunitionData b)
    {
        return !(a == b);
    }

    // Override the Equals method
    public override bool Equals(object obj)
    {
        if (obj is AmmunitionData other)
        {
            return this == other;
        }

        return false;
    }

    // Override the GetHashCode method
    public override int GetHashCode()
    {
        // Use the hash code of the 'name' field
        return name != null ? name.GetHashCode() : 0;
    }

}