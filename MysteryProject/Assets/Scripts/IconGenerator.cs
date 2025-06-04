using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class IconGenerator : MonoBehaviour
{
    [Header("References")]
    public GameObject iconPrefab;
    public TextMeshPro numberText;

    private Transform iconsParent;

    void Start()
    {
        if (numberText == null)
            numberText = GetComponentInChildren<TextMeshPro>();

        iconsParent = transform.Find("Icons") ?? CreateIconsParent();
        UpdateIconsFromText();
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
        var (positions, scales) = GetIconLayout(number);
        
        for (int i = 0; i < positions.Length; i++)
        {
            var icon = Instantiate(iconPrefab, iconsParent);
            icon.transform.localPosition = positions[i];
            icon.transform.localScale = scales[i];
            icon.transform.localRotation = Quaternion.identity;
        }
    }

    private (Vector3[] positions, Vector3[] scales) GetIconLayout(int number)
    {
        switch (number)
        {
            case 1: return (new[] { Vector3.zero }, 
                           new[] { new Vector3(1,1,1) });
            
            case 2: return (new[] { new Vector3(0.4f, 0.4f, 0), 
                                    new Vector3(-0.4f, -0.4f, 0) },
                           new[] { new Vector3(0.9f, 0.9f, 1f), 
                                   new Vector3(0.9f, 0.9f, 1f) });
            
            case 3: return (new[] { new Vector3(0.55f, 0.55f, 0), 
                                    Vector3.zero, 
                                    new Vector3(-0.55f, -0.55f, 0) },
                           new[] { new Vector3(0.7f, 0.7f, 1), 
                                   new Vector3(0.7f, 0.7f, 1), 
                                   new Vector3(0.7f, 0.7f, 1) });
            
            case 4: return (new[] { new Vector3(0.4f, 0.4f, 0), 
                                    new Vector3(-0.4f, 0.4f, 0), 
                                    new Vector3(0.4f, -0.4f, 0), 
                                    new Vector3(-0.4f, -0.4f, 0) },
                           new[] { new Vector3(0.6f, 0.6f, 1), 
                                   new Vector3(0.6f, 0.6f, 1), 
                                   new Vector3(0.6f, 0.6f, 1), 
                                   new Vector3(0.6f, 0.6f, 1) });
            
            case 5: return (new[] { Vector3.zero, 
                                    new Vector3(0.45f, 0.45f, 0), 
                                    new Vector3(-0.45f, 0.45f, 0), 
                                    new Vector3(0.45f, -0.45f, 0), 
                                    new Vector3(-0.45f, -0.45f, 0) },
                           new[] { new Vector3(0.5f, 0.5f, 1), 
                                   new Vector3(0.5f, 0.5f, 1), 
                                   new Vector3(0.5f, 0.5f, 1), 
                                   new Vector3(0.5f, 0.5f, 1), 
                                   new Vector3(0.5f, 0.5f, 1) });
            
            case 6: return (new[] { new Vector3(0.4f, 0.5f, 0), 
                                    new Vector3(-0.4f, 0.5f, 0), 
                                    new Vector3(0.4f, 0, 0), 
                                    new Vector3(-0.4f, 0, 0), 
                                    new Vector3(0.4f, -0.5f, 0), 
                                    new Vector3(-0.4f, -0.5f, 0) },
                           new[] { new Vector3(0.4f, 0.4f, 1), 
                                   new Vector3(0.4f, 0.4f, 1), 
                                   new Vector3(0.4f, 0.4f, 1), 
                                   new Vector3(0.4f, 0.4f, 1), 
                                   new Vector3(0.4f, 0.4f, 1), 
                                   new Vector3(0.4f, 0.4f, 1) });
            
            case 7: return (new[] { Vector3.zero, 
                                    new Vector3(0.5f, 0.5f, 0), 
                                    new Vector3(-0.5f, 0.5f, 0), 
                                    new Vector3(0.5f, 0, 0), 
                                    new Vector3(-0.5f, 0, 0), 
                                    new Vector3(0.5f, -0.5f, 0), 
                                    new Vector3(-0.5f, -0.5f, 0) },
                           new[] { new Vector3(0.4f, 0.4f, 1f), 
                                   new Vector3(0.4f, 0.4f, 1f), 
                                   new Vector3(0.4f, 0.4f, 1f), 
                                   new Vector3(0.4f, 0.4f, 1f), 
                                   new Vector3(0.4f, 0.4f, 1f), 
                                   new Vector3(0.4f, 0.4f, 1f), 
                                   new Vector3(0.4f, 0.4f, 1f) });
            
            case 8: return (new[] { new Vector3(0.3f, 0.25f, 0), 
                                    new Vector3(-0.3f, 0.25f, 0), 
                                    new Vector3(0.3f, 0.75f, 0), 
                                    new Vector3(-0.3f, 0.75f, 0), 
                                    new Vector3(0.3f, -0.25f, 0), 
                                    new Vector3(-0.3f, -0.25f, 0), 
                                    new Vector3(0.3f, -0.75f, 0), 
                                    new Vector3(-0.3f, -0.75f, 0) },
                           new[] { new Vector3(0.4f, 0.4f, 1f), 
                                   new Vector3(0.4f, 0.4f, 1f), 
                                   new Vector3(0.4f, 0.4f, 1f), 
                                   new Vector3(0.4f, 0.4f, 1f), 
                                   new Vector3(0.4f, 0.4f, 1f), 
                                   new Vector3(0.4f, 0.4f, 1f), 
                                   new Vector3(0.4f, 0.4f, 1f), 
                                   new Vector3(0.4f, 0.4f, 1f) });
            
            case 9: return (new[] { new Vector3(0.5f, 0.5f, 0), 
                                    new Vector3(0f, 0.5f, 0), 
                                    new Vector3(0f, 0f, 0),
                                    new Vector3(-0.5f, 0.5f, 0), 
                                    new Vector3(0.5f, 0f, 0), 
                                    new Vector3(-0.5f, 0f, 0), 
                                    new Vector3(0.5f, -0.5f, 0), 
                                    new Vector3(0f, -0.5f, 0), 
                                    new Vector3(-0.5f, -0.5f, 0) },
                           new[] { new Vector3(0.4f, 0.4f, 1f), 
                                   new Vector3(0.4f, 0.4f, 1f), 
                                   new Vector3(0.4f, 0.4f, 1f), 
                                   new Vector3(0.4f, 0.4f, 1f), 
                                   new Vector3(0.4f, 0.4f, 1f), 
                                   new Vector3(0.4f, 0.4f, 1f), 
                                   new Vector3(0.4f, 0.4f, 1f), 
                                   new Vector3(0.4f, 0.4f, 1f), 
                                   new Vector3(0.4f, 0.4f, 1f) });
            
            default: return (new Vector3[0], new Vector3[0]);
        }
    }
}