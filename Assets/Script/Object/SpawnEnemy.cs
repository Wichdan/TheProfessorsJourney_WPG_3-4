using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] GameObject[] enemy;
    //[SerializeField] int numSpawn = 3;
    [SerializeField] bool isMission;
    private void Reset()
    {
        //numSpawn = 3;
    }

    private void Start()
    {
        for (int i = 0; i < enemy.Length; i++)
        {
            GameObject obj = Instantiate(enemy[i], transform);
            Enemy enm = obj.GetComponent<Enemy>();
            enm.IsMission = isMission;
        }
    }
}
