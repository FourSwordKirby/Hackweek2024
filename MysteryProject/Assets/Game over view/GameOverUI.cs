using System;
using UnityEngine;

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
    }

    void OnEnable()
    {
        GameManager.OnEndGame += OnEndGame;
    }

    void OnDisable()
    {
        GameManager.OnEndGame -= OnEndGame;
    }

    private void OnEndGame()
    {
        m_animator.Play("Animation");
    }
}
