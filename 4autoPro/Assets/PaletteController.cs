using Nicrom.PM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PaletteModifier paletteModifier;
    [SerializeField] private FlexibleColorPicker flexibleColorPicker;
    [SerializeField] private MeshFilter meshHolder;
    [SerializeField] private MeshRenderer materialHolder;
    [Header("Settings")]
    [SerializeField] private List<Material> materials;
    [SerializeField] private int currentMaterialIndex;
    [SerializeField] private int currentColorIndex;
    [Header("Validate")]
    [SerializeField] private bool nextMaterial;
    [SerializeField] private bool nextColor;


    private void GetFlexibleColorPicker()
    {
        flexibleColorPicker = FindObjectOfType<FlexibleColorPicker>();
    }

    private void GetPalette()
    {
        paletteModifier = GetComponent<PaletteModifier>();
    }

    private void GetMeshHolder()
    {
        meshHolder = GetComponent<MeshFilter>();
    }

    private void GetMaterialHolder()
    {
        materialHolder = GetComponent<MeshRenderer>();
    }

    public void ChangeColor(Color color)
    {
        paletteModifier.palettesList[0].cellsList[currentColorIndex].currentCellColor = color;
    }

    private void NextMaterial()
    {
        currentMaterialIndex++;
        if (currentMaterialIndex >= materials.Count)
        {
            currentMaterialIndex = 0;
        }
        materialHolder.material = materials[currentMaterialIndex];
        SetColor();
    }

    private void NextColor()
    {
        currentColorIndex++;
        if (currentColorIndex >= paletteModifier.palettesList[0].cellsList.Count)
        {
            currentColorIndex = 0;
        }
        SetColor();
    }

    private void SetColor()
    {
        flexibleColorPicker.SetColor(GetCurrentColor());
        ChangeColor(GetCurrentColor());
    }

    private Color GetCurrentColor()
    {
        return paletteModifier.palettesList[0].cellsList[currentColorIndex].currentCellColor;
    }

    private void Awake()
    {
        GetFlexibleColorPicker();
        GetMaterialHolder();
        GetMeshHolder();
        GetPalette();
    }

    private void Start()
    {
        SetColor();
    }

    private void OnValidate()
    {
        if (nextMaterial)
        {
            NextMaterial();
            nextMaterial = false;
        }
        if (nextColor)
        {
            NextColor();
            nextColor = false;
        }
    }
}
