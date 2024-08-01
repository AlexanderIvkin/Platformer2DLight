using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Ground : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    public float Height { get; private set; }
    public float Width { get; private set; }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        Height = _spriteRenderer.sprite.textureRect.height;
        Width = _spriteRenderer.sprite.textureRect.width;
    }
}
