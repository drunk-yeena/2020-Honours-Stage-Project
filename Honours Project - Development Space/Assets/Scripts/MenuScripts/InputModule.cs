using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class InputModule : BaseInputModule
{
    private GameObject currentObject;
    private PointerEventData data;
    public Camera camera;

    //Input Action for selecting in the menu
    public SteamVR_Input_Sources targetSource;
    public SteamVR_Action_Boolean selectAction;

    protected override void Awake()
    {
        base.Awake();
        data = new PointerEventData(eventSystem);
    }

    public override void Process()
    {
        data.Reset();
        data.position = new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2);

        eventSystem.RaycastAll(data, m_RaycastResultCache);
        data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        currentObject = data.pointerCurrentRaycast.gameObject;

        m_RaycastResultCache.Clear();
        HandlePointerExitAndEnter(data, currentObject);

        if (selectAction.GetStateDown(targetSource))
        {
            ProcessPress(data);
        }

        if (selectAction.GetStateUp(targetSource))
        {
            ProcessRelease(data);
        }
    }

    public PointerEventData GetData()
    {
        return data;
    }

    /// <summary>
    /// Process method for when a canvas item is pointed at with the input action pressed.
    /// </summary>
    /// <param name="eventData"></param>
    void ProcessPress(PointerEventData eventData)
    {
        eventData.pointerCurrentRaycast = eventData.pointerCurrentRaycast;
        GameObject pointerPress = ExecuteEvents.ExecuteHierarchy(currentObject, eventData, ExecuteEvents.pointerDownHandler);

        if(pointerPress == null)
        {
            pointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentObject);
        }

        eventData.pressPosition = eventData.position;
        eventData.pointerPress = pointerPress;
        eventData.rawPointerPress = currentObject;
    }

    /// <summary>
    /// Process method for when the input action is released above the initally pressed canvas item
    /// </summary>
    /// <param name="eventData"></param>
    void ProcessRelease(PointerEventData eventData)
    {
        ExecuteEvents.Execute(eventData.pointerPress, eventData, ExecuteEvents.pointerUpHandler);

        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentObject);

        if(eventData.pointerPress == pointerUpHandler)
        {
            ExecuteEvents.Execute(eventData.pointerPress, eventData, ExecuteEvents.pointerClickHandler);
        }

        eventSystem.SetSelectedGameObject(null);

        eventData.pressPosition = Vector2.zero;
        eventData.pointerPress = null;
        eventData.rawPointerPress = null;
    }
}
