using UnityEngine;

namespace Scripts
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioSource SelectSound;
        [SerializeField] private AudioSource HitSound;
        [SerializeField] private AudioSource UpgradeSound;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void PlaySound(AudioSelection selection)
        {
            switch (selection)
            {
                case AudioSelection.Select:
                    SelectSound.Play();
                    break;

                case AudioSelection.Hit:
                    HitSound.Play();
                    break;

                case AudioSelection.Upgrade:
                    UpgradeSound.Play();
                    break;
            }
        }
    }

    public enum AudioSelection
    {
        Select,
        Hit,
        Upgrade,
    }
}
