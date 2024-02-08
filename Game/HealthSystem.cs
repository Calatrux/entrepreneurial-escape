using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public PlayerController playerController;
    public int maxHearts;
    public float currentHearts;

    public GameObject[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHearts = (float) playerController.currentHealth / 2;

        for (int i = 0; i < hearts.Length; i++){
            hearts[i].GetComponent<Image>().sprite = emptyHeart;
            
            if (currentHearts > 0.5){
                hearts[i].SetActive(true);
                hearts[i].GetComponent<Image>().sprite = fullHeart;

                currentHearts -= 1;
            }else if (currentHearts == 0.5){
                hearts[i].SetActive(true);

                hearts[i].GetComponent<Image>().sprite = halfHeart;
                currentHearts -= 0.5f;
            }
        }
    }
}
