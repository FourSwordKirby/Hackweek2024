using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class IconGenerator : MonoBehaviour
{
    [Header("References")]
    public GameObject iconPrefab;
    public TextMeshPro numberText;

    [Header("Icon Settings")]
    public float defaultScale = 0.4f;
    public float centerScale = 0.7f;
    public float largeScale = 0.9f;
    public float singleScale = 1f;
    
    private Transform iconsParent;

    void Start()
    {
        Initialize();
        UpdateIconsFromText();
    }

    private void Initialize()
    {
        numberText = numberText ?? GetComponentInChildren<TextMeshPro>();
        iconsParent = transform.Find("Icons") ?? CreateIconsParent();
    }

    public void UpdateIconsFromText()
    {
        ClearExistingIcons();
        
        if (!int.TryParse(numberText.text, out int number)) return;
        if (number == 0) return;

        GenerateIcons(number);
    }

    private Transform CreateIconsParent()
    {
        var parent = new GameObject("Icons").transform;
        parent.SetParent(transform);
        parent.localPosition = Vector3.zero;
        return parent;
    }

    private void ClearExistingIcons()
    {
        foreach (Transform child in iconsParent)
            Destroy(child.gameObject);
    }

    private void GenerateIcons(int number)
    {
        var layout = GetIconLayout(number);
        
        for (int i = 0; i < layout.positions.Length; i++)
        {
            CreateIcon(layout.positions[i], layout.scales[i]);
        }
    }

    private void CreateIcon(Vector3 position, Vector3 scale)
    {
        var icon = Instantiate(iconPrefab, iconsParent);
        icon.transform.localPosition = position;
        icon.transform.localScale = scale;
        icon.transform.localRotation = Quaternion.identity;
    }

    private (Vector3[] positions, Vector3[] scales) GetIconLayout(int number)
    {
        var positions = new List<Vector3>();
        var scales = new List<Vector3>();

        switch (number)
        {
            case 1:
                AddIcon(positions, scales, Vector3.zero, singleScale);
                break;
                
            case 2:
                AddIcon(positions, scales, new Vector3(0.4f, 0.4f, 0), largeScale);
                AddIcon(positions, scales, new Vector3(-0.4f, -0.4f, 0), largeScale);
                break;
                
            case 3:
                AddIcon(positions, scales, new Vector3(0.55f, 0.55f, 0), centerScale);
                AddIcon(positions, scales, Vector3.zero, centerScale);
                AddIcon(positions, scales, new Vector3(-0.55f, -0.55f, 0), centerScale);
                break;
                
            case 4:
                AddCornerIcons(positions, scales, 0.4f, 0.6f);
                break;
                
            case 5:
                AddIcon(positions, scales, Vector3.zero, 0.5f);
                AddCornerIcons(positions, scales, 0.45f, 0.5f);
                break;
                
            case 6:
                AddVerticalPair(positions, scales, 0.5f, 0.4f);
                AddVerticalPair(positions, scales, 0f, 0.4f);
                AddVerticalPair(positions, scales, -0.5f, 0.4f);
                break;
                
            case 7:
                AddIcon(positions, scales, Vector3.zero, defaultScale);
                AddCornerIcons(positions, scales, 0.5f, defaultScale);
                AddVerticalPair(positions, scales, 0f, defaultScale);
                break;
                
            case 8:
                AddVerticalPair(positions, scales, 0.75f, defaultScale);
                AddVerticalPair(positions, scales, 0.25f, defaultScale);
                AddVerticalPair(positions, scales, -0.25f, defaultScale);
                AddVerticalPair(positions, scales, -0.75f, defaultScale);
                break;
                
            case 9:
                AddGridIcons(positions, scales, 0.5f, defaultScale);
                AddIcon(positions, scales, Vector3.zero, defaultScale);
                break;
        }

        return (positions.ToArray(), scales.ToArray());
    }

    private void AddIcon(List<Vector3> positions, List<Vector3> scales, Vector3 position, float scale)
    {
        positions.Add(position);
        scales.Add(new Vector3(scale, scale, 1f));
    }

    private void AddCornerIcons(List<Vector3> positions, List<Vector3> scales, float offset, float scale)
    {
        AddIcon(positions, scales, new Vector3(offset, offset, 0), scale);
        AddIcon(positions, scales, new Vector3(-offset, offset, 0), scale);
        AddIcon(positions, scales, new Vector3(offset, -offset, 0), scale);
        AddIcon(positions, scales, new Vector3(-offset, -offset, 0), scale);
    }

    private void AddVerticalPair(List<Vector3> positions, List<Vector3> scales, float yPos, float scale)
    {
        AddIcon(positions, scales, new Vector3(0.4f, yPos, 0), scale);
        AddIcon(positions, scales, new Vector3(-0.4f, yPos, 0), scale);
    }

    private void AddGridIcons(List<Vector3> positions, List<Vector3> scales, float offset, float scale)
    {
        for (float x = -1; x <= 1; x++)
        {
            for (float y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                AddIcon(positions, scales, new Vector3(x * offset, y * offset, 0), scale);
            }
        }
    }
}