using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [SerializeField] int tpID;
    [SerializeField] Transform tpLocation;

    GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        EventManager.triggerTeleportAction += Teleport;
    }

    public void Teleport(int _tpID)
    {
        if (tpID == _tpID)
        {
            player.transform.position = tpLocation.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            EventManager.StartTeleport(tpID);
        }
    }
}
