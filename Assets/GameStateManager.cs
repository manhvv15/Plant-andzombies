using System.Collections;
using System.Collections.Generic;
using Assets.Script.Sound;
using UnityEngine;
using UnityEngine.Events;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    private bool isPaused = false;
    [SerializeField] private AudioClip pauseAudioClip;
    [SerializeField] private AudioClip unPauseAudioClip;

    public UnityEvent GamePaused;
    public UnityEvent GameUnPaused;

    private bool allowPausing = false;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (allowPausing && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                isPaused = true;
                SoundManager.Instance.PlaySound(pauseAudioClip);
                Time.timeScale = 0;
                SoundManager.Instance.PauseMusic();
                GamePaused.Invoke();
            }
            else
            {
                isPaused = false;
                SoundManager.Instance.PlaySound(unPauseAudioClip);
                Time.timeScale = 1;
                SoundManager.Instance.UnpauseMusic();
                GameUnPaused.Invoke();
            }
        }
    }

    public void AllowPausing()
    {
        allowPausing = true;
    }

    public void DenyPausing()
    {
        allowPausing = false;
        GamePaused.RemoveAllListeners();
        GameUnPaused.RemoveAllListeners();
    }
}
