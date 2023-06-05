using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] int arenaID;
    [SerializeField] int numOfEnemy;
    [SerializeField] int maxNumOfEnemy;
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
        numOfEnemy = 0;
        cinemachine.Follow = camFocus[arenaID];
        cinemachine.m_Lens.OrthographicSize = 7;
        battleArena[arenaID].SetActive(true);
        player.transform.position = camFocus[arenaID].position;

        if (MusicManager.instance != null)
            MusicManager.instance.SetAndPlayBGM(2);
    }

    public void FinishBattle()
    {
        if (numOfEnemy >= maxNumOfEnemy)
        {
            for (int i = 0; i < battleArena.Length; i++)
            {
                battleArena[i].SetActive(false);
            }
            cinemachine.m_Lens.OrthographicSize = 5;
            cinemachine.m_Follow = player.transform;

            if (MusicManager.instance != null)
                MusicManager.instance.SetAndPlayBGM(1);
        }
    }

    public void SetArenaID(int id) => arenaID = id;
    public void SetMaxEnemy(int enemy) => maxNumOfEnemy = enemy;
    public void UpdNumOfEnemy() => numOfEnemy++;
}
