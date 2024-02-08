using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int bulletDamage;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")){
            other.gameObject.GetComponent<PlayerController>().TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Walls")){
            Destroy(gameObject);
        }
    }
}
