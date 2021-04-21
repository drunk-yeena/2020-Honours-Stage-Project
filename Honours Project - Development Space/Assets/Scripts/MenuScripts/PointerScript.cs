using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerScript : MonoBehaviour
{
    private float defaultLength = 5f;
    public GameObject dot;
    public InputModule inputModule;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once every frame
    void Update()
    {
        Line();
    }

    /// <summary>
    /// Method for calculating Raycast for selection of menu in space
    /// </summary>
    void Line()
    {
        PointerEventData data = inputModule.GetData();
        float targetLength = data.pointerCurrentRaycast.distance == 0 ? defaultLength : data.pointerCurrentRaycast.distance;

        RaycastHit hit = RenderRaycast(targetLength);
        Vector3 endPosition = transform.position + (transform.forward * targetLength);

        if(hit.collider != null)
        {
            endPosition = hit.point;
        }

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);
        dot.transform.position = endPosition;        
    }

    /// <summary>
    /// Method for graphically rendering Raycast
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    private RaycastHit RenderRaycast(float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, defaultLength);

        return hit;
    }
}
