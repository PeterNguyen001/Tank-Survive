using UnityEngine;

public class GunRotation
{
    public float maxRotationAngle = 30f;
    public float rotationSpeed = 5f;

    public float localInitialAngle;

    public Transform guntransform;

    public GunRotation(GunBehaviour gun)
    {
        this.guntransform = gun.transform.parent;
        maxRotationAngle = gun.turretAndPortData.maxRotationAngle;
        rotationSpeed = gun.turretAndPortData.rotationSpeed;
        // Calculate the local initial angle relative to the turret's rotation
        localInitialAngle = guntransform.localEulerAngles.z;
    }

    float GetTankAngle()
    {
        // Get the tank rotation in the [0, 360) range
        float tankRotation = guntransform.parent.eulerAngles.z % 360f;

        return tankRotation;
    }

    public void GunLookAt(Vector3 positionToLookAt)
    {
        // Calculate the direction to the mouse
        Vector3 direction = positionToLookAt - guntransform.position;

        // Calculate the angle between the current turret rotation and the direction to the mouse
        float angleToMouse = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Adjust the initial angle based on the tank/hull rotation
        float adjustedInitialAngle = GetTankAngle() + localInitialAngle;

        // Calculate the difference between the angles using DeltaAngle
        float angleDifference = Mathf.DeltaAngle(adjustedInitialAngle, angleToMouse);

        // Clamp the angle to stay within the max rotation angle limits
        angleDifference = Mathf.Clamp(angleDifference, -maxRotationAngle, maxRotationAngle);

        // Smoothly rotate the turret towards the calculated angle using rotationSpeed
        float targetAngle = adjustedInitialAngle + angleDifference;
        Quaternion desiredRotation = Quaternion.Euler(0f, 0f, targetAngle);
        guntransform.rotation = Quaternion.RotateTowards(guntransform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }
}
