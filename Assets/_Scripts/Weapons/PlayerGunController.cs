using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGunController : MonoBehaviour
{

    private LinkedList<Gun> guns = new LinkedList<Gun>();
    private Vector3 mousePosition;
    private bool isPullingTheTrigger;

    public float detectionRange = 10f;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform turretAndGunPort in transform)
        {
            foreach (Transform gunTransfrom in turretAndGunPort)
            {
                Gun gun = gunTransfrom.GetComponent<Gun>();
                guns.AddLast(gun);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Update each GunRotation
        foreach (Gun gun in guns)
        {
            if (!gun.isAIControlled)
            {
                gun.AimGunAt(mousePosition);
                gun.FireGun(isPullingTheTrigger);
            }
            else
            {
                if(DetectNearestEnemyFromGun(gun) != null)
                {
                    Vector3 enemyPosition = DetectNearestEnemyFromGun(gun).transform.position;
                    gun.AimGunAt(enemyPosition);
                    gun.FireGun(IsEnemyOnGunSight(gun));
                }
                    
            }
        }
    }

    public void FireGun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            isPullingTheTrigger = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isPullingTheTrigger = false;
        }
    }

    public void MoveMouse(InputAction.CallbackContext context)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
        mousePosition.z = 0f;
    }

    public GameObject DetectNearestEnemyFromGun(Gun gun)
    {
        GameObject nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        Vector3 gunPosition = gun.transform.position;
        float coneAngle = gun.gunData.maxRotationAngle * 2f; // Double the angle for the cone

        // Calculate the cone direction based on the gun's rotation
        Vector3 coneDirection = Quaternion.Euler(0, 0, gun.GetGunLocalInitialAngle() + transform.eulerAngles.z % 360f) * Vector3.right;

        // Check for enemy objects within the cone area
        foreach (Vector2 vertex in BuildDetectionCone(gunPosition, coneDirection, coneAngle))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(vertex, detectionRange);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    GameObject enemy = collider.gameObject;
                    // Check if the enemy is within the cone angle
                    Vector2 toEnemy = enemy.transform.position - gunPosition;
                    float angleToEnemy = Vector2.Angle(coneDirection, toEnemy);
                    if (Mathf.Abs(angleToEnemy) <= gun.gunData.maxRotationAngle)
                    {
                        // Calculate the distance to the enemy
                        float distanceToEnemy = Vector2.Distance(gunPosition, enemy.transform.position);

                        // Update nearest enemy if this enemy is closer
                        if (distanceToEnemy < nearestDistance)
                        {
                            nearestEnemy = enemy;
                            nearestDistance = distanceToEnemy;
                        }
                    }
                }
            }
        }

        return nearestEnemy;
    }

    private List<Vector2> BuildDetectionCone(Vector3 gunPosition, Vector3 coneDirection, float coneAngle)
    {
        List<Vector2> coneVertices = new List<Vector2>();
        coneVertices.Add(gunPosition);
        for (int i = -1; i <= 1; i += 2)
        {
            float angle = coneAngle * 0.5f * i;
            Vector3 vertexDirection = Quaternion.Euler(0, 0, angle) * coneDirection;
            Vector2 vertex = gunPosition + vertexDirection * detectionRange;
            coneVertices.Add(vertex);
        }
        return coneVertices;
    }

    private bool IsEnemyOnGunSight(Gun gun)
    {
        // Get the position and rotation of the gunEnd
        Vector3 gunEndPosition = gun.FindGunlEnd().transform.position;
        Quaternion gunRotation = gun.transform.rotation;

        // Calculate the direction from the right side of the gunEnd
        Vector3 rayDirection = gunRotation * Vector3.right;

        // Cast a ray in the direction of the gun's rotation with the detection range
        RaycastHit2D hit = Physics2D.Raycast(gunEndPosition, rayDirection, detectionRange);

        // Check if the ray hits an enemy object
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            return true;
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var gun in guns)
        {
            if (gun.gunData != null)
            {
                // Calculate the cone direction based on the maximum rotation angle
                Vector3 coneDirection = Quaternion.Euler(0, 0, gun.GetGunLocalInitialAngle() +transform.eulerAngles.z % 360f) * Vector3.right;

                // Draw the detection cone
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(gun.transform.position, coneDirection * detectionRange);
                Gizmos.DrawRay(gun.transform.position, Quaternion.Euler(0, 0, gun.gunData.maxRotationAngle) * coneDirection * detectionRange);
                Gizmos.DrawRay(gun.transform.position, Quaternion.Euler(0, 0, -gun.gunData.maxRotationAngle) * coneDirection * detectionRange);

                Vector3 gunEndPosition = gun.FindGunlEnd().transform.position;
                Quaternion gunRotation = gun.transform.rotation;

                // Calculate the direction from the right side of the gunEnd
                Vector3 lineDirection = gunRotation * Vector3.right;

                // Draw the line from the gunEnd
                Gizmos.color = Color.green;
                Gizmos.DrawRay(gunEndPosition, lineDirection * detectionRange);
            }
        }
    }
}
