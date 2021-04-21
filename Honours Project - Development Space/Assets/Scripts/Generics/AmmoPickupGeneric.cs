using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class AmmoPickupGeneric : MonoBehaviour
{
    //SteamVR Interactable Script - Allows for items to be handled by virtual reality hands
    private Interactable interactable;
    private PlayerController player;

    public SteamVR_Action_Boolean gainAmmo;

    [SerializeField]
    private GameObject upperRotate;
    [SerializeField]
    private GameObject lowerRotate;
    
    public string AmmoType { get; set; }
    [SerializeField]
    private int ammoPickupCount;

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
        player = GameObject.FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateRings(upperRotate);
        RotateRings(lowerRotate);

        //Check to see if object has been grabbed with a player's hand
        if (interactable.attachedToHand != null)
        {
            SteamVR_Input_Sources source = interactable.attachedToHand.handType;
            GiveAmmo(source);
        }
    }

    /// <summary>
    /// Method used to determine what type of ammo the specific pickup object is.
    /// Ammo type determined by string
    /// </summary>
    /// <param name="source"></param>
    void GiveAmmo(SteamVR_Input_Sources source)
    {
        if (gainAmmo[source].stateDown)
        {
            switch (AmmoType)
            {
                case "Green":
                    player.GreenAmmoPool += ammoPickupCount;
                    Destroy(gameObject);
                    break;

                case "Red":
                    player.RedAmmoPool += ammoPickupCount;
                    Destroy(gameObject);
                    break;

                case "Blue":
                    player.BlueAmmoPool += ammoPickupCount;
                    Destroy(gameObject);
                    break;

                default:
                    Debug.LogWarning("No AmmoType Identified, check the string value that was given to this ammo pick up.\nProviding small ammo to all pools");
                    player.GreenAmmoPool += 30;
                    player.RedAmmoPool += 30;
                    player.BlueAmmoPool += 30;
                    Destroy(gameObject);
                    break;
            }
        }
    }

    void RotateRings(GameObject ring)
    {
        ring.transform.Rotate((new Vector3(0, 0, 10) * Time.deltaTime));
    }
}
