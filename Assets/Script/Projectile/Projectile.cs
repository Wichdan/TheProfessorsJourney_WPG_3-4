using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] Vector2 pos;

    public void SetProjectilePos(Vector2 _pos)
    {
        pos = _pos;
    }

    private void Update()
    {
        transform.position += new Vector3(pos.x, pos.y, transform.position.z) * moveSpeed * Time.deltaTime;
        Destroy(gameObject, 3f);
    }
}
