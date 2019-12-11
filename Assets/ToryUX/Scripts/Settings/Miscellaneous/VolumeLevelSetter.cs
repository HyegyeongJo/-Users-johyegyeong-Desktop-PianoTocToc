using System.Collections;
using System.Collections.Generic;
using ToryValue;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace ToryUX
{
    [RequireComponent(typeof(TorySlider))]
    public class VolumeLevelSetter : DefaultValueSetterMonoBehaviour
    {
        private TorySlider sliderComponent;
        public TorySlider SliderComponent
        {
            get
            {
                if (sliderComponent == null)
                {

                    sliderComponent = GetComponent<TorySlider>();
                }
                return sliderComponent;
            }
        }

        public enum DefaultAudioMixerParameters
        {
            MasterVolume,
            BgmVolume,
            SfxVolume
        }

        [SerializeField]
        public DefaultAudioMixerParameters targetParameter;

        [SerializeField]
        float defaultValue = .7f;

        public void OnEnable()
        {
            if (UISound.Instance != null)
            {
                switch (targetParameter)
                {
                    case DefaultAudioMixerParameters.MasterVolume:
                        UISound.Instance.MasterVolume.LoadSavedValue();
                        SliderComponent.value = UISound.Instance.MasterVolume.Value;
                        defaultValue = UISound.Instance.MasterVolume.DefaultValue;
                        break;

                    case DefaultAudioMixerParameters.BgmVolume:
                        UISound.Instance.BgmVolume.LoadSavedValue();
                        SliderComponent.value = UISound.Instance.BgmVolume.Value;
                        defaultValue = UISound.Instance.BgmVolume.DefaultValue;
                        break;

                    case DefaultAudioMixerParameters.SfxVolume:
                        UISound.Instance.SfxVolume.LoadSavedValue();
                        SliderComponent.value = UISound.Instance.SfxVolume.Value;
                        defaultValue = UISound.Instance.SfxVolume.DefaultValue;
                        break;

                    default:
                        break;
                }
            }
        }

        public void SetVolume(float value)
        {
            if (UISound.Instance != null)
            {
                switch (targetParameter)
                {
                    case DefaultAudioMixerParameters.MasterVolume:
                        UISound.Instance.MasterVolume.Value = value;
                        UISound.Instance.MasterVolume.Save();
                        break;

                    case DefaultAudioMixerParameters.BgmVolume:
                        UISound.Instance.BgmVolume.Value = value;
                        UISound.Instance.BgmVolume.Save();
                        break;

                    case DefaultAudioMixerParameters.SfxVolume:
                        UISound.Instance.SfxVolume.Value = value;
                        UISound.Instance.SfxVolume.Save();
                        break;

                    default:
                        break;
                }
            }
        }

        public override void RevertToDefault()
        {
            if (UISound.Instance != null)
            {
                switch (targetParameter)
                {
                    case DefaultAudioMixerParameters.MasterVolume:
                        UISound.Instance.MasterVolume.DefaultValue = defaultValue;
                        UISound.Instance.MasterVolume.LoadDefaultValue();
                        UISound.Instance.MasterVolume.Save();

                        // Fetch UI value.
                        SliderComponent.value = UISound.Instance.MasterVolume.Value;
                        break;

                    case DefaultAudioMixerParameters.BgmVolume:
                        UISound.Instance.BgmVolume.DefaultValue = defaultValue;
                        UISound.Instance.BgmVolume.LoadDefaultValue();
                        UISound.Instance.BgmVolume.Save();

                        // Fetch UI value.
                        SliderComponent.value = UISound.Instance.BgmVolume.Value;
                        break;

                    case DefaultAudioMixerParameters.SfxVolume:
                        UISound.Instance.SfxVolume.DefaultValue = defaultValue;
                        UISound.Instance.SfxVolume.LoadDefaultValue();
                        UISound.Instance.SfxVolume.Save();

                        // Fetch UI value.
                        SliderComponent.value = UISound.Instance.SfxVolume.Value;
                        break;

                    default:
                        break;
                }
            }
        }
    }
}