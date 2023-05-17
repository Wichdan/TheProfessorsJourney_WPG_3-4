using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [SerializeField] Transform tpLocation;
    [SerializeField] bool isCamFocus, isDestoryAfterInteract;
    [SerializeField] PolygonCollider2D boundingBox;
    Cinemachine.CinemachineConfiner vmCam;
    private void Start()
    {
        vmCam = GameObject.Find("CM vcam1").GetComponent<Cinemachine.CinemachineConfiner>();
    }

    void ChangeCameraFocus()
    {
        if(!isCamFocus) return;
        vmCam.m_BoundingShape2D = boundingBox;
        if(isDestoryAfterInteract)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.position = tpLocation.transform.position;
            ChangeCameraFocus();
            if(isDestoryAfterInteract)
                Destroy(gameObject);
        }
    }
}
