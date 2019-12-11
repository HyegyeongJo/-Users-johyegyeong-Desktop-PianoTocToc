using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    [RequireComponent(typeof(Image))]
    public class HSVController : MonoBehaviour
    {
        [Range(0, 1f)] public float hue;
        [Range(0, 1f)] public float saturation;
        [Range(0, 1f)] public float value;

        private Image image;

        void Awake()
        {
            image = GetComponent<Image>();
        }

        void Update()
        {
            image.color = Color.HSVToRGB(hue, saturation, value);
        }
    }
}