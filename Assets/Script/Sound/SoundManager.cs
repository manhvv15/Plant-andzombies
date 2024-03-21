using System.Collections;
using UnityEngine;

namespace Assets.Script.Sound
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;

        [SerializeField] private AudioSource _musicSource, _effectSource, _musicSource2;
        private bool isUsingMainSource = true;
        private float crossFadeDuration = 1.5f;

        private void Start()
        {
            _musicSource.loop = true;
            _musicSource2.loop = true;
            _effectSource.loop = false;
        }
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
        }

        public void PlaySound(AudioClip clip)
        {
            if (clip == null)
            {
                return;
            }
            _effectSource.PlayOneShot(clip);
        }

        public void PlayMusic(AudioClip clip, float t = 0)
        {
            StopMusic();
            if (clip == null)
            {
                return;
            }
            _musicSource.clip = clip;
            _musicSource.time = t;
            _musicSource.Play();
        }

        public void StopMusic()
        {
            _musicSource.Stop();
            _musicSource2.Stop();
            isUsingMainSource = true;
            _musicSource.volume = 1;
            _musicSource2.volume = 0;
        }

        public void PauseMusic()
        {
            _musicSource.Pause();
            _musicSource2.Pause();
        }

        public void UnpauseMusic()
        {
            _musicSource.Play();
            _musicSource2.Play();
        }


        public void SwapMusic(AudioClip clip)
        {
            if (isUsingMainSource)
            {
                _musicSource2.volume = 0.0f;
                _musicSource2.clip = clip;
                _musicSource2.time = _musicSource.time;
                _musicSource2.Play();
            }
            else
            {
                _musicSource.volume = 0.0f;
                _musicSource.clip = clip;
                _musicSource.time = _musicSource2.time;
                _musicSource.Play();
            }
            StartCoroutine(CrossFade());
        }

        private IEnumerator CrossFade()
        {
            float time = 0.0f;

            if (isUsingMainSource)
            {
                while (time <= crossFadeDuration)
                {
                    time += Time.deltaTime;
                    float progress = time / crossFadeDuration;
                    _musicSource.volume = Mathf.Lerp(1.0f, 0.0f, progress);
                    _musicSource2.volume = Mathf.Lerp(0.0f, 1.0f, progress);
                    yield return null;
                }
                isUsingMainSource = false;
                _musicSource.Stop();
            }
            else
            {
                while (time <= crossFadeDuration)
                {
                    time += Time.deltaTime;
                    float progress = time / crossFadeDuration;
                    _musicSource.volume = Mathf.Lerp(0.0f, 1.0f, progress);
                    _musicSource2.volume = Mathf.Lerp(1.0f, 0.0f, progress);
                    yield return null;
                }
                isUsingMainSource = true;
                _musicSource2.Stop();
            }
        }
    }
}
