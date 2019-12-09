using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MeinScript : MonoBehaviour
{
    public Sprite[] Presentation;
    private SpriteRenderer spriteRenderer;
    private VideoPlayer videoPlayer;

    private int index = 0;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        videoPlayer = Camera.main.GetComponent<VideoPlayer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            OnClick(Input.GetMouseButton(0));
    }

    void OnClick(bool leftClick)
    {
        if (videoPlayer.isPlaying)
            videoPlayer.Stop();
        if (leftClick)
            index++;
        else
            index--;
        spriteRenderer.sprite = Presentation[index];

        if (spriteRenderer.sprite.name == "VIDEO")
            PlayVideo();
    }

    private void PlayVideo()
    {
        videoPlayer.Play();
    }
}
