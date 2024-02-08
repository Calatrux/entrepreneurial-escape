using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies;
    public float[] enemySpawnChance;
    public GameObject enemy;
    public GameObject enemyRoom;
    public GameObject closedCorridors;
    public int avgNumEnemiesPerRoom;

    public void SpawnEnemy(Transform roomPosition, Transform enemyRoom){
        GameObject doors = Instantiate(closedCorridors, roomPosition.position, Quaternion.identity);
        doors.transform.SetParent(roomPosition);

        int numEnemies = Random.Range((int)Mathf.Round(avgNumEnemiesPerRoom / 1.5f), (int)Mathf.Round(avgNumEnemiesPerRoom * 1.5f));
        for (int i = 0; i < numEnemies; i++){
            Vector3 enemyPosition = new Vector3(Random.Range(-8, 8), Random.Range(-8, 8), 0);
            GameObject newEnemy = Instantiate(DetermineEnemy(), roomPosition.position + enemyPosition, Quaternion.identity);
            newEnemy.GetComponent<Enemy>().room = enemyRoom;
            newEnemy.transform.SetParent(enemyRoom);
            newEnemy.GetComponent<Enemy>().spawnDelay = i + 2.0f;
            newEnemy.GetComponent<Enemy>().Spawn();
        }
    }

    GameObject DetermineEnemy(){
        int rand = Random.Range(1, 100);
        for (int i = 0; i < enemySpawnChance.Length; i++){
            if (rand <= enemySpawnChance[i]){
                enemy = enemies[i];
                return enemy;
            }
        }
        return null;
    }
}
