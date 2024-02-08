using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAppearance : MonoBehaviour
{
    public GameObject square;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")){
            square.SetActive(true);
        }
    }
}
