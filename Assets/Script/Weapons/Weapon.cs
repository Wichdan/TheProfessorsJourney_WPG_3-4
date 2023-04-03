using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/New Weapon", order = 1)]
public class Weapon : ScriptableObject
{
    public GameObject theWeapon;
    public string weaponName;
    public float radius;
    public int bonusAttack;
    public float attackSpeed, staminaCost, manaCost;
    public enum AttackType{
        melee, ranged, both
    }
    public AttackType weaponType;
    public GameObject projectile;
    public Sprite weaponIcon;
}
