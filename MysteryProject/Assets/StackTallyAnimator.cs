using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class StackTallyAnimator : MonoBehaviour
{
    public CardVisuals cardPrefab;
    public GameObject stackTray;
    public TextMeshPro scoreTally;

    public float currentDisplayedScore = 0;
    public float targetScore = 0;

    private void Start()
    {
        scoreTally.gameObject.SetActive(false);
        GameManager.instance.OnGameEnded.AddListener(PlayResultAnimation);
    }

    private void Update()
    {
        currentDisplayedScore += (targetScore - currentDisplayedScore) * Time.deltaTime * 2;
        scoreTally.text = Mathf.CeilToInt(currentDisplayedScore).ToString();
    }

    private void PlayResultAnimation(GameState finalGameState)
    {
        scoreTally.gameObject.SetActive(true);
        StartCoroutine(StartReultAnimation(finalGameState));
    }

    private IEnumerator StartReultAnimation(GameState finalGameState)
    {
        foreach (GradedCardPile gradedPile in finalGameState.StashedPiles)
        {
            // Generating an arbitrary pile to test the animator
            CardPileCriteria scoringCriteria = gradedPile.criteria;
            CardPile pile = gradedPile.pile;
            yield return AnimateCardPileWithScore(scoringCriteria, pile);
        }
    }

    private IEnumerator AnimateCardPileWithScore(CardPileCriteria scoringCriteria, CardPile pile)
    {
        bool cardScored = false;
        CardPile runningCardPile = new CardPile();

        float runningTargetScore = targetScore;
        float stashTargetScore = 0;

        while (!pile.IsEmpty())
        {
            Card BottomCard = pile.DrawBottomCard();
            GameObject TopCardObject = Instantiate(cardPrefab.gameObject, this.transform.position + Vector3.up * 10, Quaternion.identity);
            TopCardObject.GetComponent<CardVisuals>().baseCardInfo = BottomCard;
            TopCardObject.GetComponent<Rigidbody>().linearVelocity = Vector3.down * 10;

            cardScored = false;

            yield return new WaitForEndOfFrame();


            while (!cardScored)
            {
                if(TopCardObject.GetComponent<Rigidbody>().linearVelocity.y > 0)
                {
                    cardScored = true;
                    runningCardPile.PlaceCardOnTop(BottomCard);
                    stashTargetScore = scoringCriteria.ScorePile(runningCardPile);
                    targetScore = runningTargetScore + stashTargetScore;
                }
                yield return new WaitForEndOfFrame();
            }
        }

        yield return new WaitForSeconds(0.5f);

        float rotationTimer = 0;
        while (rotationTimer <= 1)
        {
            stackTray.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0, 0, 360 * rotationTimer));
            rotationTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        stackTray.transform.rotation = Quaternion.identity;

    }
}
