using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardThrowAnimator : MonoBehaviour
{
    public GameObject cratePrefab;
    public GameObject cardPrefab;
    public Camera userCamera;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Create a crate in front of the camera.
        GameObject crate = Instantiate(
            cratePrefab, 
            userCamera.transform.position + userCamera.transform.forward * 5, 
            Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            // Throw a card
            StartCoroutine(AnimateCardPileWithScore());
        }
    }

    private IEnumerator AnimateCardPileWithScore()
    {
        // Create new card at a position in front of the camera.
        GameObject newCard = Instantiate(
            cardPrefab,
            userCamera.transform.position + userCamera.transform.forward,
            Quaternion.identity);

        yield return new WaitForEndOfFrame();
        
        // Get the rigidbody on the cube of the Card prefab.
        GameObject cube = newCard.transform.GetChild(0).gameObject;
        Rigidbody cardRigidBody = cube.GetComponent<Rigidbody>();

        // Determine the direction of the force we want to apply to the card.
        // Have it going more forwards than up.
        // Apply a torque to give the card some spin.
        Vector3 direction = Vector3.Normalize(userCamera.transform.up + 2*userCamera.transform.forward);
        cardRigidBody.AddForce(direction * 300);
        cardRigidBody.AddTorque(Vector3.up * 200);
    }
}
