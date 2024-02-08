using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectGenerationCheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Collider")){
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            other.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
