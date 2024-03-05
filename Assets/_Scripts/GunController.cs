using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    private LinkedList<Gun> guns = new LinkedList<Gun>();

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
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        // Update each GunRotation
        foreach (var gun in guns)
        {
            gun.AimGunAtMouse(mousePosition);
        }
    }

    // OnDrawGizmos is called in the editor to visualize the gun directions
    void OnDrawGizmos()
    {
        // Draw Gizmos for each GunRotation

    }
}
