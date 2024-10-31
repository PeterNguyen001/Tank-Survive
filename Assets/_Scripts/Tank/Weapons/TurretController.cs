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
           
            bool shouldFire = isPullingTheTrigger;
            foreach (GunBehaviour gun in turret.GetGunUnderTurretControl())
            {
                gun.FireGun(shouldFire);
            }
        }
    }

    public void AttackTaget()
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
                    shouldFire = IsTargetOnLineOfSight(gun);
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

    public Vector3 GetNearestEnemyPosition(TurretAndPortBehaviour turret)
    {
        Transform turretTransform = turret.transform;
        Vector3 nearestEnemyPosition = aiSensor.Detect(turret.transform, 5, 360, tagsToDetect, false).position;
        return nearestEnemyPosition;
    }

    public bool IsTargetOnLineOfSight(GunBehaviour gun)
    {
        DetectionInfo targetDetectionInfo = aiSensor.Detect(gun.FindGunlEnd().transform, 5, 2, tagsToDetect);
        if (targetDetectionInfo.tag != "")
        {
            Debug.Log("Enemy Spotted");
            return true;
        }
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
