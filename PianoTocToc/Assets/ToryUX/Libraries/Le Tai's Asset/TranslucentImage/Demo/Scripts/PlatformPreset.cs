using System;
using UnityEngine;
using UnityEngine.UI;

namespace LeTai.Asset.TranslucentImage.Demo
{
    public class PlatformPreset : MonoBehaviour
    {
        public Preset[] presets;
        // Use this for initialization
        void Start ()
        {
            Slider iterationSlider = GameObject.Find("IterationSlider").GetComponent<Slider>();
            Slider downsampleSlider = GameObject.Find("DownsampleSlider").GetComponent<Slider>();
            InputField maxUpdateRateField = GameObject.Find("UpdateRateInputField").GetComponent<InputField>();

            foreach (Preset preset in presets)
            {
                if (preset.platform == Application.platform)
                {
                    iterationSlider.value = preset.iteration;
                    downsampleSlider.value = preset.downsample;
                    maxUpdateRateField.text = preset.maxUpdateRate.ToString();
                }

            }
        }
    }

    [Serializable]
    public struct Preset
    {
        public RuntimePlatform platform;
        public int iteration;
        public int downsample;
        public float maxUpdateRate;
    }
}