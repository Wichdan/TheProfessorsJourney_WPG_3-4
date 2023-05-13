using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] int arenaID;
    [SerializeField] GameObject[] battleArena;
    [SerializeField] Transform[] camFocus;
    Cinemachine.CinemachineVirtualCamera cinemachine;
    GameObject player;

    public static BattleManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cinemachine = GameObject.Find("CM vcam1").GetComponent<Cinemachine.CinemachineVirtualCamera>();
    }

    public void StartBattle()
    {
        cinemachine.Follow = camFocus[arenaID];
        battleArena[arenaID].SetActive(true);
        player.transform.position = camFocus[arenaID].position;
    }

    public void FinishBattle(){
        for (int i = 0; i < battleArena.Length; i++)
        {
            battleArena[i].SetActive(false);
        }
    }

    public void SetArenaID(int id) => arenaID = id;
}
