using UnityEngine;

namespace DefaultNamespace
{
    public class AudioManager : MonoBehaviour
    {
        // Singleton instance
        private static AudioManager _instance;
        public static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject("AudioManager");
                    _instance = obj.AddComponent<AudioManager>();
                    DontDestroyOnLoad(obj);
                }
                return _instance;
            }
        }

        // Audio clips
        public AudioClip shootSound;        // Gunshot sound effect
        public AudioClip backgroundMusic1;  // Background music 1
        public AudioClip backgroundMusic2;  // Background music 2

        public AudioSource _audioSource;         // Audio source for sound effects
        public AudioSource _backgroundSource;    // Audio source for background music

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            
            _backgroundSource.loop = true;
            SwitchBackgroundMusic(1);
        }

        // Play gunshot sound effect
        public void PlayShootSound()
        {
            if (shootSound != null)
            {
                _audioSource.PlayOneShot(shootSound);
            }
        }

        // Switch background music by index
        public void SwitchBackgroundMusic(int musicIndex)
        {
            _backgroundSource.Stop();

            switch (musicIndex)
            {
                case 1:
                    if (backgroundMusic1 != null)
                    {
                        _backgroundSource.clip = backgroundMusic1;
                        _backgroundSource.Play();
                    }
                    break;
                case 2:
                    if (backgroundMusic2 != null)
                    {
                        _backgroundSource.clip = backgroundMusic2;
                        _backgroundSource.Play();
                    }
                    break;
                default:
                    Debug.LogWarning("Invalid music index provided.");
                    break;
            }
        }
    }
}
