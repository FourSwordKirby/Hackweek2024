using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class StackTallyAnimator : MonoBehaviour
{
    public GameObject cardPrefab;
    public TextMeshPro scoreTally;

    public float currentDisplayedScore = 0;
    public float targetScore = 0;

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            // Generating an arbitrary pile to test the animator
            CardPileCriteria scoringCriteria = new IncrementWithDuplicatesCriteria();
            CardPile pile = new CardPile(true, true);
            for (int i = 0; i< 30;i++)
            {
                pile.DrawTopCard();
            }
            StartCoroutine(AnimateCardPileWithScore(scoringCriteria, pile));
        }

        currentDisplayedScore += (targetScore - currentDisplayedScore) * Time.deltaTime;
        scoreTally.text = currentDisplayedScore.ToString("0");
    }

    private IEnumerator AnimateCardPileWithScore(CardPileCriteria scoringCriteria, CardPile pile)
    {
        bool cardScored = false;
        CardPile runningCardPile = new CardPile();

        while (!pile.IsEmpty())
        {
            Card BottomCard = pile.DrawBottomCard();
            GameObject TopCardObject = Instantiate(cardPrefab, this.transform.position + Vector3.up * 20, Quaternion.identity);
            cardScored = false;

            yield return new WaitForEndOfFrame();


            while (!cardScored)
            {
                if(TopCardObject.GetComponent<Rigidbody>().linearVelocity.y > 0)
                {
                    cardScored = true;
                    runningCardPile.PlaceCardOnTop(BottomCard);
                    targetScore = scoringCriteria.ScorePile(runningCardPile);
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }

    //private void AnimateCardPileWithScore(CardPileCriteria scoringCriteria, CardPile pile)
    //{
    //    foreach(Card in pile)
    //    {

    //    }
    //}
}
