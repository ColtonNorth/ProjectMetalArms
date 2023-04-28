using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class Interactable : MonoBehaviour
{
    //Add or remove an InteractionEvent component to this game object.
    public bool useEvents;
    public string promptMessage;

    public void BaseInteract()
    {
        if(useEvents)
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        Interact();
    }
    
    protected virtual void Interact()
    {
        //This code is to be overridden by our subclasses for interactable objects.
    }
}
