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
            GameObject TopCardObject = Instantiate(cardPrefab.gameObject, this.transform.position + Vector3.up * 2, Quaternion.Euler(90, 90, 0));
            TopCardObject.GetComponent<CardVisuals>().baseCardInfo = BottomCard;
            TopCardObject.GetComponent<Rigidbody>().linearVelocity = Vector3.down * 3;

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

        float flingTimer = 0;
        while (flingTimer <= 0.3)
        {
            stackTray.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(60 * flingTimer * (1.0f/0.3f), 0, 0));
            stackTray.GetComponent<Rigidbody>().MovePosition(transform.TransformPoint(Vector3.up * flingTimer * (1.0f / 0.3f)));

            flingTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        float retractTimer = 0;
        while (retractTimer <= 1)
        {
            stackTray.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(60 * (1-retractTimer), 0, 0));
            stackTray.GetComponent<Rigidbody>().MovePosition(transform.TransformPoint(Vector3.up * (1-retractTimer)));

            retractTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        stackTray.transform.rotation = Quaternion.identity;
        stackTray.transform.position = transform.TransformPoint(Vector3.zero);
    }
}
