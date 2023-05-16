using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;
    [SerializeField] bool isAutoInteract = false;
    
    void AutoDialogue()
    {
        DialogueManager.instance.SetDialogue(dialogue);
        DialogueManager.instance.StartDialogue();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(isAutoInteract)
                AutoDialogue();
            else
                DialogueManager.instance.SetDialogue(dialogue);
        }
    }
}
