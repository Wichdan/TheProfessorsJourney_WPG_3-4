using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCharCollider : MonoBehaviour
{
    [SerializeField] string tagName;
    
    private void Start()
    {
        GameObject target = GameObject.FindGameObjectWithTag(tagName);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), target.GetComponent<Collider2D>());
    }
}
