using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBullet : MonoBehaviour
{
    private ProjectilePhysicsGenericScript projectilePhysics;
    private Rigidbody bulletRigidbody;
    private BoxCollider boxCollider;

    public float speed;
    public float projectileLifeSpan;

    private readonly bool infiniteLifeSpan = false;

    // Start is called before the first frame update
    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        projectilePhysics = GetComponent<ProjectilePhysicsGenericScript>();

        projectilePhysics.BulletRigidbody = bulletRigidbody;
        projectilePhysics.BoxCollider = boxCollider;

        projectilePhysics.SetBulletType("BULLET_GREEN");

        projectilePhysics.Speed = speed;
        projectilePhysics.ProjectileLifeSpan = projectileLifeSpan;
        projectilePhysics.InfiniteLifeSpan = infiniteLifeSpan;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
