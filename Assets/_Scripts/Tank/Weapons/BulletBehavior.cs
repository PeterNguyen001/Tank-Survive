using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public AmmunitionData ammo;
    public float lifespan = 2.0f; // Adjust the lifespan as needed
    public float timer;
    private Rigidbody2D bulletRb;

    [SerializeField]
    private LinkedList<Collider2D> ignoreColliders = new LinkedList<Collider2D>();

    private LinkedList<RaycastHit2D> collisions = new LinkedList<RaycastHit2D>();
    private HashSet<Collider2D> existingColliders = new HashSet<Collider2D>();

    private bool hasTarget;
    private bool isMissed;
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
    public void DeactivateBullet()
    {
        gameObject.SetActive(false);
        collisions.Clear();
        existingColliders.Clear();
        hasTarget = false;
        isMissed = false;
        timer = 0f; // Reset the timer when firing
    }

    public AmmunitionData GetAmmunitionData()
    {
        return ammo;
    }

    public void SetupBullet(LinkedList<Collider2D> colliders)
    {
        bulletRb = GetComponent<Rigidbody2D>();
        ignoreColliders = colliders;
    }

    // Calculate the trajectory and return a list of possible collisions
    public LinkedList<RaycastHit2D> CalculateTrajectory(float length)
    {
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
            if (hit.collider != null && !ignoreColliders.Contains(hit.collider) && bulletBehavior == null)
            {
                if (!existingColliders.Contains(hit.collider))
                {
                    Debug.Log("Add collider: " + hit.collider.gameObject);
                    collisions.AddLast(hit);
                    existingColliders.Add(hit.collider);
                    // Draw the hit point
                    Debug.DrawLine(position, hit.point, Color.green, 1f);
                }
            }
        }

        return collisions;
    }


    public Collider2D CalculateHit(LinkedList<RaycastHit2D> hits, float missChance)
    {
        if (hits.Count > 0)
        {
            // Determine if the shot is a miss based on the miss chance
            if (Random.value < missChance)
            {
                // It's a miss
                hasTarget = false;
                isMissed = true;
                return null;
            }

            // Choose a random hit from the list
            int randomIndex = Random.Range(0, hits.Count);
            LinkedListNode<RaycastHit2D> node = hits.First;

            for (int i = 0; i < randomIndex; i++)
            {
                node = node.Next;
            }

            RaycastHit2D hit = node.Value;
            hasTarget = true;
            isMissed = false;
            return hit.collider;
        }
        else
        {
            // No hits, it's a miss
            hasTarget = false;
            isMissed = true;
            return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Obstacle")
        {
            // Assuming a 20% chance to miss
            float missChance = 0.2f;
            Collider2D hitCollider = CalculateHit(CalculateTrajectory(5), missChance);
            TankPart part = null;
            hitCollider.TryGetComponent(out part);                 
            if (hitCollider != null && part != null)
            {
                // Handle the hit
                if (collision = hitCollider)
                {
                    part.TakeDamage(5);
                    Debug.Log("Hit: " + hitCollider.gameObject.name);
                    DeactivateBullet();
                }
            }
            else if (isMissed)
            {
                // Handle the miss
                Debug.Log("Missed");
            }
        }
        else
        {
            DeactivateBullet();
            Debug.Log("Hit wall");
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        // Remove the collider from existingColliders
        if (existingColliders.Contains(other))
        {
            Debug.Log("Remove Collider: " + other);
            existingColliders.Remove(other);
            // Also remove from collisions list
            LinkedListNode<RaycastHit2D> node = collisions.First;
            while (node != null)
            {
                LinkedListNode<RaycastHit2D> nextNode = node.Next;
                if (node.Value.collider == other)
                {
                    collisions.Remove(node);
                }
                node = nextNode;
            }
        }
    }
}
