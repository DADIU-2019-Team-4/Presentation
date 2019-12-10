using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MeinScript : MonoBehaviour
{
    public float BurnDuration = 1.0f;

    public Object[] Presentation;
    public GameObject[] Slide;

    private SpriteRenderer[] spriteRenderer;
    private VideoPlayer videoPlayer;

    private int index = 0;
    private int _currentSlide;
    private int _nextSlide;
    private AudioSource _fireDissolveSFX;

    private void Awake()
    {
        videoPlayer = Camera.main.GetComponent<VideoPlayer>();

        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        _fireDissolveSFX = GetComponent<AudioSource>();
        _currentSlide = 0;
        _nextSlide = 1;

        if (Presentation[0] is Texture2D)
            spriteRenderer[0].sprite = TextureToSprite(Presentation[0] as Texture2D);
        if (Presentation[1] is Texture2D)
            spriteRenderer[1].sprite = TextureToSprite(Presentation[1] as Texture2D);
    }

    private void Start()
    {
        if (Presentation[0] is VideoClip)
        {
            videoPlayer.clip = (Presentation[0] as VideoClip);
            videoPlayer.Play();
            SetSlidesActive(false);
        }
    }

    private Sprite TextureToSprite(Texture2D texture2D) { return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f)); }

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
        if (Presentation[index] is Texture2D)
            _fireDissolveSFX.Play();

        if (videoPlayer.isPlaying)
            videoPlayer.Stop();

        SetSlidesActive(true);
        StartCoroutine(Dissolve(spriteRenderer[_currentSlide].material, leftClick));
    }

    IEnumerator Dissolve(Material material, bool click)
    {
        if (click)
        {
            index++;

        }
        else
        {
            index--;
            if (Presentation[index] is Texture2D)
            {
                spriteRenderer[_nextSlide].sprite = TextureToSprite(Presentation[index] as Texture2D);
                spriteRenderer[_nextSlide].material.SetTexture("_maintexture", spriteRenderer[_nextSlide].sprite.texture);
            }
        }


        for (float timer = 0; timer < BurnDuration; timer += Time.unscaledDeltaTime)
        {
            material.SetFloat("_DissolveAmount", Mathf.Lerp(1f, 0f, timer / BurnDuration));
            yield return null;
        }

        SwitchPosition();

        SetCurrentSlide(click);

        if (Presentation[index] is VideoClip)
        {
            videoPlayer.clip = (Presentation[index] as VideoClip);
            videoPlayer.Play();
            SetSlidesActive(false);
            yield return null;
        }

        if (Presentation[index + 1] is VideoClip)
            spriteRenderer[_nextSlide].sprite = null;

        if (click && Presentation[index + 1] is Texture2D)
        {
            spriteRenderer[_nextSlide].sprite = TextureToSprite(Presentation[index + 1] as Texture2D);
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
