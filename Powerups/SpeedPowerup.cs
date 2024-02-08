using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerup : Powerup
{
    public float speedIncrement;
    public override void PowerupEffect(){
        PlayerController playerController = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerController>();
        playerController.moveSpeed += speedIncrement;
    }
}
