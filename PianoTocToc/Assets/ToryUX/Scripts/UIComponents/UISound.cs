using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ToryValue;
using UnityEngine;
using UnityEngine.Audio;

namespace ToryUX
{
    public class UISound : MonoBehaviour, IDefaultValueSetter
    {
        #region Singleton
        static volatile UISound instance;
        static readonly object syncRoot = new object();

        public static UISound Instance
        {
            get
            {
                if (instance == null)
                {
                    lock(syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = FindObjectOfType<UISound>();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        private static AudioSource player;
        private static AudioSource Player
        {
            get
            {
                if (UISound.player == null)
                {
                    UISound.player = Instance.gameObject.GetComponent<AudioSource>();
                    if (UISound.player == null)
                    {
                        UISound.player = Instance.gameObject.AddComponent<AudioSource>();
                    }
                }
                return UISound.player;
            }
            set
            {
                UISound.player = value;
            }
        }

        [SerializeField]
        private AudioMixer audioMixer;
        public static AudioMixer AudioMixer
        {
            get
            {
                return Instance.audioMixer;
            }
            set
            {
                Instance.audioMixer = value;
            }
        }

        [SerializeField]
        ToryFloat masterVolume;
        public ToryFloat MasterVolume
        {
            get
            {
                if (masterVolume == null)
                {
                    masterVolume = new ToryFloat(MethodBase.GetCurrentMethod().Name.Substring(4), .7f, .7f, .7f);
                }
                return masterVolume;
            }
        }

        [SerializeField]
        ToryFloat bgmVolume;
        public ToryFloat BgmVolume
        {
            get
            {
                if (bgmVolume == null)
                {
                    bgmVolume = new ToryFloat(MethodBase.GetCurrentMethod().Name.Substring(4), .7f, .7f, .7f);
                }
                return bgmVolume;
            }
        }

        [SerializeField]
        ToryFloat sfxVolume;
        public ToryFloat SfxVolume
        {
            get
            {
                if (sfxVolume == null)
                {
                    sfxVolume = new ToryFloat(MethodBase.GetCurrentMethod().Name.Substring(4), .7f, .7f, .7f);
                }
                return sfxVolume;
            }
        }

        void Awake()
        {
            #region Create Singleton Instance
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning("UISound component can only be one in a scene. Destroying duplicate.");
                #endif
                Destroy(gameObject);
            }
            #endregion

            MasterVolume.ValueChanged += delegate(float value)
            {
                SetVolume(MasterVolume.Key, value);
                ToryCare.Config.MasterVolume = MasterVolume.Value;
                if (SettingsUI.Instance != null)
                {
                    SettingsUI.Instance.saveSharedSettingsJson = true;
                }
            };

            BgmVolume.ValueChanged += delegate(float value)
            {
                SetVolume(BgmVolume.Key, value);
                ToryCare.Config.BgmVolume = BgmVolume.Value;
                if (SettingsUI.Instance != null)
                {
                    SettingsUI.Instance.saveSharedSettingsJson = true;
                }
            };

            SfxVolume.ValueChanged += delegate(float value)
            {
                SetVolume(SfxVolume.Key, value);
                ToryCare.Config.SfxVolume = SfxVolume.Value;
                if (SettingsUI.Instance != null)
                {
                    SettingsUI.Instance.saveSharedSettingsJson = true;
                }
            };
        }

        // Bugfix : Set exposedParameter in AudioMixer doesn't applied before few frames.
        IEnumerator Start()
        {
            yield return null;

            MasterVolume.SavedValue = ToryCare.Config.MasterVolume;
            BgmVolume.SavedValue = ToryCare.Config.BgmVolume;
            SfxVolume.SavedValue = ToryCare.Config.SfxVolume;

            MasterVolume.LoadSavedValue();
            BgmVolume.LoadSavedValue();
            SfxVolume.LoadSavedValue();

            SetVolume(MasterVolume.Key, MasterVolume.Value);
            SetVolume(BgmVolume.Key, BgmVolume.Value);
            SetVolume(SfxVolume.Key, SfxVolume.Value);
        }

        /// <summary>
        /// Plays provided <c>audioClip</c> via <c>PlayOneShot</c> method.
        /// </summary>
        /// <param name="audioClip"><c>AudioClip</c> to play.</param>
        public static void Play(AudioClip audioClip, float delay = 0)
        {
            if (delay <= 0)
            {
                UISound.Player.PlayOneShot(audioClip, Instance.SfxVolume.Value);
            }
            else
            {
                Instance.StartCoroutine(Instance.DelayedPlayOneShotCoroutine(audioClip, delay));
            }
        }

        IEnumerator DelayedPlayOneShotCoroutine(AudioClip audioClip, float delay)
        {
            yield return new WaitForSeconds(delay);
            UISound.Player.PlayOneShot(audioClip, Instance.SfxVolume.Value);
        }

        void OnDestroy()
        {
            if (UISound.Player.gameObject == this)
            {
                UISound.Player = null;
            }
            instance = null;
        }

        #region Audio Volume Control

        public static void SetVolume(string mixerGroupVolumeExposedName, float normalizedVolume)
        {
            if (SettingsUI.Instance == null)
            {
                return;
            }
            if (AudioMixer == null)
            {
                Debug.LogWarning("AudioMixer is not assinged at hierarchy ToryUX/Canvas/UI Root");
                return;
            }

            if (normalizedVolume > 0)
            {
                AudioMixer.SetFloat(mixerGroupVolumeExposedName, 20f * Mathf.Log10(normalizedVolume));
            }
            else
            {
                AudioMixer.SetFloat(mixerGroupVolumeExposedName, -144f);
            }
        }

        public static float GetLinerVolume(string mixerGroupVolumeExposedName)
        {
            if (SettingsUI.Instance == null)
            {
                return -1f;
            }
            if (AudioMixer == null)
            {
                Debug.LogWarning("AudioMixer is not assinged at hierarchy ToryUX/Canvas/UI Root");
                return -1f;
            }

            float volume = float.MinValue;
            AudioMixer.GetFloat(mixerGroupVolumeExposedName, out volume);

            return (volume > -144f) ? Mathf.Pow(10f, volume) : -1f;
        }

        public void RevertToDefault()
        {
            MasterVolume.LoadDefaultValue();
            MasterVolume.Save();

            BgmVolume.LoadDefaultValue();
            BgmVolume.Save();

            SfxVolume.LoadDefaultValue();
            SfxVolume.Save();
        }

        #endregion
    }
}