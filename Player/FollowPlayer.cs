using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        cam.Follow = GameObject.FindGameObjectWithTag("Player").transform;

    }
}
