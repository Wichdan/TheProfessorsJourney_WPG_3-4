using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    [SerializeField] int arenaId;
    [SerializeField] int maxNumOfEnemy;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player"){
            BattleManager.instance.SetArenaID(arenaId);
            BattleManager.instance.SetMaxEnemy(3);
            Destroy(gameObject);
        }
    }
}
