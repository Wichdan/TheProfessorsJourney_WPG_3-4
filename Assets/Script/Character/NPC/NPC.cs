using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("NPC Settings")]
    [SerializeField] Vector2 movement;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float partrolDelay = 1f;
    float tempPatrolCD;
    [SerializeField] bool isPatrol, isMove, isFacingPlayer;
    Animator charAnim;
    Rigidbody2D rb;
    GameObject player;
    Transform playerPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        charAnim = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform;
        
        tempPatrolCD = partrolDelay;
    }

    private void Update()
    {
        FacingPlayer();

        if (!isPatrol) return;
        partrolDelay -= Time.deltaTime;
        if (partrolDelay <= 0)
        {
            partrolDelay = tempPatrolCD;
            FlipMove();
            if (isMove)
                PatrolMove();
        }
    }

    private void FixedUpdate()
    {
        if (isPatrol && isMove)
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        else
        {
            rb.velocity = new Vector2(0, 0);
            charAnim.SetFloat("speed", 0);
        }
    }

    void PatrolMove()
    {
        if (!isMove) return;
        movement.x = Random.Range(-1, 2);
        movement.y = Random.Range(-1, 2);

        movement.Normalize();
        if (movement != Vector2.zero)
        {
            charAnim.SetFloat("moveX", movement.x);
            charAnim.SetFloat("moveY", movement.y);
        }

        charAnim.SetFloat("speed", movement.sqrMagnitude);
    }

    void FacingPlayer()
    {
        if(!isFacingPlayer) return;
        float distance = Vector2.Distance(transform.position, playerPos.position);

        if (distance < 2)
        {
            Vector2 facing = (transform.position - playerPos.position).normalized;
            //Debug.Log(facing);
            charAnim.SetFloat("moveX", -facing.x);
            charAnim.SetFloat("moveY", -facing.y);
        }
    }

    void FlipMove() => isMove = !isMove;
}
