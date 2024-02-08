using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttributes : MonoBehaviour
{
    public float attackTimeBoost;
    public float healthBoost;
    public float speedBoost;

    public float currentAttackTimeMultiplier;
    public float currentHealthMultiplier;
    public float currentSpeedMultiplier;


    public void IncreaseDifficulty()
    {
        currentAttackTimeMultiplier *= attackTimeBoost;
        currentHealthMultiplier *= healthBoost;
        currentSpeedMultiplier *= speedBoost;
    }
}
