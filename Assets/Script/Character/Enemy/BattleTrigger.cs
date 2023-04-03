using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    [SerializeField] GameObject battleArena;
    [SerializeField] Transform camFocus;
    [SerializeField] Cinemachine.CinemachineVirtualCamera cinemachine;
    [SerializeField] GameObject player;

    public void StartBattle()
    {
        cinemachine.Follow = camFocus;
        battleArena.SetActive(true);
        Destroy(gameObject);
        player.transform.position = camFocus.position;
    }
}
