using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGun : MonoBehaviour
{
    private PlayerController player;
    private GunObjectGenericScript gunGeneric;

    public GameObject gunPrefab;
    public GameObject emmitterPoint;
    public GameObject projectile;

    public float rateOfFire;
    public int magazineSize;
    public bool infiniteAmmo;
    public bool automaticFire;

    // Start is called before the first frame update
    void Start()
    {
        gunGeneric = GetComponent<GunObjectGenericScript>();
        player = GameObject.FindObjectOfType<PlayerController>();

        gunGeneric.GunPrefab = gunPrefab;
        gunGeneric.EmitterPoint = emmitterPoint;
        gunGeneric.Projectile = projectile;

        gunGeneric.RateOfFire = rateOfFire;
        gunGeneric.AmmoPool = player.RedAmmoPool;
        gunGeneric.MagSize = magazineSize;
        gunGeneric.currentAmmoCount = magazineSize;
        gunGeneric.InfiniteAmmo = infiniteAmmo;
        gunGeneric.AutomaticFire = automaticFire;
    }

    // Update is called once per frame
    void Update()
    {
        player.BlueAmmoPool = gunGeneric.AmmoPool;
    }
}
