using System;
using System.Collections;
using UnityEngine;

public class ShowRedX : MonoBehaviour
{
    public GameObject redCross; // Assign in Inspector
    public float displayTime = 0.2f; // Duration in seconds
    [SerializeField] Vector3 cardSpawnRotation;
    [SerializeField] Vector3[] cardSpawnPositions;
    private void Awake()
    {
        HideIt();
    }

    void Start()
    {
        CardManager.instance.OnInvalidCard.AddListener((index)=>StartCoroutine(ShowIt(index)));
    }

    IEnumerator ShowIt(int index)
    {
        redCross.transform.position = new Vector3(cardSpawnPositions[index].x, cardSpawnPositions[index].y, cardSpawnPositions[index].z);
        redCross.SetActive(true); // Show the red cross
        yield return new WaitForSeconds(displayTime); // Wait
        redCross.SetActive(false); // Hide it
    }

    void HideIt()
    {
        redCross.SetActive(false);
    }

}
