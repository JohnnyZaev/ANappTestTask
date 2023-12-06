using Events;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    /// <summary>
    /// AudioSettingsSO is a ScriptableObject that holds the AudioMixer, volume settings, mute settings,
    /// and AudioClip references for various game events. This allows for easy management of game-wide
    /// audio settings and clips in a single location. The ScriptableObject also subscribes to volume
    /// change events to keep its volume settings up-to-date.
    /// </summary>
    [CreateAssetMenu(fileName = "AudioSettings", menuName = "ANappTestTask/AudioSettings", order = 1)]
    public class AudioSettingsSo : DescriptionSo
    {
        // Default ScriptableObject data
        private const float KDefaultMasterVolume = 1f;
        private const float KDefaultSfxVolume = 1f;
        private const float KDefaultMusicVolume = 0f;

        [FormerlySerializedAs("m_AudioMixer")]
        [Header("Mixer")]
        [Tooltip("The AudioMixer that controls the audio levels for the game")]
        [SerializeField] private AudioMixer mAudioMixer;

        [FormerlySerializedAs("m_MasterVolume")]
        [Header("Volume Settings")]
        [Tooltip("The master volume level (0 to 1)")]
        [SerializeField] private float mMasterVolume = KDefaultMasterVolume;

        [FormerlySerializedAs("m_SoundEffectsVolume")]
        [Tooltip("The sound effects volume level (0 to 1)")]
        [SerializeField] private float mSoundEffectsVolume = KDefaultSfxVolume;

        [FormerlySerializedAs("m_MusicVolume")]
        [Tooltip("The music volume level (0 to 1)")]
        [SerializeField] private float mMusicVolume = KDefaultMusicVolume;

        // Convert bool to 1 and 0 and multiply by MixerGroup (unused in this project, here for demo purposes)
        [FormerlySerializedAs("m_IsMasterMuted")]
        [Header("Mute Settings")]
        [Tooltip("Mute or unmute the master volume")]
        [SerializeField] private bool mIsMasterMuted = false;

        [FormerlySerializedAs("m_IsSoundEffectsMuted")]
        [Tooltip("Mute or unmute the sound effects volume")]
        [SerializeField] private bool mIsSoundEffectsMuted = false;

        [FormerlySerializedAs("m_IsMusicMuted")]
        [Tooltip("Mute or unmute the music volume")]
        [SerializeField] private bool mIsMusicMuted = false;

        [FormerlySerializedAs("m_SuccessSound")]
        [Header("Sound Effects")]
        [Tooltip("AudioClip for correctly answering a question")]
        [SerializeField] private AudioClip mSuccessSound;
        [FormerlySerializedAs("m_FailSound")]
        [Tooltip("AudioClip for incorrectly answering a question")]
        [SerializeField] private AudioClip mFailSound;
        [FormerlySerializedAs("m_GameWonSound")]
        [Tooltip("AudioClip for passing the quiz")]
        [SerializeField] private AudioClip mGameWonSound;
        [FormerlySerializedAs("m_GameLostSound")]
        [Tooltip("AudioClip for failing the quiz")]
        [SerializeField] private AudioClip mGameLostSound;

        [FormerlySerializedAs("m_TapClickSound")]
        [Tooltip("AudioClip for simple button tap")]
        [SerializeField] private AudioClip mTapClickSound;

        // Properties
        public AudioMixer AudioMixer => mAudioMixer;
        public AudioClip SuccessSound => mSuccessSound;
        public AudioClip FailSound => mFailSound;
        public AudioClip GameWonSound => mGameWonSound;
        public AudioClip GameLostSound => mGameLostSound;
        public AudioClip TapClickSound => mTapClickSound;

        public float MasterVolume { get => mMasterVolume; set => mMasterVolume = value; }
        public float SoundEffectsVolume { get => mSoundEffectsVolume; set => mSoundEffectsVolume = value; }
        public float MusicVolume { get => mMusicVolume; set => mMusicVolume = value; }

        public bool IsMasterMuted { get => mIsMasterMuted; set => mIsMasterMuted = value; }
        public bool IsSoundEffectsMuted { get => mIsSoundEffectsMuted; set => mIsSoundEffectsMuted = value; }
        public bool IsMusicMuted { get => mIsMusicMuted; set => mIsMusicMuted = value; }


        // Event subscriptions
        private void OnEnable()
        {
            SettingsEvents.MasterVolumeChanged += SettingsEvents_MasterVolumeChanged;
            SettingsEvents.MusicVolumeChanged += SettingsEvents_MusicVolumeChanged;
            SettingsEvents.SfxVolumeChanged += SettingsEvents_SFXVolumeChanged;

            // Note: we disable validating the Sound Effects AudioClips here, instead we
            // validate it from the GameplaySounds class

            // NullRefChecker.Validate(this);
        }

        // Event unsubscriptions
        private void OnDisable()
        {
            SettingsEvents.MasterVolumeChanged -= SettingsEvents_MasterVolumeChanged;
            SettingsEvents.MusicVolumeChanged -= SettingsEvents_MusicVolumeChanged;
            SettingsEvents.SfxVolumeChanged -= SettingsEvents_SFXVolumeChanged;
        }

        // Update the master volume when the event is triggered.
        private void SettingsEvents_MasterVolumeChanged(float volume)
        {
            MasterVolume = volume;
        }

        // Update the sound effects volume when the event is triggered.
        private void SettingsEvents_SFXVolumeChanged(float volume)
        {
            SoundEffectsVolume = volume;
        }

        // Update the music volume when the event is triggered.
        private void SettingsEvents_MusicVolumeChanged(float volume)
        {
            MusicVolume = volume;
        }
    }
}
