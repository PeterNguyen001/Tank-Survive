using UnityEngine;

public class GunRotation
{
    private float maxRotationAngle = 30f;
    private float rotationSpeed = 5f;

    private float localInitialAngle;

    private Transform guntransform;

    public GunRotation(Gun gun)
    { 
        this.guntransform = gun.transform;
        maxRotationAngle = gun.gunData.maxRotationAngle;
        rotationSpeed = gun.gunData.rotationSpeed;
    }
    public void Start()
    {

        // Calculate the local initial angle relative to the turret's rotation
        localInitialAngle = guntransform.localEulerAngles.z;
    }

    float GetTankAngle()
    {
        // Get the tank rotation in the [0, 360) range
        float tankRotation = guntransform.parent.eulerAngles.z % 360f;

        return tankRotation;
    }

    void Update()
    {
        // Get the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // Calculate the direction from the turret to the mouse
        GunLookAt(mousePosition);
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

    void OnDrawGizmos()
    {
        // Draw a line in the Scene view to visualize the direction the turret is looking at
        Gizmos.color = Color.red;

        // Calculate the length of the line based on the distance to the mouse position
        float lineLength = 10f; // Adjust the line length as needed

        Gizmos.DrawLine(guntransform.position, guntransform.position + guntransform.right * lineLength);
    }
}
