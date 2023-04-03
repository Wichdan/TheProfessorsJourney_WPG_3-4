using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int healthPoints = 100;
    [SerializeField] private int maxHealthPoints = 100, defense = 0, attack = 1;
    [SerializeField]
    private float attackSpeed = 2, stamina = 10f, mana = 10f,
    maxStamina = 10f, maxMana = 10f, invincibleCD = 1f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4;
    [SerializeField] private float dodgeSpeed = 8;

    [Header("Condition")]
    [SerializeField] private bool isAttack;
    [SerializeField] private bool isMove, isDodge, isInvincible, isCanMove, isCanAttack;

    [Header("DeBuff")]
    [SerializeField] private bool isStun;
    [SerializeField] private bool isBurn, isBlind, isImmuneEffect;
    [SerializeField] private float stunCD = 2f, burnCD = 2f, blindCD = 2f;
    [SerializeField] private float burnDelay = 1f, immuneCD = 3f;
    [SerializeField] int burnDMG = 10;
    float tempBurnDelay;

    [Header("Buff")]
    [SerializeField] private bool isRegenHP;
    [SerializeField] float regenCD = 3f, regenDelay = 1f;
    [SerializeField] int healValue = 10;
    float tempRegenDelay;

    Animator anim;
    SpriteMask mask;
    SpriteRenderer sr;

    [Header("Other")]
    [SerializeField] GameObject theMask;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        mask = GetComponent<SpriteMask>();
        sr = GetComponent<SpriteRenderer>();
        tempBurnDelay = burnDelay;
        tempRegenDelay = regenDelay;
    }

    public int HealthPoint { get => healthPoints; set => healthPoints = value; }
    public int Defense { get => defense; set => defense = value; }
    public int Attack { get => attack; set => attack = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float Stamina { get => stamina; set => stamina = value; }
    public float Mana { get => mana; set => mana = value; }
    public float MaxStamina { get => maxStamina; set => maxStamina = value; }
    public float MaxMana { get => maxMana; set => maxMana = value; }
    public float InvincibleCD { get => invincibleCD; set => invincibleCD = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public bool IsAttack { get => isAttack; set => isAttack = value; }
    public bool IsMove { get => isMove; set => isMove = value; }
    public bool IsDodge { get => isDodge; set => isDodge = value; }
    public bool IsInvincible { get => isInvincible; set => isInvincible = value; }
    public bool IsCanMove { get => isCanMove; set => isCanMove = value; }
    public bool IsCanAttack { get => isCanAttack; set => isCanAttack = value; }
    public bool IsStun { get => isStun; set => isStun = value; }
    public bool IsBurn { get => isBurn; set => isBurn = value; }
    public bool IsBlind { get => isBlind; set => isBlind = value; }
    public float DodgeSpeed { get => dodgeSpeed; set => dodgeSpeed = value; }
    public bool IsImmuneEffect { get => isImmuneEffect; set => isImmuneEffect = value; }
	public int MaxHealthPoints { get => maxHealthPoints; set => maxHealthPoints = value; }
	public bool IsRegenHP { get => isRegenHP; set => isRegenHP = value; }

	private void Update()
    {
        if (IsImmuneEffect)
        {
            Immune();
            return;
        }

        if (isBurn)
            Burn();

        if (isStun)
            Stun();

        if (isBlind)
            Blind();

        if (isRegenHP)
            RegenerateHP();

        if(regenDelay < 0)
        {
            regenDelay = tempRegenDelay;
			Heal(healValue);
		}

        if (burnDelay < 0)
        {
            burnDelay = tempBurnDelay;
            TakeDamage(burnDMG);
        }

        if (isInvincible && mask != null){
            mask.sprite = sr.sprite;
            theMask.SetActive(true);
            StartCoroutine(DisableInvicible());
        }
    }

    void RegenerateHP()
    {
        regenDelay -= Time.deltaTime;
        StartCoroutine(DisableRegeneration());
    }

    void Immune()
    {
        isBurn = false;
        isStun = false;
        isBlind = false;
        isCanMove = true;
        StartCoroutine(DisableImmune());
    }

    void Burn()
    {
		burnDelay -= Time.deltaTime;
		StartCoroutine(DisableBurn());
    }

    void Stun()
    {
        DisableMove();
        StartCoroutine(DisableStun());
    }

    void Blind()
    {
        StartCoroutine(DisableBlind());
    }

    IEnumerator DisableImmune()
    {
        yield return new WaitForSeconds(immuneCD);
        isImmuneEffect = false;
    }

    IEnumerator DisableStun()
    {
        yield return new WaitForSeconds(stunCD);
        isCanMove = true;
        isStun = false;
    }

    IEnumerator DisableBurn()
    {
        yield return new WaitForSeconds(burnCD);
        isBurn = false;
    }

    IEnumerator DisableBlind()
    {
        yield return new WaitForSeconds(blindCD);
        isBlind = false;
    }

    IEnumerator DisableInvicible()
    {
        yield return new WaitForSeconds(invincibleCD);
        isInvincible = false;
        theMask.SetActive(false);
    }

    IEnumerator DisableRegeneration()
    {
        yield return new WaitForSeconds(regenCD);
        isRegenHP = false;
    }

    public void Attacking()
    {
        if (isStun) return;
        DisableMove();
        anim.SetTrigger("attack");
        isAttack = true;
        StartCoroutine(AttackCD(attackSpeed));
    }

    public IEnumerator AttackCD(float time)
    {
        yield return new WaitForSeconds(time);
        isCanMove = true;
        isAttack = false;
    }

    public void DisableMove()
    {
        isCanMove = false;
        isMove = false;
        anim.SetFloat("speed", 0);
    }

    public void DecreaseStamina(float _stamina) => stamina -= _stamina;
    public void DecreaseMana(float _mana) => mana = _mana;

    public void TakeDamage(int dmg)
    {
        if(isInvincible) return;
        anim.SetTrigger("hurt");
        if (defense > 0)
            defense -= 1;
        else if (defense <= 0)
            healthPoints -= dmg;
    }

    public void Heal(int _heal)
    {
        if (healthPoints >= maxHealthPoints) return;
        healthPoints += _heal;
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "AOE")
        {
            AreaOfEffect aoe = other.GetComponent<AreaOfEffect>();
            if (aoe == null) return;

            if (aoe.type == AreaOfEffect.AOEType.Burn)
                isBurn = true;
            else if (aoe.type == AreaOfEffect.AOEType.Stun)
                isStun = true;
            else
                isBlind = true;
        }
    }
}
