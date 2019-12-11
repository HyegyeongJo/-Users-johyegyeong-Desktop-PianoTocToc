using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    [RequireComponent(typeof(Image))]
    public class ToryConsoleItem : MonoBehaviour
    {
        public Image Background
        {
            get
            {
                if (background == null)
                {
                    background = GetComponent<Image>();
                }
                return background;
            }
        }
        private Image background;

        public Text Message
        {
            get
            {
                if (message == null)
                {
                    message = GetComponentInChildren<Text>();
                }
                return message;
            }
        }
        private Text message;
    }
}