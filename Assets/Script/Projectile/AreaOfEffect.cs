using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : MonoBehaviour
{
    public enum AOEType{
        Burn, Stun, Blind
    }

    public AOEType type;

    private void Update()
    {
        
        Destroy(gameObject, 3f);
    }
}
