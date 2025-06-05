using UnityEngine;

public class SelectedBoxDisplay : MonoBehaviour
{
    [SerializeField] Vector3 positionOffset;
    [SerializeField] Vector3 rotationOffset;

    [SerializeField] float lerpSpeed = 5f;
    [SerializeField] Transform[] boxes;

    void Update()
    {
        Transform targetPos = boxes[CardManager.instance.currentSelectedPileIndex];

        transform.position = Vector3.Lerp(transform.position, targetPos.position + positionOffset, Time.deltaTime * lerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetPos.rotation * Quaternion.Euler(rotationOffset), Time.deltaTime * lerpSpeed);
    }
}
