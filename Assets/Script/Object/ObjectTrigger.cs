using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTrigger : MonoBehaviour
{
    [SerializeField] bool isAutoInteract = false;
    [SerializeField] bool isInteracted = false;
    
    public void DoInteract()
    {
        SwapInteracted();
        Debug.Log("Do Something!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (isAutoInteract)
                DoInteract();
        }
    }

    void SwapInteracted() => isInteracted = !isInteracted;
}
