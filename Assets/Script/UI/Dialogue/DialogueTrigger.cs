using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue theDialogue;
    [SerializeField] bool isAutoInteract = false;
    [SerializeField] bool isInteracted = false;

    public void TriggerDialogue()
    {
        DialogueManager.instance.SetDialogue(theDialogue);
        DialogueManager.instance.StartDialogue();
    }

    public void StartDialogue()
    {
        if(!isInteracted) return;
        DialogueManager.instance.SetDialogue(theDialogue);
        DialogueManager.instance.StartDialogue();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (isAutoInteract)
                TriggerDialogue();

            DialogueManager.instance.SetDialogue(theDialogue);
        }
    }
}
