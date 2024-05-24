using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTurretAndGunPort", menuName = "TurretAndGunPort", order = 1)]
public class TurretAndGunPortData : Item
{
    public bool isTurret = true;
    public float maxRotationAngle = 30f;
    public float rotationSpeed = 5f;

    // Add other parameters as needed
}
