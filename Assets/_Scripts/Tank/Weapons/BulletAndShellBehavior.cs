using System.Collections;
using UnityEngine;

public class BulletAndShellBehavior : MonoBehaviour
{
    public AmmunitionData ammo;
    public float lifespan = 2.0f; // Adjust the lifespan as needed
    public float timer;
    private Rigidbody2D bulletRb;

    // Start is called before the first frame update
    void Start()
    {
        bulletRb = GetComponent<Rigidbody2D>();
        gameObject.SetActive(false);
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
    { return ammo; }
}
