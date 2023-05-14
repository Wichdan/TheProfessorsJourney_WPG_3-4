using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue theDialogue;
    [SerializeField] int triggerID;

    private void Start()
    {
        EventManager.triggerDialogueAction += TriggerDialogue;
    }

    public void TriggerDialogue(int _triggerID)
    {
        if (triggerID == _triggerID)
        {
            DialogueManager.instance.SetDialogue(theDialogue);
            DialogueManager.instance.StartDialogue();
        }
    }

    private void OnDisable()
    {
        EventManager.triggerDialogueAction -= TriggerDialogue;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            EventManager.StartTriggerDialogue(triggerID);
            DialogueManager.instance.SetDialogue(theDialogue);
        }
    }
}
