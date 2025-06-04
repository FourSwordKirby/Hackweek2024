using System.Collections.Generic;
using UnityEngine;

public class CardSpawnHandler : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;

    [SerializeField] CardManager cardManager;
    [SerializeField] CardVisuals heldCardModel;

    [SerializeField] Vector3 cardSpawnRotation;
    [SerializeField] Vector3[] cardSpawnPositions;

    List<List<GameObject>> spawnedCards = new();

    void OnEnable()
    {
        cardManager.OnCardAddedToPile.AddListener(SpawnCardVisual);
        cardManager.OnPileStashed.AddListener(OnPileStashed);
    }

    void OnDisable()
    {
        cardManager.OnCardAddedToPile.RemoveListener(SpawnCardVisual);
        cardManager.OnPileStashed.RemoveListener(OnPileStashed);
    }

    void Start()
    {
        // heldCardModel.baseCardInfo = cardManager.MainDeck.PeekTopCard();
    }

    void SpawnCardVisual(int pileIndex)
    {
        Card drawnCard = cardManager.currentSelectedPile.PeekTopCard();
        GameObject cardVisual = Instantiate(cardPrefab, cardSpawnPositions[pileIndex], Quaternion.Euler(cardSpawnRotation));
        cardVisual.GetComponentInChildren<CardVisuals>().baseCardInfo = drawnCard;

        if (pileIndex > spawnedCards.Count - 1)
            for (int i = spawnedCards.Count; i <= pileIndex; i++)
                spawnedCards.Add(new List<GameObject>());

        spawnedCards[pileIndex].Add(cardVisual);
        heldCardModel.baseCardInfo = cardManager.MainDeck.PeekTopCard();
    }

    void OnPileStashed(int pileIndex)
    {
        foreach (GameObject obj in spawnedCards[pileIndex])
        {
            obj.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-5f, 5f), 10 * spawnedCards[pileIndex].Count, Random.Range(-5f, 5f)), ForceMode.Impulse);
            Destroy(obj, 1);
        }

        spawnedCards[pileIndex].Clear();
    }
}
