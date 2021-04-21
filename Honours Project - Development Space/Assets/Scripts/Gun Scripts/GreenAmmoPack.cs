using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenAmmoPack : MonoBehaviour
{
    private AmmoPickupGeneric ammoPickup;
    private readonly string ammoType = "Green";

    // Start is called before the first frame update
    void Start()
    {
        ammoPickup = this.gameObject.GetComponent<AmmoPickupGeneric>();
        ammoPickup.AmmoType = ammoType;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
