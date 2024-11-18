using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    Collider2D armorCollider2D;
    [SerializeField]
    int thickness;

    public bool isBeingHit;
    bool isPenetrated;
    private TankPart partAttachedTo;

    private PolygonCollider2D polygonCollider2D;

    public List<Vector2> edgeNormals = new List<Vector2>();

    public bool IsBeingHit { get => isBeingHit; set => isBeingHit = value; }
    public TankPart TankPartAttachedTo { get => partAttachedTo; set => partAttachedTo = value; }

    private void Start()
    {
       armorCollider2D = GetComponent<Collider2D>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        GenerateEdgeNormals();
    }

    // Method to generate normals along the edges of the polygon collider
    private void GenerateEdgeNormals()
    {
        if (polygonCollider2D == null) return;

        Vector2[] points = polygonCollider2D.points;

        // Loop through each edge and calculate normals
        for (int i = 0; i < points.Length; i++)
        {
            // Get current and next points to form an edge
            Vector2 p1 = transform.TransformPoint(points[i]);
            Vector2 p2 = transform.TransformPoint(points[(i + 1) % points.Length]);

            // Calculate edge direction and normal
            Vector2 edgeDirection = (p2 - p1).normalized;
            Vector2 edgeNormal = new Vector2(-edgeDirection.y, edgeDirection.x); // Perpendicular vector as normal

            edgeNormals.Add(edgeNormal);

            // Optionally, draw the normals in the Scene view for visualization
            Debug.DrawLine((p1 + p2) / 2, (p1 + p2) / 2 + edgeNormal * 0.5f, Color.blue, 2.0f);
        }
    }
    public bool CheckForPenetration(BulletBehavior bullet)
    {
        if (bullet.GetAmmunitionData().penetrationPower > thickness)
        {
            Debug.Log("Pen");
            return true;
        }
        else
        {
            bullet.DeactivateBullet();
            Debug.Log("NonPen");
            return false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            Debug.Log("Projectile left the armor!");
            isBeingHit = false;
        }
    }

    public float CalculateEffectiveThickness(Vector2 bulletDirection)
    {
        float minEffectiveThickness = float.MaxValue;

        foreach (Vector2 normal in edgeNormals)
        {
            float angleCosine = Vector2.Dot(bulletDirection.normalized, normal);
            float effectiveThickness = thickness / Mathf.Abs(angleCosine);
            minEffectiveThickness = Mathf.Min(minEffectiveThickness, effectiveThickness);
        }

        return minEffectiveThickness;
    }
}

