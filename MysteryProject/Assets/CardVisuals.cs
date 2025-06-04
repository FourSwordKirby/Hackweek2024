using TMPro;
using UnityEngine;

public class CardVisuals : MonoBehaviour
{
    public TextMeshPro TopNumber;
    public TextMeshPro BottomNumber;
    public MeshRenderer selfRenderer;

    public Material RedMaterial;
    public Material YellowMaterial;
    public Material GreenMaterial;
    public Material BlueMaterial;
    
    public GameObject iconPrefab;
    public GameObject redIconPrefab;
    public GameObject yellowIconPrefab;
    public GameObject greenIconPrefab;
    public GameObject blueIconPrefab;
    
    public Card baseCardInfo;
    private IconGenerator iconGenerator;
    
    void Start()
    {
        InitializeIconGenerator();
        UpdateVisuals();
    }
    void Update()
    {
        UpdateVisuals();
    }
    
    private void InitializeIconGenerator()
    {
        iconGenerator = iconPrefab.GetComponent<IconGenerator>();
        if (iconGenerator == null)
        {
            iconGenerator = iconPrefab.AddComponent<IconGenerator>();
        }
    }
    
    public void UpdateVisuals()
    {
        if (baseCardInfo == null)
        {
            selfRenderer.enabled = false;
            return;
        }

        selfRenderer.enabled = true;
        UpdateCardAppearance();
        UpdateIcons();
    }
    private void UpdateCardAppearance()
    {
        if (baseCardInfo == null)
        {
            selfRenderer.enabled = false;
            return;
        }

        selfRenderer.enabled = true;
        selfRenderer.material = baseCardInfo.suit switch
        {
            CardSuit.Red => RedMaterial,
            CardSuit.Yellow => YellowMaterial,
            CardSuit.Green => GreenMaterial,
            CardSuit.Blue => BlueMaterial,
            _ => BlueMaterial
        };
        var textColor = baseCardInfo.suit switch
        {
            CardSuit.Red => Color.white,
            CardSuit.Yellow => Color.black,
            CardSuit.Green => Color.black,
            CardSuit.Blue => Color.white,
            _ => Color.black
        };

        TopNumber.text = BottomNumber.text = baseCardInfo.numberValue.ToString();
        TopNumber.color = BottomNumber.color = textColor;
    }
    private void UpdateIcons()
    {
        if (iconGenerator == null) return;

        // Select the appropriate prefab based on suit
        iconGenerator.iconPrefab = baseCardInfo.suit switch
        {
            CardSuit.Red => redIconPrefab,
            CardSuit.Yellow => yellowIconPrefab,
            CardSuit.Green => greenIconPrefab,
            CardSuit.Blue => blueIconPrefab,
            _ => blueIconPrefab
        };

        // Update the icons
        iconGenerator.UpdateIconsFromNumber(baseCardInfo.numberValue);
    }
}
