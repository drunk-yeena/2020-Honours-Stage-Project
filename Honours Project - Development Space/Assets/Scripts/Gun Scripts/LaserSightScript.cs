using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class LaserSightScript : MonoBehaviour
{
    public LineRenderer line;
    private Vector3 endPoint;
    private Vector3 endPointDefault = new Vector3(0, 0, 350);

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit raycast;

        if(Physics.Raycast(transform.position, transform.forward, out raycast))
        {
            if (raycast.collider)
            {
                endPoint = new Vector3(0, 0, raycast.distance);
                line.SetPosition(1, endPoint);
            }
        }
        else
        {
            line.SetPosition(1, endPointDefault);
        }
    }
}
