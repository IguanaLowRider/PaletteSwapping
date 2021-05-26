using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PaletteSwap : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Material material;
    [SerializeField] private List<Color> colors;
    [SerializeField] private bool allowRuntimeUpdateChange;
    [SerializeField] private SourceType sourceType;

    private static readonly int PaletteTex = Shader.PropertyToID("_PaletteTex");

    private void Awake()
    {
        UpdatePalette();
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
            return;

        image = GetComponent<Image>();
        sprite = GetComponent<SpriteRenderer>();
        
        UpdatePalette();
    }

    private void Update()
    {
        if (!allowRuntimeUpdateChange)
            return;
        
        UpdatePalette();
    }

    public void UpdatePalette()
    {
        if (sourceType == SourceType.Image)
            image.material.SetTexture(PaletteTex, GetTexture());
        else
            sprite.sharedMaterial.SetTexture(PaletteTex, GetTexture());
    }

    private Texture2D GetTexture()
    {
        Texture2D newTexture = new Texture2D(colors.Count, 1, TextureFormat.RGBA32, false);
        for (int i = 0; i < colors.Count; i++)
        {
            newTexture.SetPixel(i, 0, colors[i]);
        }

        newTexture.filterMode = FilterMode.Point;
        newTexture.wrapMode = TextureWrapMode.Clamp;
        newTexture.Apply();

        return newTexture;
    }

    private enum SourceType
    {
        Image,
        Sprite,
    }
}
