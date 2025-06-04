using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawnHandler : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;

    [SerializeField] GameManager gameManager;
    [SerializeField] CardManager cardManager;
    [SerializeField] CardVisuals heldCardModel;

    [SerializeField] Vector3 cardSpawnRotation;
    [SerializeField] Vector3[] cardSpawnPositions;

    [SerializeField] Vector3 initialSpawnPosition;
    [SerializeField] float[] animationForces;
    [SerializeField] float[] animationTimes;
    
    List<List<GameObject>> spawnedCards = new();

    void OnEnable()
    {
        gameManager.OnGameStarted.AddListener(OnGameStarted);
        gameManager.OnGameEnded.AddListener(OnGameEnded);
        cardManager.OnCardAddedToPile.AddListener(SpawnCardVisual);
        cardManager.OnPileStashed.AddListener(OnPileStashed);
    }

    void OnDisable()
    {
        gameManager.OnGameStarted.RemoveListener(OnGameStarted);
        gameManager.OnGameEnded.RemoveListener(OnGameEnded);
        cardManager.OnCardAddedToPile.RemoveListener(SpawnCardVisual);
        cardManager.OnPileStashed.RemoveListener(OnPileStashed);
    }

    void OnGameStarted()
    {
        heldCardModel.baseCardInfo = cardManager.MainDeck.PeekTopCard();
    }

    void OnGameEnded(GameState state)
    {
        for (int i = 0; i < spawnedCards.Count; i++)
        {
            foreach (GameObject obj in spawnedCards[i])
            {
                obj.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-5f, 5f), 10 * spawnedCards[i].Count, Random.Range(-5f, 5f)), ForceMode.Impulse);
                Destroy(obj, 1);
            }

            spawnedCards[i].Clear();
        }
    }

    void SpawnCardVisual(int pileIndex)
    {
        Card drawnCard = cardManager.currentSelectedPile.PeekTopCard();
        
        // Spawn a card at the initial position, give it a force in the direction of the card spawn position
        // Then a second later, we need to reset the position and velocity to cardspawnposition and 0.
        
        //GameObject cardVisual = Instantiate(cardPrefab, cardSpawnPositions[pileIndex], Quaternion.Euler(cardSpawnRotation));
        Quaternion finalCardRotation = Quaternion.Euler(cardSpawnRotation);
        GameObject cardVisual = Instantiate(cardPrefab, initialSpawnPosition, finalCardRotation);
        cardVisual.GetComponentInChildren<CardVisuals>().baseCardInfo = drawnCard;

        if (pileIndex > spawnedCards.Count - 1)
            for (int i = spawnedCards.Count; i <= pileIndex; i++)
                spawnedCards.Add(new List<GameObject>());

        spawnedCards[pileIndex].Add(cardVisual);
        heldCardModel.baseCardInfo = cardManager.MainDeck.PeekTopCard();
        
        // 
        Vector3 finalCardPosition = cardSpawnPositions[pileIndex];
        Vector3 forceDirection = finalCardPosition - initialSpawnPosition;
        forceDirection.y += 0.5f;
        StartCoroutine(StartCardThrowAnimation(
            cardVisual,
            finalCardPosition,
            finalCardRotation,
            forceDirection,
            animationForces[pileIndex],
            animationTimes[pileIndex]));
    }

    private IEnumerator StartCardThrowAnimation(
        GameObject cardVisual, 
        Vector3 finalPosition, 
        Quaternion finalRotation,
        Vector3 forceDirection, 
        float force, 
        float time)
    {
        Rigidbody cardRigidBody = cardVisual.GetComponent<Rigidbody>();
        cardRigidBody.AddForce(forceDirection * force);
        yield return new WaitForSeconds(time);
        
        // After the specified time, reset the position and the velocity.
        cardRigidBody.linearVelocity = Vector3.zero;
        cardRigidBody.angularVelocity = Vector3.zero;
        cardRigidBody.position = finalPosition;
        cardRigidBody.rotation = finalRotation;
    }

    void OnPileStashed(int pileIndex)
    {
        if (pileIndex > spawnedCards.Count - 1)
            return;

        foreach (GameObject obj in spawnedCards[pileIndex])
        {
            obj.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-5f, 5f), 10 * spawnedCards[pileIndex].Count, Random.Range(-5f, 5f)), ForceMode.Impulse);
            Destroy(obj, 1);
        }

        spawnedCards[pileIndex].Clear();
    }
}
