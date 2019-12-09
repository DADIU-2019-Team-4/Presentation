using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SlideController : MonoBehaviour
{
    public const float BurnDuration = 1.0f;
    public const string VideoLabel = "VIDEO";
    public Material MaterialPrefab;

    private Image _imageComponent;
    private VideoPlayer _videoPlayer;

    void Awake()
    {
        // Validate
        _imageComponent = GetComponent<Image>();
        _videoPlayer = Camera.main.GetComponent<VideoPlayer>();
        if (!_videoPlayer) throw new System.Exception("SlideController: No videoPlayer found");
        if (!_imageComponent) throw new System.Exception("SlideController: Unable to find \"Image\" component");
        if (!MaterialPrefab) throw new System.Exception("SlideController: \"MaterialPrefab\" not given");
    }

    public void LoadSprite(Sprite sprite)
    {
        // We must create a new instance of the Material, since the mainTexture should be unique
        _imageComponent.sprite = sprite;
        _imageComponent.preserveAspect = true;
        _imageComponent.material = new Material(MaterialPrefab);
        _imageComponent.material.SetTexture("_maintexture", _imageComponent.mainTexture);
    }

    public void Hide()
    {
        if (_videoPlayer.isPlaying)
            _videoPlayer.Stop();

        StartCoroutine(BurnCoroutine(true));
    }

    public void Show()
    {
        StartCoroutine(BurnCoroutine(false));

        if (_imageComponent.sprite.name == VideoLabel)
            _videoPlayer.Play();
    }

    private IEnumerator BurnCoroutine(bool willBurn)
    {
        // Set whether we burn or unburn the picture
        float from = willBurn ? 1f : 0f;
        float to = willBurn ? 0f : 1f;

        _imageComponent.enabled = willBurn;
        _imageComponent.material.SetFloat("_DissolveAmount", from);

        for (float timer = 0; timer < BurnDuration; timer += Time.unscaledDeltaTime)
        {
            _imageComponent.material.SetFloat("_DissolveAmount", Mathf.Lerp(from, to, timer / BurnDuration));
            Debug.Log(_imageComponent.material.GetFloat("_DissolveAmount"));
            yield return null;
        }

        _imageComponent.material.SetFloat("_DissolveAmount", to);
        _imageComponent.enabled = !willBurn;
    }
}
