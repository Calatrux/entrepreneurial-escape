using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    bool playerInRange;
    public Sprite openChest;
    bool opened = false;

    public GameObject[] powerups;
    public float[] powerupChance;
    public int avgNumPowerupsPerChest;
    public float powerupLaunchSpeed;

    
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

    private void Update() {
        if (playerInRange && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.RightControl)) && !opened){
            OpenChest();
            opened = true;
        }
    }

    void OpenChest(){
        GetComponent<Animator>().SetTrigger("Open");
        StartCoroutine(SpawnPowerup());
    }

    IEnumerator SpawnPowerup(){
        for (int i = 0; i < avgNumPowerupsPerChest; i++){
            Vector3 spawnOffset = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0).normalized * 2f; // Spawn at a distance of 2 units from the chest
            GameObject powerup = Instantiate(DeterminePowerup(), transform.position + spawnOffset, Quaternion.identity);
            
            // Animate the powerup moving away from the chest
            float speed = powerupLaunchSpeed;
            for (float t = 0; t < 1; t += Time.deltaTime / 0.5f) { // The powerup will move away for 0.5 seconds
                powerup.transform.position += spawnOffset * speed * Time.deltaTime;
                speed = Mathf.Lerp(powerupLaunchSpeed, 0, t); // Decrease speed over time
                yield return null;
            }
        }
    }

    GameObject DeterminePowerup(){
        int rand = Random.Range(1, 100);
        for (int i = 0; i < powerupChance.Length; i++){
            if (rand <= powerupChance[i]){
                GameObject powerup = powerups[i];
                return powerup;
            }
        }
        return null;
    }
}
