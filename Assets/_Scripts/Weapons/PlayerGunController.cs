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
        foreach (Transform child in transform)
        {
            Gun gun = child.gameObject.GetComponent<Gun>();

            if (gun != null)
            {
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
                if(DetectEnemy(gun) != null)
                {
                    gun.AimGunAt(DetectEnemy(gun).transform.position);
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

    public GameObject DetectEnemy(Gun gun)
    {
        Vector3 gunPosition = gun.transform.position;
        float coneAngle = gun.gunData.maxRotationAngle * 2f; // Double the angle for the cone

        // Calculate the cone direction based on the gun's rotation
        Vector3 coneDirection = Quaternion.Euler(0, 0, gun.GetLocalInitialAngle() + transform.eulerAngles.z % 360f) * Vector3.right;

        // Calculate the cone vertices
        List<Vector2> coneVertices = new List<Vector2>();
        coneVertices.Add(gunPosition);
        for (int i = -1; i <= 1; i += 2)
        {
            float angle = coneAngle * 0.5f * i;
            Vector3 vertexDirection = Quaternion.Euler(0, 0, angle) * coneDirection;
            Vector2 vertex = gunPosition + vertexDirection * detectionRange;
            coneVertices.Add(vertex);
        }

        // Check for enemy objects within the cone area
        foreach (Vector2 vertex in coneVertices)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(vertex, detectionRange);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    // Check if the enemy is within the cone angle
                    Vector2 toEnemy = collider.transform.position - gunPosition;
                    float angleToEnemy = Vector2.Angle(coneDirection, toEnemy);
                    if (Mathf.Abs(angleToEnemy) <= gun.gunData.maxRotationAngle)
                    {
                        Debug.Log("Enemy spotted");
                        return collider.gameObject;
                    }
                }
            }
        }

        return null; // Return null if no enemy is detected within the cone
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var gun in guns)
        {
            if (gun.gunData != null)
            {
                // Calculate the cone direction based on the maximum rotation angle
                Vector3 coneDirection = Quaternion.Euler(0, 0, gun.GetLocalInitialAngle() +transform.eulerAngles.z % 360f) * Vector3.right;

                // Draw the detection cone
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(gun.transform.position, coneDirection * detectionRange);
                Gizmos.DrawRay(gun.transform.position, Quaternion.Euler(0, 0, gun.gunData.maxRotationAngle) * coneDirection * detectionRange);
                Gizmos.DrawRay(gun.transform.position, Quaternion.Euler(0, 0, -gun.gunData.maxRotationAngle) * coneDirection * detectionRange);
            }
        }
    }
}
