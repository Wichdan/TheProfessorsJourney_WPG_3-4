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
		[TextArea(1,3)]
		public string objective;
		public enum ObjectiveType
		{
			TalkToNPC, DefeatEnemy
		}
		public ObjectiveType objectiveType;
		public int enemyTotal, npcTotal;
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
	[SerializeField] int enemyCount, npcCount;
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
			theMission[missionIndex].closedDoor[i].SetActive(true);
		enemyCount = 0;
		npcCount = 0;
	}

	void Completion()
	{
		if (theMission == null) return;
		if (theMission[missionIndex].objectiveType == Mission.ObjectiveType.TalkToNPC)
		{
			if(npcCount >= theMission[missionIndex].npcTotal)
				Reward(theMission[missionIndex], theMission[missionIndex].reward);
		}
		else
		{
			if (enemyCount >= theMission[missionIndex].enemyTotal){
				Reward(theMission[missionIndex], theMission[missionIndex].reward);
				BattleManager.instance.FinishBattle();
				cam.Follow = player.transform;
			}
		}
	}

	void Reward(Mission mission ,MissionReward reward)
	{
		for (int i = 0; i < theMission[missionIndex].closedDoor.Length; i++)
			theMission[missionIndex].closedDoor[i].SetActive(false);

		if (reward.weaponGet != null)
			playerCombat.WeaponSlot.Add(reward.weaponGet);

		player.HealthPoint += reward.hpGet;
		player.Defense += reward.defGet;
		mission.isClear = true;
		UpdateMission();
	}

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
