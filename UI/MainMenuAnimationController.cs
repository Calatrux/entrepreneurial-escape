using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuAnimationController : MonoBehaviour
{
    public Animator animator;
    public TextCrawler textCrawler;
    public float bufferAfterCrawl = 5f;
    public float mainMenuCrawlTime = 5f;

    public GameObject getReady;
    public GameObject mainMenu;
    public Animator crossfade;
    public GameObject ffButton;

    public void PlayGame()
    {
        animator.SetTrigger("Start");
        StartCoroutine(WaitForMainMenuCrawl());
    }

    void Update()
    {
        if (textCrawler.finished)
        {
            GetReady();
        }
    }

    IEnumerator WaitForMainMenuCrawl()
    {
        yield return new WaitForSeconds(mainMenuCrawlTime);
        ffButton.SetActive(true);
    }

    public void GetReady()
    {
        getReady.SetActive(true);
        ffButton.SetActive(false);
        mainMenu.SetActive(false);
        animator.SetTrigger("GetReady");
    }

    public void StartGame()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel(){
        crossfade.gameObject.SetActive(true);
        crossfade.SetBool("start", true);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Game");
        yield return new WaitForSeconds(0.5f);
        crossfade.gameObject.SetActive(false);
        crossfade.SetBool("start", false);
    }

    public void FastForward()
    {
        textCrawler.FastForward();
    }
}
