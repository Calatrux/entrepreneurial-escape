using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerup : Powerup
{
    public int healthIncrement;

    public override void PowerupEffect(){
        PlayerController player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerController>();
        player.currentHealth += healthIncrement;
        player.currentHealth = Mathf.Clamp(player.currentHealth, 0, player.maxHealth);
    }

}
