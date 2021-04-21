using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportingBallScript : MonoBehaviour
{
    private PlayerController playerController;

    private float reflectionCount = 3;
    private readonly float speed = 25;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        Invoke("DestroyObject", 5);
    }

    // Update is called once per frame
    void Update()
    {
        Velocity();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "TeleportPanel":
                SendTeleportData();
                break;

            case "TeleportBounce":
                if(reflectionCount > 0)
                {
                    Richocet();
                }
                else
                {
                    DestroyObject();
                }
                break;

            default:
                DestroyObject();
                break;
        }
    }

    void Velocity()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);
    }

    void Richocet()
    {
        Ray reflectionRay = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(reflectionRay, out hit, Time.deltaTime * speed + .1f))
        {
            Vector3 reflectDirection = Vector3.Reflect(reflectionRay.direction, hit.normal);
            float rotation = 90 - Mathf.Atan2(reflectDirection.z, reflectDirection.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, rotation, 0);

            reflectionCount--;
        }
    }

    void SendTeleportData()
    {
        playerController.TeleportPosition = transform.position;
        Debug.Log("Ball landed, Position: " + playerController.TeleportPosition);
        playerController.TeleportSnap();
        DestroyObject();
    }

    void DestroyObject()
    {
        playerController.TeleportBallWorldSpace = false;
        Destroy(this.gameObject);
    }
}
