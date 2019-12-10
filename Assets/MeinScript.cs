using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MeinScript : MonoBehaviour
{
    public float BurnDuration = 1.0f;

    public Sprite[] Presentation;
    public GameObject[] Slide;

    private SpriteRenderer[] spriteRenderer;
    private VideoPlayer videoPlayer;

    private int index = 0;
    private int _currentSlide;
    private int _nextSlide;
    private AudioSource _fireDissolveSFX;

    private void Awake()
    {
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        videoPlayer = Camera.main.GetComponent<VideoPlayer>();
        _fireDissolveSFX = GetComponent<AudioSource>();
        _currentSlide = 0;
        _nextSlide = 1;
        for (int i = 0; i <= spriteRenderer.Length - 1; i++)
            spriteRenderer[i].sprite = Presentation[i];
    }

    private void Update()
    {
        // Next slide: Left mouse button OR Right arrow
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.RightArrow))
            TransitionSlide(true);

        // Previous slide: Right mouse button OR Left arrow
        else if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.LeftArrow))
            TransitionSlide(false);
    }

    void TransitionSlide(bool leftClick)
    {
        // here burn
        if (spriteRenderer[_currentSlide].sprite.name != "VIDEO")
        {
            // PLAY AUDIO
            _fireDissolveSFX.Play();
        }

        if (videoPlayer.isPlaying)
            videoPlayer.Stop();
        SetSlidesActive(true);
        StartCoroutine(Dissolve(spriteRenderer[_currentSlide].material, leftClick));
    }

    private void PlayVideo() { videoPlayer.Play(); }

    IEnumerator Dissolve(Material material, bool click)
    {
        if (click)
            index++;
        else
        {
            index--;
            spriteRenderer[_nextSlide].sprite = Presentation[index];
            spriteRenderer[_nextSlide].material.SetTexture("_maintexture", spriteRenderer[_nextSlide].sprite.texture);
        }

        for (float timer = 0; timer < BurnDuration; timer += Time.unscaledDeltaTime)
        {
            material.SetFloat("_DissolveAmount", Mathf.Lerp(0, 1f, timer / BurnDuration));
            yield return null;
        }

        SwitchPosition();

        SetCurrentSlide(click);

        if (spriteRenderer[_currentSlide].sprite.name == "VIDEO")
        {
            yield return null;
            PlayVideo();
            SetSlidesActive(false);
        }

        if (!videoPlayer.isPlaying && click)
        {
            spriteRenderer[_nextSlide].sprite = Presentation[index + 1];
            spriteRenderer[_nextSlide].material.SetTexture("_maintexture", spriteRenderer[_nextSlide].sprite.texture);
        }
        material.SetFloat("_DissolveAmount", 0);
    }

    private void SetSlidesActive(bool b)
    {
        foreach (GameObject o in Slide)
            o.SetActive(b);
    }

    private void SetCurrentSlide(bool click)
    {
        var tempNumb = _nextSlide;
        _nextSlide = _currentSlide;
        _currentSlide = tempNumb;
    }

    private void SwitchPosition()
    {
        var temp = Slide[_nextSlide].transform.position;
        Slide[_nextSlide].transform.position = Slide[_currentSlide].transform.position;
        Slide[_currentSlide].transform.position = temp;
    }
}
