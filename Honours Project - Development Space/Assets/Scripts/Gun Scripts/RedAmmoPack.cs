using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedAmmoPack : MonoBehaviour
{
    private AmmoPickupGeneric ammoPickup;
    private readonly string ammoType = "Red";

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
