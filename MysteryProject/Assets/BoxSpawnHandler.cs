using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoxSpawnHandler : MonoBehaviour
{
    [SerializeField] Vector3[] boxSpawnPositions;
    [SerializeField] Vector3[] boxSpawnRotations;

    [SerializeField] GameObject incrementBoxPrefab;

    [SerializeField] GameManager gameManager;
    [SerializeField] CardManager cardManager;

    List<GameObject> spawnedBoxes = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        gameManager.OnGameStarted.AddListener(OnGameStarted);
        gameManager.OnGameEnded.AddListener(OnGameEnded);
        cardManager.OnPileCreated.AddListener(SpawnBox);
        cardManager.OnPileStashed.AddListener(StashBox);
    }

    void OnDisable()
    {
        gameManager.OnGameStarted.RemoveListener(OnGameStarted);
        gameManager.OnGameEnded.RemoveListener(OnGameEnded);
        cardManager.OnPileCreated.RemoveListener(SpawnBox);
        cardManager.OnPileStashed.RemoveListener(StashBox);
    }

    void OnGameStarted()
    {
    }

    void OnGameEnded(GameState state)
    {
    }

    void StashBox(int pileIndex)
    {
        Debug.Log("StashBox");
        // Sometimes this is null. Need to make sure the game manager is always adding additional boxes
        // After unpausing the game.
        GameObject obj = spawnedBoxes[pileIndex];
        obj.transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
        obj.transform.GetChild(0).GetComponent<Rigidbody>().AddForce(new Vector3(0, 4 * 100, 0));
        obj.transform.GetChild(0).GetComponent<Rigidbody>().AddTorque(new Vector3(0, 300, 0));

        Destroy(obj, 1);
        spawnedBoxes[pileIndex] = null;
        
        // Give the object to a Coroutine to animate and destroy.
        
    }

    void SpawnBox(int pileIndex, CardPileCriteria criteria)
    {
        /*
        Debug.Log("SpawnBox");
        // Won't always be the incrementBoxPrefab. Have method to go from CriteriaType->Prefab.
        GameObject boxVisual = Instantiate(
            incrementBoxPrefab,
            boxSpawnPositions[pileIndex],
            Quaternion.Euler(boxSpawnRotations[pileIndex]));

        if (pileIndex >= spawnedBoxes.Count)
        {
            spawnedBoxes.Add(boxVisual);
        }
        else
        {
            spawnedBoxes[pileIndex] = boxVisual;
        }
        */

        StartCoroutine(DelaySpawnBox(pileIndex, criteria));
    }
    
    IEnumerator DelaySpawnBox(int pileIndex, CardPileCriteria criteria)
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("SpawnBox");
        // Won't always be the incrementBoxPrefab. Have method to go from CriteriaType->Prefab.
        GameObject boxVisual = Instantiate(
            incrementBoxPrefab,
            boxSpawnPositions[pileIndex],
            Quaternion.Euler(boxSpawnRotations[pileIndex]));

        boxVisual.GetComponentInChildren<TextMeshPro>().text = criteria.CriteriaDescriptor();

        if (pileIndex >= spawnedBoxes.Count)
        {
            spawnedBoxes.Add(boxVisual);
        }
        else
        {
            spawnedBoxes[pileIndex] = boxVisual;
        }

        yield return new WaitForEndOfFrame();
    }

    public Vector3 GetBoxPosition(int pileIndex)
    {
        if (pileIndex < 0 || pileIndex >= boxSpawnPositions.Length)
            return Vector3.zero;
        return boxSpawnPositions[pileIndex];
    }

    public Vector3 GetBoxRotation(int pileIndex)
    {
        if (pileIndex < 0 || pileIndex >= boxSpawnRotations.Length)
            return Vector3.zero;
        return boxSpawnRotations[pileIndex];
    }
}
