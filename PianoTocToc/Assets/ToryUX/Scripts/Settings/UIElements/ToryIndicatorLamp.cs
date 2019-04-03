using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ToryUX
{
    [ExecuteInEditMode]
    public class ToryIndicatorLamp : UIBehaviour
    {
        public enum IndicatorLampState
        {
            Off,
            Green,
            Yellow,
            Red
        }

        /// <summary>
        /// Sets/gets current indicator lamp state.
        /// Default value is <c>IndicatorLampState.Off</c>.
        /// </summary>
        public IndicatorLampState CurrentIndicatorLampState
        {
            get
            {
                return currentIndicatorLampState;
            }
            set
            {
                currentIndicatorLampState = value;
                TryUpdateState();
            }
        }
        [SerializeField] private IndicatorLampState currentIndicatorLampState = IndicatorLampState.Off;

        public Image offImageObject;
        public Image greenImageObject;
        public Image yellowImageObject;
        public Image redImageObject;
        public Text labelText;

        public bool showDifferentMessageForDifferentState = false;
        public string label;
        public string offMessage;
        public string greenMessage;
        public string yellowMessage;
        public string redMessage;

        public Color offTextColor = new Color(1f, 1f, 1f, 0.392f);
        public Color onTextColor = Color.white;

        protected override void Awake()
        {
            base.Awake ();
            FetchObjects();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            TryUpdateState();
        }

        void FetchObjects()
        {
            try
            {
                labelText = transform.Find("Label").GetComponent<Text>();;
            }
            catch
            {
                Debug.LogErrorFormat("ToryIndicatorLamp {0} cannot find Text object named \"Label\" among its children.", name);
            }
            if (labelText == null)
            {
                Debug.LogErrorFormat("ToryIndicatorLamp {0} cannot find Text object named \"Label\" among its children.", name);
            }

            try
            {
                offImageObject = transform.Find("Off").GetComponent<Image>();
            }
            catch
            {
                Debug.LogErrorFormat("ToryIndicatorLamp {0} cannot find Image object named \"Off\" among its children.", name);
            }
            if (offImageObject == null)
            {
                Debug.LogErrorFormat("ToryIndicatorLamp {0} cannot find Image object named \"Off\" among its children.", name);
            }

            try
            {
                greenImageObject = transform.Find("Green").GetComponent<Image>();
            }
            catch
            {
                Debug.LogErrorFormat("ToryIndicatorLamp {0} cannot find Image object named \"Green\" among its children.", name);
            }
            if (greenImageObject == null)
            {
                Debug.LogErrorFormat("ToryIndicatorLamp {0} cannot find Image object named \"Green\" among its children.", name);
            }

            try
            {
                yellowImageObject = transform.Find("Yellow").GetComponent<Image>();
            }
            catch
            {
                Debug.LogErrorFormat("ToryIndicatorLamp {0} cannot find Image object named \"Yellow\" among its children.", name);
            }
            if (yellowImageObject == null)
            {
                Debug.LogErrorFormat("ToryIndicatorLamp {0} cannot find Image object named \"Yellow\" among its children.", name);
            }

            try
            {
                redImageObject = transform.Find("Red").GetComponent<Image>();
            }
            catch
            {
                Debug.LogErrorFormat("ToryIndicatorLamp {0} cannot find Image object named \"Red\" among its children.", name);
            }
            if (redImageObject == null)
            {
                Debug.LogErrorFormat("ToryIndicatorLamp {0} cannot find Image object named \"Red\" among its children.", name);
            }
        }

        #if UNITY_EDITOR
        void Update()
        {
            if (!Application.isPlaying)
            {
                TryUpdateState();
            }
        }
        #endif

        void UpdateState()
        {
            offImageObject.enabled = (CurrentIndicatorLampState == IndicatorLampState.Off);
            greenImageObject.enabled = (CurrentIndicatorLampState == IndicatorLampState.Green);
            yellowImageObject.enabled = (CurrentIndicatorLampState == IndicatorLampState.Yellow);
            redImageObject.enabled = (CurrentIndicatorLampState == IndicatorLampState.Red);
            if (CurrentIndicatorLampState == IndicatorLampState.Off)
            {
                labelText.color = offTextColor;
            }
            else
            {
                labelText.color = onTextColor;
            }

            if (showDifferentMessageForDifferentState)
            {
                switch (CurrentIndicatorLampState)
                {
                    case IndicatorLampState.Off:
                        labelText.text = offMessage;
                        break;
                    case IndicatorLampState.Green:
                        labelText.text = greenMessage;
                        break;
                    case IndicatorLampState.Yellow:
                        labelText.text = yellowMessage;
                        break;
                    case IndicatorLampState.Red:
                        labelText.text = redMessage;
                        break;
                    default:
                        labelText.text = label;
                        break;
                }
            }
            else
            {
                labelText.text = label;
            }
        }

        public void TryUpdateState()
        {
            try
            {
                UpdateState();
            }
            catch (System.NullReferenceException)
            {
                FetchObjects();
                UpdateState();
            }
        }

        /// <summary>
        /// Turns the indicator lamp off.
        /// </summary>
        public void TurnOff()
        {
            CurrentIndicatorLampState = IndicatorLampState.Off;
        }

        /// <summary>
        /// Turns the indicator lamp on green.
        /// </summary>
        public void TurnOnGreen()
        {
            CurrentIndicatorLampState = IndicatorLampState.Green;
        }

        /// <summary>
        /// Turns the indicator lamp on yellow.
        /// </summary>
        public void TurnOnYellow()
        {
            CurrentIndicatorLampState = IndicatorLampState.Yellow;
        }

        /// <summary>
        /// Turns the indicator lamp on red.
        /// </summary>
        public void TurnOnRed()
        {
            CurrentIndicatorLampState = IndicatorLampState.Red;
        }
    }
}