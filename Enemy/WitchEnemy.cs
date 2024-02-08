using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchEnemy : MonoBehaviour
{
    public int numChildren;
    public int timeBtwSpawns;
    public GameObject child;
    public Transform[] spawnPoints;
    Enemy enemy;

    private void Start() {
        enemy = GetComponent<Enemy>();
        StartCoroutine(WaitForSpawn());
    }

    IEnumerator WaitForSpawn(){
        yield return new WaitForSeconds(timeBtwSpawns);
        Spawn();
        StartCoroutine(WaitForSpawn());
    }

    void Spawn(){
        for (int i = 0; i < numChildren; i++){
            GameObject newChild = Instantiate(child, spawnPoints[i].position, Quaternion.identity);
            newChild.transform.SetParent(enemy.room.transform);
        }
    }
}
