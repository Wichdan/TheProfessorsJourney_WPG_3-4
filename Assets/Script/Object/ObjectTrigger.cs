using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTrigger : MonoBehaviour
{
    [SerializeField] bool isAutoInteract = false;
    [SerializeField] bool isInteracted = false;
    bool hasInteract;
    Player player;

    private void Update()
    {
        if (isInteracted)
        {
            Debug.Log("Do Something!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject.GetComponent<Player>();
            if (isAutoInteract)
                SwapInteracted();
        }
    }

    void SwapInteracted() => isInteracted = !isInteracted;

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
            player = null;
    }
}
