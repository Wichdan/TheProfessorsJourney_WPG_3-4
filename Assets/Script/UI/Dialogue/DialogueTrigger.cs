using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue theDialogue;
    //public bool isMission;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player"){
            DialogueManager.instance.SetDialogue(theDialogue);
        }
    }
}
