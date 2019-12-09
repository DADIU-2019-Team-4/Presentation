using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MeinScript : MonoBehaviour
{
    public float BurnDuration = 1.0f;

    public Sprite[] Presentation;
    public GameObject[] Slide;
    private int _currentSlide;
    private int _nextSlide;
    private SpriteRenderer[] spriteRenderer;
    private VideoPlayer videoPlayer;
    private int index = 1;

    private void Awake()
    {
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        videoPlayer = Camera.main.GetComponent<VideoPlayer>();
        _currentSlide = 0;
        _nextSlide = 1;
        for (int i = 0; i <= spriteRenderer.Length - 1; i++)
            spriteRenderer[i].sprite = Presentation[i];
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
        StartCoroutine(Dissolve(spriteRenderer[_currentSlide].material, leftClick));
    }

    private void PlayVideo()
    {
        videoPlayer.Play();
    }

    IEnumerator Dissolve(Material material, bool click)
    {

        for (float timer = 0; timer < BurnDuration; timer += Time.unscaledDeltaTime)
        {
            material.SetFloat("_DissolveAmount", Mathf.Lerp(0, 1f, timer / BurnDuration));
            yield return null;
        }
        if (click)
            index++;
        else
            index--;
        var temp = Slide[_nextSlide].transform.position;
        Slide[_nextSlide].transform.position = Slide[_currentSlide].transform.position;
        Slide[_currentSlide].transform.position = temp;
        var tempNumb = _nextSlide;
        _nextSlide = _currentSlide;
        _currentSlide = tempNumb;
        spriteRenderer[_nextSlide].sprite = Presentation[index];
        spriteRenderer[_nextSlide].material.SetTexture("_maintexture", spriteRenderer[_nextSlide].sprite.texture);
        material.SetFloat("_DissolveAmount", 0);
        if (spriteRenderer[_currentSlide].sprite.name == "VIDEO")
            PlayVideo();
    }
}
