using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePowerup : Powerup
{
    public float damageIncrement;

    public override void PowerupEffect(){
        WeaponParent playerWeapon = GameObject.FindGameObjectsWithTag("Player")[0].GetComponentInChildren<WeaponParent>();
        playerWeapon.damage += damageIncrement;
    }
}
