using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class EventManager : MonoBehaviour
{
    public static Action<int> triggerDialogueAction;
    public static Action<int> triggerTeleportAction;
    public static void StartTriggerDialogue(int triggerID) => triggerDialogueAction?.Invoke(triggerID);
    public static void StartTeleport(int tpID) => triggerTeleportAction?.Invoke(tpID);
}
