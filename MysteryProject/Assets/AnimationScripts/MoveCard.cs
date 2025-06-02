using UnityEngine;

public class MoveCard : MonoBehaviour
{
    Vector3 targetPosition;
    public float duration = 1.0f;
    private Vector3 startPosition;
    private float elapsedTime = 0f;
    private bool moving = true;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + new Vector3(2.0f, 5.0f, 0.0f); //placeholder for the move target
    }

    void Update()
    {
        if (moving)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            if (t >= 1f)
                moving = false;
        }
    }
}
