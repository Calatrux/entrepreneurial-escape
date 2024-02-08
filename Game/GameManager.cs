using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject crossfade;
    public PlayerController player;
    public GameObject winScreen;
    public static GameManager instance;
    public int currentLevel;
    public int maxLevels;
    public bool won;

    public void Start()
    {
        // if (instance == null){
        //     instance = this;
        // } else {
        //     Destroy(gameObject);
        // }
        // DontDestroyOnLoad(gameObject);

        if (instance == null){
            instance = this;
        } else {
            instance.gameObject.GetComponent<Timer>().timerText = gameObject.GetComponent<Timer>().timerText;
            instance.gameObject.GetComponent<DifficultyManager>().difficultyText = gameObject.GetComponent<DifficultyManager>().difficultyText;

            instance.gameObject.GetComponent<DifficultyManager>().global = gameObject.GetComponent<DifficultyManager>().global;
            instance.gameObject.GetComponent<DifficultyManager>().globalLight = gameObject.GetComponent<DifficultyManager>().globalLight;
            Destroy(gameObject);

        }
        DontDestroyOnLoad(gameObject);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        crossfade = GameObject.FindGameObjectWithTag("Crossfade").transform.GetChild(0).gameObject;
        winScreen = GameObject.FindGameObjectWithTag("WinScreen");
    }

    public void advanceLevel(){
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        crossfade = GameObject.FindGameObjectWithTag("Crossfade").transform.GetChild(0).gameObject;
        winScreen = GameObject.FindGameObjectWithTag("WinScreen");
        currentLevel++;
        CheckLevel();
        if (!won) StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel(){
        crossfade.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        crossfade.SetActive(false);
        player.ResetPlayerPosition();
        yield return new WaitForSeconds(0.5f);
    }

    public void CheckLevel()
    {
        if (currentLevel == maxLevels)
        {
            winScreen.transform.parent.GetComponent<WinScreen>().SetWinScreenActive();
            won = true;
            player.playerWon = true;
        }
    }
}
