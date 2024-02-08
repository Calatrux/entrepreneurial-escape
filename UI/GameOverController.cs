using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameOverController : MonoBehaviour
{
    
    public PlayerController playerController;
    public bool playerIsAlive = true;
    [Header("UI Elements")]
    public GameObject gameOverScreen;
    public GameObject hearts;
    public GameObject minimap;
    public GameObject timer;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerIsAlive = true;
        gameOverScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        playerIsAlive = playerController.playerAlive;
        if (playerIsAlive == false){
            SetGameOverScreenActive();
        }

    }

    public void SetGameOverScreenActive()
    {
        gameOverScreen.SetActive(true);
        hearts.SetActive(false);
        timer.SetActive(false);
        minimap.SetActive(false);
    }

    public void LaunchMainMenu()
    {
        Destroy(playerController.gameObject);
        Destroy(GameObject.FindGameObjectWithTag("GameManager"));
        SceneManager.LoadScene("MainMenu");
    }
}
