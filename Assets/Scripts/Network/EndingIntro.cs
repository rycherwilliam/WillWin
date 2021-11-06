using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class EndingIntro : MonoBehaviour
{
    public GameObject TopPanel;
    public GameObject LoginPanel;
    private VideoPlayer video;

    private void Start()
    {
        video = GetComponent<VideoPlayer>();
    }

    private void Update()
    {
        if (video.isPaused)
        {
            TopPanel.gameObject.SetActive(true);
            LoginPanel.gameObject.SetActive(true); 
        }
    }
}
