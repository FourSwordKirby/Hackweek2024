using System;
using UnityEngine;

[DefaultExecutionOrder(-50)]
public class GameOverUI : MonoBehaviour
{
    private Animator m_animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_animator = GetComponent<Animator>();

        var canvas = GetComponent<Canvas>();

        if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null)
        {
            canvas.worldCamera = Camera.main;
        }

        
        if (GameManager.instance != null)
        {
            GameManager.instance.OnGameEnded.AddListener(OnEndGame);
        }
    }

    private void OnEndGame(int s)
    {
        m_animator.Play("Animation");
    }
}
