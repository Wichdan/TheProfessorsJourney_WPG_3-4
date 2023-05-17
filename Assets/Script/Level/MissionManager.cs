using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [System.Serializable]
    public struct Mission
    {
        public string missionName;
        [TextArea(1, 3)]
        public string objective;
        public enum ObjectiveType
        {
            TalkToNPC, DefeatEnemy, Escape
        }
        public ObjectiveType objectiveType;
        public int enemyTotal, npcTotal, escapeTotal;
        public GameObject[] closedDoor;
        public bool isClear;
        public MissionReward reward;
    }

    [System.Serializable]
    public struct MissionReward
    {
        public Weapon weaponGet;
        public int hpGet;
        public int defGet;
    }

    public List<Mission> theMission;
    [SerializeField] int enemyCount, npcCount, escapeCount;
    [SerializeField] int missionIndex;
    Character player;
    PlayerCombat playerCombat;
    [SerializeField] Cinemachine.CinemachineVirtualCamera cam;

    #region  singleton
    public static MissionManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
        StartMission();
    }

    public void StartMission()
    {
        UIManager.instance.UpdateObjectiveText(theMission[missionIndex].objective);
        for (int i = 0; i < theMission[missionIndex].closedDoor.Length; i++)
        {
            if (theMission[missionIndex].closedDoor[i] == null)
                break;
            else
                theMission[missionIndex].closedDoor[i].SetActive(true);
        }
        enemyCount = 0;
        npcCount = 0;
    }

    void Completion()
    {
        if (theMission == null) return;
        if (theMission[missionIndex].objectiveType == Mission.ObjectiveType.TalkToNPC)
        {
            if (npcCount >= theMission[missionIndex].npcTotal)
                Reward(theMission[missionIndex], theMission[missionIndex].reward);
        }
        else if(theMission[missionIndex].objectiveType == Mission.ObjectiveType.DefeatEnemy)
        {
            if (enemyCount >= theMission[missionIndex].enemyTotal)
            {
                Reward(theMission[missionIndex], theMission[missionIndex].reward);
                BattleManager.instance.FinishBattle();
                cam.Follow = player.transform;
            }
        }else if(theMission[missionIndex].objectiveType == Mission.ObjectiveType.Escape){
            if(escapeCount >= theMission[missionIndex].escapeTotal)
                Reward(theMission[missionIndex], theMission[missionIndex].reward);
        }
    }

    void Reward(Mission mission, MissionReward reward)
    {
        for (int i = 0; i < theMission[missionIndex].closedDoor.Length; i++)
        {
            if (theMission[missionIndex].closedDoor[i] == null)
                break;
            else
                theMission[missionIndex].closedDoor[i].SetActive(false);
        }

        if (reward.weaponGet != null)
            playerCombat.WeaponSlot.Add(reward.weaponGet);

        player.HealthPoint += reward.hpGet;
        player.Defense += reward.defGet;
        mission.isClear = true;
        UpdateMission();
    }

    public void SetEscapeCount(int value) => escapeCount = value; 

    void UpdateMission()
    {
        if (missionIndex >= theMission.Count - 1)
        {
            UIManager.instance.UpdateObjectiveText("All Clear!");
            return;
        }
        missionIndex++;
        StartMission();
    }

    public void AddNPCCount(int count)
    {
        npcCount += count;
    }

    public void AddEnemyCount(int count)
    {
        enemyCount += count;
    }

    private void Update()
    {
        Completion();
    }
}
