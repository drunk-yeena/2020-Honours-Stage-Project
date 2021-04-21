using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ObjectHandAttachment : MonoBehaviour
{
    private Interactable interactable;
    private bool throwing;
    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    private void OnHandHoverBegin(Hand hand)
    {
        
    }

    private void OnHandHoverEnd(Hand hand)
    {
        
    }

    private void HandHoverUpdate(Hand hand)
    {

        GrabTypes grabType = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabEnding(gameObject);

        if (interactable.attachedToHand == null && grabType != GrabTypes.None)
        {
            throwing = false;
            rigidbody = null;

            hand.AttachObject(gameObject, grabType);
            hand.HoverLock(interactable);
        }
        else if (isGrabEnding)
        {
            rigidbody = GetComponent<Rigidbody>();
            throwing = true;

            if (throwing)
            {
                Transform origin;
                if (interactable.transform != null)
                {
                    origin = interactable.transform;
                }
                else
                {
                    origin = interactable.transform.parent;
                }

                if (origin != null)
                {
                    rigidbody.velocity = origin.TransformVector(hand.GetTrackedObjectVelocity(0));
                    rigidbody.angularVelocity = origin.TransformVector(hand.GetTrackedObjectAngularVelocity(0) * 0.25f);
                }
                else
                {
                    rigidbody.velocity = hand.GetTrackedObjectVelocity(0);
                    rigidbody.angularVelocity = hand.GetTrackedObjectAngularVelocity(0) * 0.25f;
                }

                rigidbody.maxAngularVelocity = rigidbody.angularVelocity.magnitude;
                throwing = false;
            }

            hand.DetachObject(gameObject);
            hand.HoverUnlock(interactable);
        }
    }
}
