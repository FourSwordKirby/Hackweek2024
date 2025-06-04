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

    public Card baseCardInfo;

    // Update is called once per frame
    void Update()
    {
        selfRenderer.material = baseCardInfo.suit switch
        {
        CardSuit.Red => RedMaterial,
        CardSuit.Yellow => YellowMaterial,
        CardSuit.Green => GreenMaterial,
        CardSuit.Blue => BlueMaterial
        };

        TopNumber.text = baseCardInfo.numberValue.ToString();
        BottomNumber.text = baseCardInfo.numberValue.ToString();
    }
}
