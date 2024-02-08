using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TextCrawler : MonoBehaviour
{
    public float scrollSpeed = 20f;
    public bool finished;
    public float storylineScrollDistance = 1500f;
    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    
    void Update()
    {
        transform.Translate(Camera.main.transform.up * scrollSpeed * Time.deltaTime);

        if (rectTransform.anchoredPosition.y > storylineScrollDistance)
        {
            finished = true;
        }
    }

    public void FastForward()
    {
        scrollSpeed *= 7f;
    }
}
