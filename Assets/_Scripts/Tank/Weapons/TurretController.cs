using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurretController : TankSubComponent
{


    private LinkedList<GunBehaviour> gunList = new LinkedList<GunBehaviour>();
    private LinkedList<TurretAndPortBehaviour> turretAndGunPortList = new LinkedList<TurretAndPortBehaviour>();
    public List<TurretAndPortBehaviour>  tList = new List<TurretAndPortBehaviour>(); 
    private LinkedList<Collider2D> tankColliders;

    private Vector3 mousePosition;
    private bool isPullingTheTrigger;


    [SerializeField] List<string> tagsToDetect = new List<string>();

    public float detectionRange = 10f;

    private AISensor aiSensor;


    // Start is called before the first frame update

    public override void Init()
    {
        gunList.Clear();
        gunList = tankStatus.GetListOfGun();
        turretAndGunPortList.Clear();
        turretAndGunPortList = tankStatus.GetListOfTurretAndPort();
        tankColliders = tankStatus.GetListOfCollider2D();
        aiSensor = GetComponent<AISensor>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (TurretAndPortBehaviour turret in turretAndGunPortList)
        {
            if (!turret.isAIControl)
            {
                turret.AimGunAt(mousePosition);
            }
            else
            {
                Vector3 enemyPosition = GetNearestEnemyPosition(turret);
                if (enemyPosition != Vector3.zero)
                {
                    
                    turret.AimGunAt(enemyPosition);
                }
            }

            bool shouldFire = isPullingTheTrigger;
            foreach (GunBehaviour gun in turret.GetGunUnderTurretControl())
            {
                if (turret.isAIControl)
                {
                    shouldFire = IsEnemyOnGunSight(gun);
                }
                gun.FireGun(shouldFire);
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

    public GameObject DetectNearestEnemyFromGun(TurretAndPortBehaviour turret)
    {
        GameObject nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        Vector3 gunPosition = turret.transform.position;
        float coneAngle = turret.turretAndGunPortData.maxRotationAngle * 2f; // Double the angle for the cone

        // Calculate the cone direction based on the turret's rotation
        Vector3 coneDirection = Quaternion.Euler(0, 0, turret.GetTurretLocalInitialAngle() + transform.eulerAngles.z % 360f) * Vector3.right;

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
                    if (Mathf.Abs(angleToEnemy) <= turret.turretAndGunPortData.maxRotationAngle)
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

    public Vector3 GetNearestEnemyPosition(TurretAndPortBehaviour turret)
    {
        Transform turretTransform = turret.transform;
        Vector3 nearestEnemyPosition = aiSensor.Detect(turret.transform, 5, 360, tagsToDetect).position;
        return nearestEnemyPosition;
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

    private bool IsEnemyOnGunSight(GunBehaviour gun)
    {
        // Get the position and rotation of the gunEnd
        Vector3 gunEndPosition = gun.FindGunlEnd().transform.position;
        Quaternion gunRotation = gun.transform.rotation;

        // Calculate the direction from the right side of the gunEnd
        Vector3 rayDirection = gunRotation * Vector3.right;

        // Temporarily disable the tank's colliders
        LinkedListNode<Collider2D> current = tankColliders.First;
        while (current != null)
        {
            current.Value.enabled = false;
            current = current.Next;
        }

        // Cast a ray in the direction of the turret's rotation with the detection range
        RaycastHit2D hit = Physics2D.Raycast(gunEndPosition, rayDirection, detectionRange);

        // Re-enable the tank's colliders
        current = tankColliders.First;
        while (current != null)
        {
            current.Value.enabled = true;
            current = current.Next;
        }

        // Draw the ray for debugging purposes
        Debug.DrawRay(gunEndPosition, rayDirection * detectionRange, Color.red);

        // Check if the ray hits an enemy object
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            // Draw a green line if an enemy is hit
            Debug.DrawLine(gunEndPosition, hit.point, Color.green);
            return true;
        }

        // Draw a yellow line if no enemy is hit
        Debug.DrawLine(gunEndPosition, gunEndPosition + rayDirection * detectionRange, Color.yellow);
        return false;
    }

    public bool IsTargetOnLineOfSight(GunBehaviour gun)
    {
        return false;
    }
    //private void OnDrawGizmosSelected()
    //{
    //    foreach (var gun in gunList)
    //    {
    //        if (gun.gunData != null)
    //        {
    //            // Calculate the cone direction based on the maximum rotation angle
    //            Vector3 coneDirection = Quaternion.Euler(0, 0, gun.GetGunLocalInitialAngle() + transform.eulerAngles.z % 360f) * Vector3.right;

    //            // Draw the detection cone
    //            Gizmos.color = Color.yellow;
    //            Gizmos.DrawRay(gun.transform.position, coneDirection * obstacleDetectionRange);
    //            Gizmos.DrawRay(gun.transform.position, Quaternion.Euler(0, 0, gun.gunData.maxRotationAngle) * coneDirection * obstacleDetectionRange);
    //            Gizmos.DrawRay(gun.transform.position, Quaternion.Euler(0, 0, -gun.gunData.maxRotationAngle) * coneDirection * obstacleDetectionRange);

    //            Vector3 gunEndPosition = gun.FindGunlEnd().transform.position;
    //            Quaternion gunRotation = gun.transform.rotation;

    //            // Calculate the direction from the right side of the gunEnd
    //            Vector3 lineDirection = gunRotation * Vector3.right;

    //            // Draw the line from the gunEnd
    //            Gizmos.color = Color.green;
    //            Gizmos.DrawRay(gunEndPosition, lineDirection * obstacleDetectionRange);
    //        }
    //    }
    //}
}
