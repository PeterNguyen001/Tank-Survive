using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    private LinkedList<GunRotation> gunRotations = new LinkedList<GunRotation>();
    private LinkedList<Gun> guns = new LinkedList<Gun>();

    // Start is called before the first frame update
    void Start()
    {
        // Iterate through all child objects
        foreach (Transform child in transform)
        {
            // Check if the child has a GameObject and a GunRotation script
            Gun gun = child.gameObject.GetComponent<Gun>();
            GunRotation gunRotation = new GunRotation(gun);

            // If both are present, add them to the lists
            if (gun != null && gunRotation != null)
            {
                guns.AddLast(gun);
                gunRotations.AddLast(gunRotation);

                // Initialize the GunRotation
                gunRotation.Start();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        // Update each GunRotation
        foreach (var gunRotation in gunRotations)
        {
            gunRotation.GunLookAt(mousePosition);
        }
    }

    // OnDrawGizmos is called in the editor to visualize the gun directions
    void OnDrawGizmos()
    {
        // Draw Gizmos for each GunRotation
        foreach (var gunRotation in gunRotations)
        {
            //gunRotation.OnDrawGizmos();
        }
    }
}
