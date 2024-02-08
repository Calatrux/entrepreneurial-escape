using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{

    public GameObject winScreenChild;
    public GameOverController gameOverController;
    public GameObject hearts;
    public GameObject minimap;
    public GameObject timer;

    // Start is called before the first frame update
    void Start()
    {
        winScreenChild.SetActive(false);
    }

    public void SetWinScreenActive()
    {
        winScreenChild.SetActive(true);
        hearts.SetActive(false);
        timer.SetActive(false);
        minimap.SetActive(false);
    }

    public void LaunchMainMenu()
    {
        gameOverController.LaunchMainMenu();
    }
}
