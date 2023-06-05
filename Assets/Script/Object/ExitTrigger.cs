using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player"){
            if(SceneChanger.instance != null){
                SceneChanger.instance.ChangeScene(0);
                MusicManager.instance.StopBGM();
            }
        }
    }
}
