using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    bool alreadySpawnedEnemies = false;

    private void Start() {
        enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !alreadySpawnedEnemies){
            enemySpawner.SpawnEnemy(transform.parent, transform);
            alreadySpawnedEnemies = true;
        }
    }

    private void Update() {
        if (transform.childCount == 0 && alreadySpawnedEnemies){
            DestroyCorridors();
            Destroy(gameObject);
        }
    }

    void DestroyCorridors(){
        foreach (Transform child in transform.parent){
            if (child.gameObject.CompareTag("ClosedCorridors")){
                Destroy(child.gameObject);
            }
        }
    }
}
