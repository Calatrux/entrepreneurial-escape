using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DecomposingEnemy : Enemy
{
    public int maxDecompositions;
    public int numChildren;
    int currentDecompositions;
    public float proportionalChildrenHealth;
    public float proportionalChildrenSize;  
    public float proportionalChildrenSpeed;

    public override void Die(){
        Decompose(room);
    }

    public void Decompose(Transform room){
        currentDecompositions++;
        if (currentDecompositions > maxDecompositions){
            Destroy(gameObject);
            return;
        }
        for (int i = 0; i < numChildren; i++)
        {
            GameObject child = Instantiate(gameObject, transform.position, transform.rotation);
            
            child.GetComponent<Enemy>().maxEnemyHealth = maxEnemyHealth * proportionalChildrenHealth;
            child.transform.localScale = transform.localScale * proportionalChildrenSize;
            child.GetComponent<Enemy>().speed = speed * proportionalChildrenSpeed;

            child.GetComponent<DecomposingEnemy>().currentDecompositions = currentDecompositions;
            
            child.transform.SetParent(room);

            child.GetComponent<Enemy>().invulnerable = true;
            child.GetComponent<Enemy>().isKnockedBack = false;
        }

        Destroy(gameObject);
    }
}
