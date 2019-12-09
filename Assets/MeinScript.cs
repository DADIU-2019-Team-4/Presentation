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

    private void OnMouseDown()
    {
        if (videoPlayer.isPlaying)
            videoPlayer.Stop();

        index++;
        spriteRenderer.sprite = Presentation[index];

        if (spriteRenderer.sprite.name == "VIDEO")
            PlayVideo();
    }

    private void PlayVideo()
    {
        videoPlayer.Play();
    }
}
