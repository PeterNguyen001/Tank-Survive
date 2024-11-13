using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPart : MonoBehaviour
{
    protected TankPartData tankPart;
    protected TankPartManager tankPartManager;
    private bool isDisable;
    private SpriteRenderer spriteRenderer;

    protected Armor[] armorList;

    [SerializeField]
    public float HP;
    public float maxHP = 10; // Maximum HP to calculate color intensity

    public bool IsDisable { get => isDisable; set => isDisable = value; }

    // Start is called before the first frame update
    void Awake()
    {
        Init();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get SpriteRenderer component
        HP = maxHP; // Initialize HP to max
        UpdateSpriteColor(); // Initial color setup
    }

    public void SetManager(TankPartManager manager)
    {
        tankPartManager = manager;
    }

    public virtual void Init()
    {
        // Initialization logic for TankPart can go here
    }

    // Called whenever the tank part takes damage
    public void TakeHit(BulletBehavior bullet)
    {
        bool isPenetraded;
        float damage = bullet.GetAmmunitionData().damage;
        foreach (Armor armor in armorList)
        {
            isPenetraded = armor.CheckForPenetration(bullet.GetAmmunitionData());
            if (isPenetraded)
            {
                HP -= damage;
                if (HP <= 0)
                {
                    UpdateSpriteColor();
                    HP = 0;
                    IsDisable = true;
                }
                break;
            }
        }
      


    }

    // Update the sprite color to reflect damage level
    private void UpdateSpriteColor()
    {
        if (spriteRenderer != null)
        {
            float colorIntensity = HP / maxHP; // Calculate color intensity (1 for full HP, 0 for zero HP)
            spriteRenderer.color = new Color(colorIntensity, colorIntensity, colorIntensity); // Apply grayscale color
        }
    }

    public TankPartData GetTankPart() { return tankPart; }
}
