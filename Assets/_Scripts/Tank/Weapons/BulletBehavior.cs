using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public AmmunitionData ammo;
    public float lifespan = 2.0f; // Adjust the lifespan as needed
    public float timer;
    private Rigidbody2D bulletRb;
    private LinkedList<Collider2D> ignoreColliders = new LinkedList<Collider2D>();

    // Serialize this field to ensure it shows in the Inspector for debugging purposes
    [SerializeField]
    private TankStatus tankStatus;

    // Start is called before the first frame update
    void Start()
    {
        bulletRb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (gameObject.activeSelf)
        {
            bulletRb.AddForce(transform.right * ammo.velocity);

            // Update the timer
            timer += Time.fixedDeltaTime;

            // Check if the bullet has exceeded its lifespan
            if (timer >= lifespan)
            {
                DeactivateBullet();
            }
        }
    }

    public void Fire()
    {

        gameObject.SetActive(true);
    }

    // Deactivate the bullet and reset the timer
    private void DeactivateBullet()
    {
        gameObject.SetActive(false);
        timer = 0f; // Reset the timer when firing
    }

    public AmmunitionData GetAmmunitionData()
    {
        return ammo;
    }

    public void SetTankStatus(TankStatus status)
    {
        tankStatus = status;

        
    }

    // Calculate the trajectory and return a list of possible collisions
    public List<RaycastHit2D> CalculateTrajectory(float length)
    {


        List<RaycastHit2D> collisions = new List<RaycastHit2D>();

        Vector2 position = transform.position;
        Vector2 direction = transform.right;

        // Perform a raycast
        RaycastHit2D[] hits = Physics2D.RaycastAll(position, direction, length);

        // Draw the ray in the Scene view
        Debug.DrawRay(position, direction * length, Color.red, 1f);

        // Process each hit
        foreach (RaycastHit2D hit in hits)
        {
            BulletBehavior bulletBehavior = hit.collider.gameObject.GetComponent<BulletBehavior>();
            if (hit.collider != null && !tankStatus.GetListOfCollider2D().Contains(hit.collider) && bulletBehavior == null)
            {
                Debug.Log(hit.collider.gameObject);
                collisions.Add(hit);
                // Draw the hit point
                Debug.DrawLine(position, hit.point, Color.green, 1f);
            }
        }

        return collisions;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        CalculateTrajectory(10);
    }
}
