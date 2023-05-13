using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static Action triggerDialogueAction;

    public static void StartDialogueEvent()
    {
        //if(triggerDialogueAction != null)
            //triggerDialogueAction();
        triggerDialogueAction?.Invoke();
    }
}
