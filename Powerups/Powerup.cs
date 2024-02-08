using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public bool playerInRange;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")){
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")){
            playerInRange = false;
        }
    }

    public void Update(){
        if (playerInRange && Input.GetKeyDown(KeyCode.E)){
            PowerupEffect();
            Destroy(gameObject);
        }
    }

    public virtual void PowerupEffect(){
        Debug.Log("Powerup effect not implemented");
    }
}
