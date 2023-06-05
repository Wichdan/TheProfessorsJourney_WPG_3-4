using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack Config")]
    [SerializeField] Transform atkPoint;
    [SerializeField] float radius;
    [SerializeField] LayerMask hitTarget;

    [Header("Weapons")]
    //[SerializeField] Weapon weapon;
    [SerializeField] List<Weapon> weaponSlot;
    [SerializeField] List<GameObject> theWeapon;
    [SerializeField] int selectedIndex;
    //[SerializeField] Image weaponIcon;

    [Header("Skill")]
    [SerializeField] float skillCD;
    [SerializeField] float skill1ManaCost = 5, skill2ManaCost = 3;
    [SerializeField] bool isUseSkill;

    [Header("Projectile")]
    [SerializeField] GameObject[] projectiles;

    [Header("Reference")]
    Character stats;
    public Animator weaponAnim;
    float tempAtkSpeed;
    int tempAtk;
    Player player;

    public List<Weapon> WeaponSlot { get => weaponSlot; set => weaponSlot = value; }

    private void Start()
    {
        stats = GetComponent<Character>();
        player = GetComponent<Player>();

        tempAtk = stats.Attack;
        tempAtkSpeed = stats.AttackSpeed;

        for (int i = 0; i < weaponSlot.Count; i++)
        {
            if (weaponSlot == null) break;
            theWeapon.Add(weaponSlot[i].theWeapon);
            theWeapon[i] = Instantiate(weaponSlot[i].theWeapon, transform);
        }
        ActiveWeapon();
    }

    private void Update()
    {
        Attack();
        ChooseWeapon();
        Skill();
    }

    void Attack()
    {
        if (player.isCaptured) return;
        if (stats.Stamina - weaponSlot[selectedIndex].staminaCost <= 0) return;
        if (Input.GetKeyDown(KeyCode.X) && !stats.IsAttack && !stats.IsStun)
        {
            if(MusicManager.instance != null)
                MusicManager.instance.SetAndPlaySFX(weaponSlot[selectedIndex].sfx);
            
            stats.Attacking();
            weaponAnim.SetTrigger("attack");
            stats.DecreaseStamina(weaponSlot[selectedIndex].staminaCost);
            SpawnHitBox();
        }
    }

    void SpawnHitBox()
    {
        Collider2D[] col =
        Physics2D.OverlapCircleAll(atkPoint.position, radius, hitTarget);

        foreach (var hit in col)
        {
            if (hit != null)
            {
                Character enemy = hit.GetComponent<Character>();
                enemy.TakeDamage(stats.Attack);

                if (enemy.HealthPoint <= 0)
                {
                    stats.Defense += 1;
                    //MissionManager.instance.AddEnemyCount(1);
                }
            }
        }
    }

    void Skill()
    {
        if (isUseSkill) return;
        if (Input.GetKeyDown(KeyCode.C) && skill1ManaCost - stats.Mana <= 0)
        {
            isUseSkill = true;
            SpawnAOE(0);
            StartCoroutine(DisableSkill());
            stats.DecreaseMana(skill1ManaCost);
        }
        else if (Input.GetKeyDown(KeyCode.V) && skill2ManaCost - stats.Mana <= 0)
        {
            isUseSkill = true;
            SpawnAOE(1);
            StartCoroutine(DisableSkill());
            stats.DecreaseMana(skill2ManaCost);
        }
    }

    void SpawnAOE(int index)
    {
        stats.IsImmuneEffect = true;
        GameObject aoe = Instantiate(projectiles[index], transform);
        aoe.transform.parent = null;
    }

    IEnumerator DisableSkill()
    {
        yield return new WaitForSeconds(skillCD);
        isUseSkill = false;
    }

    void ChooseWeapon()
    {
        if (stats.IsAttack) return;
        if (Input.GetKeyDown(KeyCode.A))
        {
            selectedIndex--;
            ActiveWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            selectedIndex++;
            ActiveWeapon();
        }

        if (selectedIndex >= weaponSlot.Count - 1)
            selectedIndex = weaponSlot.Count - 1;
        else if (selectedIndex <= 0)
            selectedIndex = 0;
    }

    void ActiveWeapon()
    {
        if (selectedIndex < 0 || selectedIndex > weaponSlot.Count - 1) return;
        for (int i = 0; i < weaponSlot.Count; i++)
        {
            if (selectedIndex == i)
                theWeapon[i].SetActive(true);
            else
                theWeapon[i].SetActive(false);
        }
        ApplyWeapon();
    }

    void ApplyWeapon()
    {
        weaponAnim = theWeapon[selectedIndex].GetComponent<Animator>();
        atkPoint = theWeapon[selectedIndex].transform;
        radius = weaponSlot[selectedIndex].radius;

        stats.Attack = tempAtk;
        stats.AttackSpeed = tempAtkSpeed;

        stats.Attack += weaponSlot[selectedIndex].bonusAttack;
        stats.AttackSpeed -= weaponSlot[selectedIndex].attackSpeed;

        weaponAnim.SetFloat("moveX", player.tempMove.x);
        weaponAnim.SetFloat("moveY", player.tempMove.y);

        UIManager.instance.SetWeaponIcon(weaponSlot[selectedIndex].weaponIcon);
    }

    bool flipCondition(bool condition)
    {
        return !condition;
    }

    private void OnDrawGizmosSelected()
    {
        if (atkPoint != null)
        {
            Gizmos.DrawWireSphere(atkPoint.position, radius);
        }
    }
}
