using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ToryUX
{
    [System.Serializable]
    public class TorySliderFloatEvent : UnityEvent<ToryValue.ToryFloat>
    {}

    [System.Serializable]
    public class TorySliderIntEvent : UnityEvent<ToryValue.ToryInt>
    {}

    public class TorySlider : Slider, IDefaultValueSetter<object>
    {
        public int stepCounts = 10;

        public float valueMultiplier = 1f;
        public string valueFormat = "F2";
        public string valueUnit;

        public Text labelText;
        public Text valueText;

        private RectTransform rectTransform;

        [SerializeField]
        public BindToryValueType bindToryValueType;
        [SerializeField]
        public TorySliderFloatEvent toryFloatProperty;
        [SerializeField]
        public TorySliderIntEvent toryIntProperty;

        public List<ToryValue.ToryFloat> boundToryFloats;
        public List<ToryValue.ToryInt> boundToryInts;

        bool hasBoundToryValue;

        public object DefaultValue
        {
            get
            {
                if (hasBoundToryValue)
                {
                    if (bindToryValueType == BindToryValueType.Float)
                    {
                        return boundToryFloats[0].DefaultValue;
                    }
                    else
                    {
                        return boundToryInts[0].DefaultValue;
                    }
                }
                return null;
            }
            set
            {
                if (hasBoundToryValue)
                {
                    if (bindToryValueType == BindToryValueType.Float)
                    {
                        for (int i = 0; i < boundToryFloats.Count; i++)
                        {
                            boundToryFloats[i].DefaultValue = (float) value;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < boundToryInts.Count; i++)
                        {
                            boundToryInts[i].DefaultValue = (int) value;
                        }
                    }
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            FetchTextObjects();
            rectTransform = GetComponent<RectTransform>();
        }

		protected override void Start()
		{
			base.Start();

			onValueChanged.AddListener(UpdateValue);
			BindToryValue();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			onValueChanged.RemoveAllListeners();
		}

		public void BindToryValue()
        {
            boundToryFloats = new List<ToryValue.ToryFloat>();
            boundToryInts = new List<ToryValue.ToryInt>();

            if (bindToryValueType == BindToryValueType.Float)
            {
                BindToryValue<ToryValue.ToryFloat>(ref boundToryFloats, toryFloatProperty);
            }
            else if (bindToryValueType == BindToryValueType.Int)
            {
                BindToryValue<ToryValue.ToryInt>(ref boundToryInts, toryIntProperty);
            }

            hasBoundToryValue = boundToryFloats.Count > 0 || boundToryInts.Count > 0;
            if (hasBoundToryValue)
            {
                UpdateSliderValue();
            }
            UpdateValue(value, true);
        }

        void BindToryValue<ToryValueType>(ref List<ToryValueType> toryValueArray, UnityEvent<ToryValueType> torySliderValueEvent)
        {
            for (int i = 0; i < torySliderValueEvent.GetPersistentEventCount(); i++)
            {
                if (torySliderValueEvent.GetPersistentTarget(i) != null && !string.IsNullOrEmpty(torySliderValueEvent.GetPersistentMethodName(i)))
                {
                    try
                    {
                        string propertyName = torySliderValueEvent.GetPersistentMethodName(i).Substring(4);
                        toryValueArray.Add((ToryValueType) torySliderValueEvent.GetPersistentTarget(i).GetType().GetProperty(propertyName).GetValue(torySliderValueEvent.GetPersistentTarget(i), null));
                    }
                    catch (UnityException e)
                    {
                        Debug.LogError("Binding ToryValue failed by " + e);
                    }
                }
            }
        }

        void UpdateSliderValue()
        {
            if (PlayerPrefsElite.key != null)
            {
                if (bindToryValueType == BindToryValueType.Float)
                {
                    wholeNumbers = false;

                    for (int i = 0; i < boundToryFloats.Count; i++)
                    {
                        boundToryFloats[i].LoadSavedValue();
                        if (boundToryFloats[i].Value < minValue)
                        {
                            minValue = boundToryFloats[i].Value;
                        }
                        if (boundToryFloats[i].Value > maxValue)
                        {
                            maxValue = boundToryFloats[i].Value;
                        }
                        value = boundToryFloats[i].Value;
                        boundToryFloats[i].Save();
                    }
                }

                if (bindToryValueType == BindToryValueType.Int)
                {
                    wholeNumbers = true;

                    for (int i = 0; i < boundToryInts.Count; i++)
                    {
                        boundToryInts[i].LoadSavedValue();
                        if (boundToryInts[i].Value < minValue)
                        {
                            minValue = boundToryInts[i].Value;
                        }
                        if (boundToryInts[i].Value > maxValue)
                        {
                            maxValue = boundToryInts[i].Value;
                        }
                        value = boundToryInts[i].Value;
                        boundToryInts[i].Save();
                    }
                }
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (PlayerPrefsElite.key != null)
            {
                if (hasBoundToryValue)
                {
                    if (bindToryValueType == BindToryValueType.Float)
                    {
                        boundToryFloats[0].LoadSavedValue();
                        value = boundToryFloats[0].Value;
                        UpdateValue(value, true);
                    }
                    else if (bindToryValueType == BindToryValueType.Int)
                    {
                        boundToryInts[0].LoadSavedValue();
                        value = boundToryInts[0].Value;
                        UpdateValue(value, true);
                    }
                }
            }
        }

        public void RevertToDefault()
        {
            if (hasBoundToryValue)
            {
                if (bindToryValueType == BindToryValueType.Float)
                {
                    value = (float) DefaultValue;
                    UpdateValue(value, true);
                }
                else if (bindToryValueType == BindToryValueType.Int)
                {
                    value = (int) DefaultValue;
                    UpdateValue(value, true);
                }
            }
        }

        void FetchTextObjects()
        {
            try
            {
                labelText = transform.Find("Label").GetComponent<Text>();
            }
            catch
            {
                Debug.LogErrorFormat("TorySlider {0} cannot find Text object named \"Label\" among its children.", name);
            }
            if (labelText == null)
            {
                Debug.LogErrorFormat("TorySlider {0} cannot find Text object named \"Label\" among its children.", name);
            }

            try
            {
                valueText = transform.Find("Value").GetComponent<Text>();
            }
            catch
            {
                Debug.LogErrorFormat("TorySlider {0} cannot find Text object named \"Value\" among its children.", name);
            }
            if (valueText == null)
            {
                Debug.LogErrorFormat("TorySlider {0} cannot find Text object named \"Value\" among its children.", name);
            }
        }

		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
			if (eventData.delta.magnitude > 0)
			{
				EventSystem.current.SetSelectedGameObject(gameObject);
			}
		}

		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			if (SettingsUI.Instance.selectionSound != null && interactable)
			{
				UISound.Play(SettingsUI.Instance.selectionSound);
			}
			SettingsUI.ScrollTo(rectTransform);
		}

		public void UpdateValue(float value, bool muteSfx)
        {
            #if UNITY_EDITOR
            if (valueText == null)
            {
                FetchTextObjects();
            }
            #endif

            try
            {
                if (valueFormat.ToLower().StartsWith("d") || valueFormat.ToLower().StartsWith("x"))
                {
                    valueText.text = string.Format("{0}{1}", (Mathf.RoundToInt(value * valueMultiplier)).ToString(valueFormat), valueUnit);
                }
                else
                {
                    valueText.text = string.Format("{0}{1}", (value * valueMultiplier).ToString(valueFormat), valueUnit);
                }
            }
            catch (System.FormatException e)
            {
                Debug.LogError(e);
                valueText.text = "-";
            }

            if (Application.isPlaying)
            {
                try
                {
                    if (!muteSfx && SettingsUI.Instance.slideSound != null && interactable)
                    {
                        UISound.Play(SettingsUI.Instance.slideSound);
                    }
                }
                catch
                {}
            }

            if (hasBoundToryValue)
            {
                if (PlayerPrefsElite.key != null)
                {
                    if (bindToryValueType == BindToryValueType.Float)
                    {
                        boundToryFloats[0].Value = (float) value;
                        boundToryFloats[0].Save();
                    }
                    else if (bindToryValueType == BindToryValueType.Int)
                    {
                        boundToryInts[0].Value = (int) value;
                        boundToryInts[0].Save();
                    }
                }
            }
        }

        public void UpdateValue(float value)
        {
            UpdateValue(value, false);
        }

		public override void OnMove(AxisEventData eventData)
		{
			if (eventData.moveDir == MoveDirection.Left)
			{
				value -= (maxValue - minValue) / (float)stepCounts;
			}
			else if (eventData.moveDir == MoveDirection.Right)
			{
				value += (maxValue - minValue) / (float)stepCounts;
			}
			else
			{
				base.OnMove(eventData);
			}
		}
	}
}