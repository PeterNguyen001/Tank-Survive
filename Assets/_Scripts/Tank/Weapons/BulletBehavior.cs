using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public AmmunitionData ammoData;
    
    private GameObject ownerObject;

    public float lifespan = 2.0f; // Adjust the lifespan as needed
    public float timer;
    private Rigidbody2D bulletRb;
    private int penetrationPower;

    [SerializeField]
    private LinkedList<Collider2D> ignoreColliders = new LinkedList<Collider2D>();

    private LinkedList<RaycastHit2D> collisions = new LinkedList<RaycastHit2D>();
    private HashSet<Collider2D> existingColliders = new HashSet<Collider2D>();

    private HashSet<TankPart> existingTankPart = new HashSet<TankPart>();

    private LinkedList<Armor> hitArmorList = new LinkedList<Armor>();
    private HashSet<Armor> penetratedArmorList = new HashSet<Armor>();
    private HashSet<Armor> existingArmor = new HashSet<Armor>(); // New HashSet for Armor components

    // Define cone properties
    public float coneAngle = 10f;
    public int rayCount = 5;
    public float rayLength = 10f;


    private bool hasTarget;
    private bool isMissed;
    private bool hasHitted;

    public int PenetrationPower { get => penetrationPower; set => penetrationPower = value; }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameObject.activeSelf)
        {
            bulletRb.AddForce(transform.right * ammoData.velocity);

            // Update the timer
            timer += Time.fixedDeltaTime;

            // Check if the bullet has exceeded its lifespan
            if (timer >= lifespan)
            {
                DeactivateBullet();
            }
        }
    }

    public void Fire(GameObject owner)
    {
        penetrationPower = ammoData.penetrationPower;
        gameObject.SetActive(true);
        ownerObject = owner;
    }

    // Deactivate the bullet and reset the timer
    public void DeactivateBullet()
    {

        // Reset all Armor's IsBeingHit status before deactivating
        foreach (Armor armor in hitArmorList)
        {
            armor.IsBeingHit = false;
        }

        gameObject.SetActive(false);

        // Clear all collections for reuse
        collisions.Clear();
        existingColliders.Clear();
        hitArmorList.Clear();
        existingArmor.Clear();
        penetratedArmorList.Clear();
        existingTankPart.Clear();

        hasTarget = false;
        isMissed = false;
        hasHitted = false;
        ownerObject = null;
        timer = 0f; // Reset the timer when firing
    }

    public AmmunitionData GetAmmunitionData()
    {
        return ammoData;
    }

    public void SetupBullet(LinkedList<Collider2D> colliders)
    {
        bulletRb = GetComponent<Rigidbody2D>();
        ignoreColliders = colliders;
    }

    public float CastRayConeAndCalculateAverageHitAngle(Armor armor)
    {
        // Adjust this offset to move the raycast start position
        float rayStartOffset = 0.5f;

        // Calculate the adjusted starting position for the raycasts
        Vector2 position = (Vector2)transform.position - (Vector2)transform.right * rayStartOffset;
        Vector2 bulletDirection = transform.right;

        // Calculate the starting angle of the cone
        float startAngle = -coneAngle / 2f;

        List<Vector2> hitPoints = new List<Vector2>();
        List<float> angles = new List<float>();

        for (int i = 0; i < rayCount; i++)
        {
            // Calculate each ray's angle within the cone
            float angle = startAngle + (coneAngle / (rayCount - 1)) * i;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * bulletDirection;

            // Perform RaycastAll in the current direction within the cone
            RaycastHit2D[] hits = Physics2D.RaycastAll(position, direction, rayLength);

            // Filter hits to include only the specified Armor object
            foreach (RaycastHit2D hit in hits)
            {
                Collider2D collider = hit.collider;
                if (collider != null && collider.gameObject == armor.gameObject)
                {
                    hitPoints.Add(hit.point); // Collect the hit point
                    Debug.DrawRay(position, direction * hit.distance, Color.green, 1f);
                    break; // Stop after the first hit on this armor to avoid extra hits along the same ray
                }
            }
        }

        // Calculate angles between bullet direction and lines formed by consecutive hit points
        for (int i = 0; i < hitPoints.Count - 1; i++)
        {
            Vector2 pointA = hitPoints[i];
            Vector2 pointB = hitPoints[i + 1];

            // Draw a line between consecutive hit points
            Debug.DrawLine(pointA, pointB, Color.blue, 1f);

            // Calculate the direction vector of the line segment
            Vector2 lineDirection = (pointB - pointA).normalized;

            // Calculate angle between bullet direction and line segment direction
            float angleToSurface = Mathf.Abs(Vector2.Angle(bulletDirection, lineDirection)) - 90;
            angles.Add(angleToSurface);
        }

        // Calculate and return the average angle
        if (angles.Count > 0)
        {
            float averageAngle = 0;
            foreach (float angle in angles)
            {
                averageAngle += angle;
            }
            averageAngle /= angles.Count;
            Debug.Log("Average angle of incidence: " + averageAngle);
            return averageAngle;
        }
        else
        {
            Debug.Log("No valid angles to calculate average.");
            return 0f; // Return 0 if no angles were calculated
        }
    }


    public void RemovePenetratedPower(int effectiveThickness)
    {
        Debug.Log("P");
        penetrationPower -= effectiveThickness;
        if (penetrationPower <= 0f)
        {
            DeactivateBullet();
        }
    }

    public void AddArmorToPenetratedList(Armor armor)
    {
        penetratedArmorList.Add(armor);
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
            TankPart part = hit.collider.gameObject.GetComponent<TankPart>();
            if (hit.collider != null && !ignoreColliders.Contains(hit.collider) && bulletBehavior == null && part != null)
            {
                if (!existingColliders.Contains(hit.collider))
                {                   
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
        //if (collision.CompareTag("Armor"))
        //{
        //    Armor armor = collision.GetComponent<Armor>();

        //    if (armor != null && !existingArmor.Contains(armor) && !penetratedArmorList.Contains(armor) )
        //    {
        //        //Tools.PauseEditor();
        //        Debug.Log("Hit: " + armor.gameObject.name);
        //        hitArmorList.AddLast(armor);
        //        existingArmor.Add(armor);
        //        armor.IsBeingHit = true;
        //        float missChance = 0.2f;
        //        Collider2D hitCollider = CalculateHit(CalculateTrajectory(5), missChance);
        //        if (hitCollider != null && !hasHitted)
        //        {
        //            TankPart part = hitCollider.GetComponent<TankPart>();
        //            if (part == armor.TankPartAttachedTo)
        //            {
        //                CastRayConeAndCalculateAverageHitAngle(armor);
        //                //hasHitted = true;
        //                part?.TakeHit(this);
        //                Debug.Log("Hit: " + hitCollider.gameObject.name);
        //            }
        //        }
        //    }

        //    // Exit here to process armor hit first
        //    return;
        //}

        if (collision.CompareTag("Armor"))
        {
            Armor armor = collision.GetComponent<Armor>();

            if (armor != null && !existingArmor.Contains(armor) &&  armor.OwnerObject != ownerObject)
            {
                //Tools.PauseEditor();
                Debug.Log("Hit: " + armor.gameObject.name);
                hitArmorList.AddLast(armor);
                existingArmor.Add(armor);
                armor.IsBeingHit = true;
                
                //Collider2D hitCollider = CalculateHit(CalculateTrajectory(5), missChance);
                //if (hitCollider != null && !hasHitted)
                //{
                //    TankPart part = hitCollider.GetComponent<TankPart>();
                //    if (part == armor.TankPartAttachedTo)
                //    {
                //        CastRayConeAndCalculateAverageHitAngle(armor);
                //        //hasHitted = true;
                //        part?.TakeHit(this);
                //        Debug.Log("Hit: " + hitCollider.gameObject.name);
                //    }
                //}

                    armor.CheckForPenetration(this);
               
            }

            // Exit here to process armor hit first
            return;
        }

        if (collision.GetComponent<TankPart>().OwnerObject != ownerObject)
        {
            TankPart hitPart = collision.GetComponent<TankPart>();

            Debug.Log(collision.GetComponent<TankPart>().OwnerObject);
            Debug.Log(ownerObject);
            //EditorApplication.isPaused = true;
            float missChance = 0.2f;
            Collider2D hitCollider = CalculateHit(CalculateTrajectory(5), missChance);
            if (hitCollider != null && !existingTankPart.Contains(hitPart))
            {
                TankPart part = hitCollider.GetComponent<TankPart>();

                    part?.TakeHit(this);
                    Debug.Log("Hit: " + hitCollider.gameObject.name);
                    existingTankPart.Add(hitPart);
            }
        }

        if (collision.CompareTag("Obstacle"))
        {
            DeactivateBullet();
            Debug.Log("Hit wall");
            return;
        }

        else if (isMissed)
        {
            Debug.Log("Missed");
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log(other.gameObject.name);
        // Remove the collider from existingColliders
        if (existingColliders.Contains(other))
        {
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

        // Remove armor if exiting from Armor collider
        Armor armor = other.GetComponent<Armor>();
        if (armor != null && existingArmor.Contains(armor))
        {
            armor.IsBeingHit = false;
            existingArmor.Remove(armor);

            // Also remove from hitArmorList
            LinkedListNode<Armor> armorNode = hitArmorList.First;
            while (armorNode != null)
            {
                LinkedListNode<Armor> nextArmorNode = armorNode.Next;
                if (armorNode.Value == armor)
                {
                    hitArmorList.Remove(armorNode);
                }
                armorNode = nextArmorNode;
            }
        }
    }
}
