using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class GunObjectGenericScript : MonoBehaviour
{
    //SteamVR Interactable Script - Allows for items to be handled by virtual reality hands
    private Interactable interactable;

    //Controller Input Variables - tied to buttons and actions
    public SteamVR_Action_Boolean fireAction;
    public SteamVR_Action_Boolean reloadAction;
    
    //Gameobject Children
    public GameObject GunPrefab { get; set; }
    public GameObject EmitterPoint { get; set; }
    public GameObject Projectile { get; set; }

    [SerializeField]
    private GameObject ring;
    [SerializeField]
    private GameObject core;

    //Get-Set values for mechanics of the gun
    public float RateOfFire { get; set; }
    public int MagSize { get; set; }
    public int AmmoPool { get; set; }
    public bool AutomaticFire { get; set; }
    public bool InfiniteAmmo { get; set; }

    private float fireTimer;
    public int currentAmmoCount;

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer += Time.deltaTime;

        //Check to see if object has been grabbed with a player's hand
        if (interactable.attachedToHand != null)
        {
            RotateRings(ring);
            RotateCore(core);

            SteamVR_Input_Sources source = interactable.attachedToHand.handType;
            SingleShotFire(source);
            AutomaticShotFire(source);
            Reload(source);
        } 
    }

    void SpawnProjectile()
    {
        Quaternion projectileRotation;
        projectileRotation = EmitterPoint.transform.rotation;

        if (EmitterPoint != null)
        {
            _ = Instantiate(Projectile, EmitterPoint.transform.position, projectileRotation);
        }
        else
        {
            Debug.Log("No Fire Point");
        }
    }

    /// <summary>
    /// Single Shot fire Method. Can be toggled, infinite fire can be toggled
    /// </summary>
    /// <param name="source"></param>
    void SingleShotFire(SteamVR_Input_Sources source)
    {
        if (AutomaticFire == false)
        {
            if (InfiniteAmmo == true)
            {
                if (fireAction[source].stateDown && (fireTimer > RateOfFire))
                {
                    SpawnProjectile();
                    fireTimer = 0;
                }
            }
            else if (currentAmmoCount > 0)
            {
                if (fireAction[source].stateDown && (fireTimer > RateOfFire))
                {
                    SpawnProjectile();
                    currentAmmoCount--;
                    fireTimer = 0;
                }
            }
        }
    }

    /// <summary>
    /// Automatic fire Method. Can be toggled, infinite fire can be toggled
    /// </summary>
    /// <param name="source"></param>
    void AutomaticShotFire(SteamVR_Input_Sources source)
    {
        if (AutomaticFire == true)
        {
            if (InfiniteAmmo == true)
            {
                while (fireAction[source].state == true && (fireTimer > RateOfFire))
                {
                    SpawnProjectile();
                    fireTimer = 0;
                }
            }
            else if (currentAmmoCount > 0)
            {
                while (fireAction[source].state == true && (fireTimer > RateOfFire))
                {
                    SpawnProjectile();
                    currentAmmoCount--;
                    fireTimer = 0;
                }
            }
        }
    }

    /// <summary>
    /// Ability to reload a game when holding a weapon.
    /// reloadAction source is currently set to: Y/B
    /// </summary>
    /// <param name="source"></param>
    void Reload(SteamVR_Input_Sources source)
    {
        if (reloadAction[source].stateDown)
        {
            int refillammoCount = MagSize - currentAmmoCount;
            if (AmmoPool >= refillammoCount)
            {
                AmmoPool -= refillammoCount;
                currentAmmoCount = MagSize;
            }
            else
            {
                Debug.Log("Ammo is not sufficent");
            }
        }
    }

    void RotateRings(GameObject ring)
    {
        ring.transform.Rotate((new Vector3(0, 0, 10) * Time.deltaTime));
    }

    void RotateCore(GameObject core)
    {
        core.transform.Rotate((new Vector3(60, -40, 74) * Time.deltaTime));
    }
}
