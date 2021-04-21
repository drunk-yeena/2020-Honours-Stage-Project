using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBullet : MonoBehaviour
{
    private ProjectilePhysicsGenericScript projectilePhysics;
    private Rigidbody bulletRigidbody;
    private BoxCollider boxCollider;

    public float speed;
    public float projectileLifeSpan;
    public float reflectionCount;

    private readonly bool richocetBullets = true;
    private readonly bool infiniteLifeSpan = false;

    // Start is called before the first frame update
    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        projectilePhysics = GetComponent<ProjectilePhysicsGenericScript>();

        projectilePhysics.BulletRigidbody = bulletRigidbody;
        projectilePhysics.BoxCollider = boxCollider;

        projectilePhysics.SetBulletType("BULLET_RED");

        projectilePhysics.Speed = speed;
        projectilePhysics.ProjectileLifeSpan = projectileLifeSpan;
        projectilePhysics.InfiniteLifeSpan = infiniteLifeSpan;

        projectilePhysics.RichocetBullets = richocetBullets;
        projectilePhysics.ReflectionCount = reflectionCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
