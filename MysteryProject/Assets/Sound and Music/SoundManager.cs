using System;
using UnityEngine;
using UnityEngine.Audio;

[DefaultExecutionOrder(-30)]
public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixerSnapshot m_defaultSnapshot;
    [SerializeField]
    private AudioMixerSnapshot m_attenuatedSnapshot;
    [SerializeField]
    private AudioClip[] m_musicPlaylist = new AudioClip[0];
    [SerializeField]
    private AudioSource m_muiscAudioSource;
    [SerializeField]
    private AudioSource m_cardAddedAudioSource;
    [SerializeField]
    private AudioSource m_pileStashedAudioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.instance.OnGameStarted.AddListener(OnGameStarted);
        GameManager.instance.OnGameReset.AddListener(OnGameReset);
        GameManager.instance.OnGameEnded.AddListener(OnGameEnded);
        GameManager.instance.OnGameStateChanged.AddListener(OnGameStateChanged);

        CardManager.instance.OnCardAddedToPile.AddListener(OnCardAddedToPile);
        CardManager.instance.OnPileStashed.AddListener(OnPileStashed);
    }

    void OnDestroy()
    {
        GameManager.instance.OnGameStarted.RemoveListener(OnGameStarted);
        GameManager.instance.OnGameReset.RemoveListener(OnGameReset);
        GameManager.instance.OnGameEnded.AddListener(OnGameEnded);
        GameManager.instance.OnGameStateChanged.RemoveListener(OnGameStateChanged);

        CardManager.instance.OnCardAddedToPile.AddListener(OnCardAddedToPile);
        CardManager.instance.OnPileStashed.AddListener(OnPileStashed);
    }

    private void OnCardAddedToPile(int arg0)
    {
        m_cardAddedAudioSource.Play();
    }

    private void OnPileStashed(int arg0)
    {
        m_pileStashedAudioSource.Play();
    }

    private void OnGameStarted()
    {
        var randomMusicClipId = UnityEngine.Random.Range(0, m_musicPlaylist.Length);
        m_muiscAudioSource.clip = m_musicPlaylist[randomMusicClipId];
        m_muiscAudioSource.loop = true;
        m_muiscAudioSource.Play();
    }

    private void OnGameEnded(GameState arg0)
    {
        m_muiscAudioSource.Stop();
    }

    private void OnGameReset()
    {
        m_muiscAudioSource.Stop();
    }

    private void OnGameStateChanged(GameManager.GameManagerState state)
    {
        switch (state)
        {
            case GameManager.GameManagerState.Running:
                m_defaultSnapshot.TransitionTo(0.5f);
                break;
            case GameManager.GameManagerState.Paused:
                m_attenuatedSnapshot.TransitionTo(0.5f);
                break;
        }
    }
}
