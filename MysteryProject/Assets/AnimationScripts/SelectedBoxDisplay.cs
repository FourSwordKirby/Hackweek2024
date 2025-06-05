using UnityEngine;

public class SelectedBoxDisplay : MonoBehaviour
{
    [SerializeField] BoxSpawnHandler boxSpawnHandler;

    [SerializeField] Vector3 positionOffset;
    [SerializeField] Vector3 rotationOffset;

    [SerializeField] float lerpSpeed = 5f;

    void Update()
    {
        Vector3 targetPos = boxSpawnHandler.GetBoxPosition(CardManager.instance.currentSelectedPileIndex);
        Vector3 targetRot = boxSpawnHandler.GetBoxRotation(CardManager.instance.currentSelectedPileIndex);

        transform.position = Vector3.Lerp(transform.position, targetPos + positionOffset, Time.deltaTime * lerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRot + rotationOffset), Time.deltaTime * lerpSpeed);
    }
}
