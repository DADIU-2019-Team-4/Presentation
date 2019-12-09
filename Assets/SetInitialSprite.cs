using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInitialSprite : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.material.SetTexture("_maintexture", _spriteRenderer.sprite.texture);
    }


}
