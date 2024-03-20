using System.Collections.Generic;
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
        foreach (var gun in guns)
        {
            if (!gun.isAIControlled)
            {
                gun.AimGunAtMouse(mousePosition);
                gun.FireGun(isPullingTheTrigger);
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
