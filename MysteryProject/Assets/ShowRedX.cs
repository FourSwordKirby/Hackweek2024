using System;
using System.Collections;
using UnityEngine;

public class ShowRedX : MonoBehaviour
{
    [SerializeField] BoxSpawnHandler boxSpawnHandler;

    public GameObject redCross; // Assign in Inspector
    public float displayTime = 0.2f; // Duration in seconds
    [SerializeField] Vector3 positionOffset;
    [SerializeField] Vector3 rotationOffset;
    [SerializeField] Transform[] boxes;
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
        redCross.transform.position = boxSpawnHandler.GetBoxPosition(index) + positionOffset; // Set position with offset
        redCross.transform.rotation = Quaternion.Euler(boxSpawnHandler.GetBoxRotation(index) + rotationOffset); // Set rotation with offset
        redCross.SetActive(true); // Show the red cross
        yield return new WaitForSeconds(displayTime); // Wait
        redCross.SetActive(false); // Hide it
    }

    void HideIt()
    {
        redCross.SetActive(false);
    }

}
