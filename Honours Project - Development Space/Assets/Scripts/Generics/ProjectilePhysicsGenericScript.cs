using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePhysicsGenericScript : MonoBehaviour
{
    /// <summary>
    /// Bullet Types to identify with on collision.
    /// Can be used for damage, keys etc.
    /// </summary>
    enum BulletTypes 
    {
        BULLET_RED,
        BULLET_GREEN,
        BULLET_BLUE,
    }
    private BulletTypes bulletType;

    //Additional Components of Generic Bullet
    public Rigidbody BulletRigidbody { get; set; }
    public BoxCollider BoxCollider { get; set; }

    //Universal variables for any projectile type
    public float Speed { get; set; }
    public float ProjectileLifeSpan { get; set; }
    public float ReflectionCount { get; set; }
    public float BounceCount { get; set; }

    //Toggle variables for various gun properties
    public bool InfiniteLifeSpan { get; set; } = false;
    public bool BouncingBullets { get; set; } = false;
    public bool RichocetBullets { get; set; } = false;
    public bool HomingBullets { get; set; } = false;


    // Start is called before the first frame update
    void Start()
    {
        if (RichocetBullets == false)
        {
            ReflectionCount = 0;
        }

        if(BouncingBullets == false)
        {
            BounceCount = 0;
        }
        else
        {
            BulletRigidbody.useGravity = true;
            BoxCollider.material = (PhysicMaterial)Resources.Load("PhysicsMaterials/BounceBullet");
        }

        if(InfiniteLifeSpan == false)
        {
            Destroy(gameObject, ProjectileLifeSpan);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Velocity();
    }

    void Velocity()
    {
        transform.position += transform.forward * (Speed * Time.deltaTime);
    }

    /// <summary>
    /// Collision method holding the actions to perform depending on boolean values
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            collision.gameObject.GetComponent<TargetScriptGeneric>().TargetHit();
        }

        if (RichocetBullets == true)
        {
            if (ReflectionCount > 0)
            {
                Richocet();
            }
            else
            {
                Speed = 0;
                Destroy(gameObject);
            }
        }
        else if (BouncingBullets == true)
        {
            if (BounceCount > 0)
            {
                Bouncing();
            }
            else
            {
                Speed = 0;
                Destroy(gameObject);
            }
        }
        else
        {
            Speed = 0;
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Method for bullets reflecting against surfaces.
    /// Uses a raycast ahead of the bullet to determine the new angle of the bullet before rotating the bullet.
    /// </summary>
    void Richocet()
    {
        Ray reflectionRay = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(reflectionRay, out hit, Time.deltaTime * Speed + .1f))
        {
            Vector3 reflectDirection = Vector3.Reflect(reflectionRay.direction, hit.normal);
            float rotation = 90 - Mathf.Atan2(reflectDirection.z, reflectDirection.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, rotation, 0);

            ReflectionCount--;
        }
    }

    /// <summary>
    /// Bullets with bouncing property inherit bouncing material.
    /// On impact, bullets are slowed down.
    /// </summary>
    void Bouncing()
    {
        if (BouncingBullets == true)
        {
            Speed *= 0.65f;
            BounceCount--;
        }
    }

    /// <summary>
    /// Bullet enum type used to determine actions on impact such as damage.
    /// </summary>
    /// <param name="bulletTypeString"></param>
    public void SetBulletType(string bulletTypeString)
    {
        switch (bulletTypeString)
        {
            case "BULLET_RED":
                bulletType = BulletTypes.BULLET_RED;
                break;

            case "BULLET_GREEN":
                bulletType = BulletTypes.BULLET_GREEN;
                break;

            case "BULLET_BLUE":
                bulletType = BulletTypes.BULLET_BLUE;
                break;

            default:
                Debug.Log("No Enum/Incorrect Enum Was Given - Setting to Green Properties");
                bulletType = BulletTypes.BULLET_GREEN;
                break;
        }
    }
}
