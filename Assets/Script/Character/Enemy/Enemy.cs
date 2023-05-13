using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] Vector2 movement;
    [SerializeField] Vector2 chasePos;
    [SerializeField] float partrolDelay = 1f;
    float tempPatrolCD;
    [Header("Condition")]
    [SerializeField] bool isPatrol;
    [SerializeField] bool isPatroling, isChase;

    [Header("Attack Config")]
    [SerializeField] Transform atkPoint;
    [SerializeField] float radius;
    [SerializeField] LayerMask hitTarget;

    [Header("Weapon")]
    [SerializeField] Weapon weapon;

    [Header("Reference")]
    Animator charAnim, weaponAnim;
    Rigidbody2D rb;
    GameObject player;
    Transform playerPos;
    Character stats;
    [SerializeField] Collider2D feetCollider;
    [SerializeField] bool isMission;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        charAnim = GetComponent<Animator>();
        stats = GetComponent<Character>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform;
        Physics2D.IgnoreCollision(feetCollider, player.GetComponent<Collider2D>());

        if (weapon != null)
        {
            GameObject enemyWeapon = Instantiate(weapon.theWeapon, transform);
            weaponAnim = enemyWeapon.GetComponent<Animator>();
            atkPoint = enemyWeapon.transform;
            radius = weapon.radius;

            stats.Attack += weapon.bonusAttack;
            stats.AttackSpeed -= weapon.attackSpeed;
        }

        tempPatrolCD = partrolDelay;
    }

    private void Update()
    {
        if (stats.HealthPoint <= 0){
            Destroy(gameObject);
            if(isMission)
                MissionManager.instance.AddEnemyCount(1);
        }

        FacingPlayer();

        if (!stats.IsCanMove)
        {
            partrolDelay = tempPatrolCD;
            isPatroling = false;
            return;
        }
        if (!isPatrol) return;
        partrolDelay -= Time.deltaTime;
        if (partrolDelay <= 0)
        {
            partrolDelay = tempPatrolCD;
            FlipPatrol();
            if (isPatroling)
                PatrolMove();
        }
    }

    private void FixedUpdate()
    {
        if (isPatrol && isPatroling && !isChase && stats.IsCanMove)
            rb.MovePosition(rb.position + movement * stats.MoveSpeed * Time.fixedDeltaTime);
        else if (!isPatrol && !isPatroling && isChase && stats.IsCanMove)
            rb.MovePosition(rb.position + chasePos * stats.MoveSpeed * Time.fixedDeltaTime);
        else if (!isPatroling)
        {
            rb.velocity = new Vector2(0, 0);
            charAnim.SetFloat("speed", 0);
        }
    }

    void PatrolMove()
    {
        if (!isPatroling || isChase || !stats.IsCanMove) return;
        movement.x = Random.Range(-1, 2);
        movement.y = Random.Range(-1, 2);

        movement.Normalize();
        if (movement != Vector2.zero)
        {
            charAnim.SetFloat("moveX", movement.x);
            charAnim.SetFloat("moveY", movement.y);

            stats.IsMove = true;
            if (weaponAnim != null)
            {
                weaponAnim.SetFloat("moveX", movement.x);
                weaponAnim.SetFloat("moveY", movement.y);
            }
        }
        else
            stats.IsMove = false;

        charAnim.SetFloat("speed", movement.sqrMagnitude);
    }

    void FacingPlayer()
    {
        float distance = Vector2.Distance(transform.position, playerPos.position);

        if (distance < 2 && !stats.IsStun)
        {
            isChase = true;
            isPatrol = false;
            isPatroling = false;
            partrolDelay = 1f;

            Vector2 facing = (transform.position - playerPos.position).normalized;
            chasePos = -facing;
            //Debug.Log(facing);
            charAnim.SetFloat("moveX", chasePos.x);
            charAnim.SetFloat("moveY", chasePos.y);

            Vector2 weaponPos = new Vector2(Mathf.Round(chasePos.x), Mathf.Round(chasePos.y));

            weaponAnim.SetFloat("moveX", weaponPos.x);
            weaponAnim.SetFloat("moveY", weaponPos.y);

            charAnim.SetFloat("speed", chasePos.sqrMagnitude);

        }
        else
        {
            isChase = false;
            isPatrol = true;
        }

        if (distance <= 1.5 && !stats.IsAttack && !stats.IsStun)
        {
            isChase = false;
            Attack();
        }
    }

    void Attack()
    {
        stats.Attacking();
        weaponAnim.SetTrigger("attack");
        SpawnHitBox();
        stats.DisableMove();
    }

    void SpawnHitBox()
    {
        Collider2D col = Physics2D.OverlapCircle(atkPoint.position, radius, hitTarget);
        if (col != null)
        {
            Character player = col.GetComponent<Character>();
            player.TakeDamage(stats.Attack);
        }
    }

    void FlipPatrol() => isPatroling = !isPatroling;
}
