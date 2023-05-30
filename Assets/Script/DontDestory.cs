using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestory : MonoBehaviour
{
    [SerializeField] string objTag;
    private void Awake()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag(objTag);
        if(obj.Length > 1){
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
