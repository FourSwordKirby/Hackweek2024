using System.Collections.Generic;
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
        // Sometimes this is null. Need to make sure the game manager is always adding additional boxes
        // After unpausing the game.
        GameObject obj = spawnedBoxes[pileIndex];
        obj.transform.GetChild(0).GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-5f, 5f), 10 * 100, Random.Range(-5f, 5f)), ForceMode.Impulse);
        Destroy(obj, 1);
        spawnedBoxes[pileIndex] = null;
    }
    
    void SpawnBox(int pileIndex, CriteriaType criteriaType)
    {
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
    }
}
