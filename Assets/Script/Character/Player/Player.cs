using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    Vector2 movement;
    [SerializeField] Vector2 dodgePos;
    public Vector2 tempMove;

    [Header("Dodge")]
    [SerializeField] float dodgeCD = 0.5f;
    [SerializeField] float dodgeStaminaCost = 5f;

    [Header("Condition")]
    public bool isInteract;
    public bool isInteracting, isCanInteract;
    public bool isCaptured = false;
    [SerializeField] int releaseValue = 0, maxRelease = 6;

    [Header("Reference")]
    Rigidbody2D rb;
    Animator anim;
    Character stats;
    PlayerCombat playerCombat;
    [SerializeField] DialogueTrigger dialogueTrigger;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        stats = GetComponent<Character>();
        playerCombat = GetComponent<PlayerCombat>();
    }

    private void Update()
    {
        Move();
        Dodge();
        Interact();
        WhenCaptured();

        if (Input.GetKeyDown(KeyCode.P))
            stats.IsRegenHP = true;

        stats.Stamina = Regenerate(stats.Stamina, stats.MaxStamina, 1f);
        stats.Mana = Regenerate(stats.Mana, stats.MaxMana, 1f);
    }

    bool flipCondition(bool condition)
    {
        return !condition;
    }

    private void FixedUpdate()
    {
        if (stats.IsMove && stats.IsCanMove)
            rb.MovePosition(rb.position + movement * stats.MoveSpeed * Time.fixedDeltaTime);
        else if (stats.IsDodge)
            rb.MovePosition(rb.position + dodgePos * stats.DodgeSpeed * Time.fixedDeltaTime);
    }

    void Move()
    {
        if (!stats.IsCanMove || isInteracting || isCaptured) return;

        if (!stats.IsAttack || isInteracting)
            movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        movement.Normalize();

        if (movement != Vector2.zero)
        {
            anim.SetFloat("moveX", movement.x);
            anim.SetFloat("moveY", movement.y);
            stats.IsMove = true;

            tempMove = movement;

            if (playerCombat.weaponAnim != null)
            {
                playerCombat.weaponAnim.SetFloat("moveX", movement.x);
                playerCombat.weaponAnim.SetFloat("moveY", movement.y);
            }
        }
        else
            stats.IsMove = false;

        anim.SetFloat("speed", movement.sqrMagnitude);
    }

    void Dodge()
    {
        if (stats.Stamina - dodgeStaminaCost <= 0) return;
        if (Input.GetKeyDown(KeyCode.LeftShift) && !stats.IsDodge && stats.IsMove)
        {
            dodgePos = movement;
            stats.IsDodge = true;
            stats.IsInvincible = true;
            stats.DisableMove();
            anim.SetBool("isDodge", stats.IsDodge);
            StartCoroutine(DisableDodge());
            stats.DecreaseStamina(dodgeStaminaCost);
        }
    }

    IEnumerator DisableDodge()
    {
        yield return new WaitForSeconds(dodgeCD);
        stats.IsDodge = false;
        stats.IsCanMove = true;
        anim.SetBool("isDodge", stats.IsDodge);
    }

    void Interact()
    {
        if (isInteracting) return;
        if (Input.GetKeyDown(KeyCode.Z) && !isInteract && isCanInteract)
        {
            stats.DisableMove();
            isInteract = true;
            StartCoroutine(DisableInteract());
            DialogueManager.instance.StartDialogue();
            isInteracting = true;
            MissionManager.instance.AddNPCCount(1, dialogueTrigger);
            //Debug.Log("Interact!");
        }
    }

    IEnumerator DisableInteract()
    {
        yield return new WaitForSeconds(0.3f);
        isInteract = false;
        stats.IsCanMove = true;
    }

    float Regenerate(float value, float maxValue, float time)
    {
        if (value >= maxValue)
            value = maxValue;
        else
            value += Time.deltaTime * time;

        return value;
    }

    void WhenCaptured()
    {
        if (!isCaptured) return;
        if (releaseValue >= maxRelease)
        {
            isCaptured = false;
            releaseValue = 0;
            return;
        }
        if (Input.GetKeyDown(KeyCode.Z) && isCaptured)
            releaseValue++;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Interacable")
        {
            isCanInteract = true;
            dialogueTrigger = other.GetComponent<DialogueTrigger>();
            if (dialogueTrigger == null) return;
            DialogueManager.instance.SetDialogue(dialogueTrigger.theDialogue);
        }

        if (other.gameObject.tag == "EnemyWorld")
        {
            BattleTrigger bt = other.gameObject.GetComponent<BattleTrigger>();
            bt.StartBattle();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Interacable")
        {
            isCanInteract = false;
        }
    }
}
