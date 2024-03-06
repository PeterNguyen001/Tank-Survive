using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{

    private LinkedList<Gun> guns = new LinkedList<Gun>();
    private Vector3 mousePosition;
    private bool isFiring;

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
        //mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mousePosition.z = 0f;
        // Update each GunRotation
        foreach (var gun in guns)
        {
            gun.AimGunAtMouse(mousePosition);
            gun.FireGun(isFiring);
        }
    }

    public void FireGun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            isFiring = true;
        else
            isFiring = false;
    }

    public void MoveMouse(InputAction.CallbackContext context)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
        mousePosition.z = 0f;
    }
}
