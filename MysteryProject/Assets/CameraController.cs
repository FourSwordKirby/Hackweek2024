using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [System.Serializable]
    public struct CameraTarget
    {
        public GameManager.GameManagerState state;
        public Transform target;
    }

    [SerializeField] List<CameraTarget> cameraTargets = new();

    [SerializeField] float positionLerpSpeed = 10f;
    [SerializeField] float rotationLerpSpeed = 10f;

    private Transform currentTarget;

    void Start()
    {
        GameManager.instance.OnGameStateChanged.AddListener(OnGameStateChanged);
    }

    void Update()
    {
        if (currentTarget == null) return;
        transform.position = Vector3.Lerp(transform.position, currentTarget.position, Time.deltaTime * positionLerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, currentTarget.rotation, Time.deltaTime * rotationLerpSpeed);
    }

    void OnDestroy()
    {
        GameManager.instance.OnGameStateChanged.RemoveListener(OnGameStateChanged);
    }

    private void OnGameStateChanged(GameManager.GameManagerState state)
    {
        foreach (var item in cameraTargets)
            if (item.state.Equals(state))
                currentTarget = item.target;
    }
}
