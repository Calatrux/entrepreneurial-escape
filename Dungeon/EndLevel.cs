using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    private GameManager gameManager;
    public void Start(){
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")){
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            gameManager.advanceLevel();
        }
    }
}
