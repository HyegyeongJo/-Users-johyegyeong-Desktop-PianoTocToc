using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ToryUX
{
    [System.Serializable]
    public class TorySimpleBoolEvent : UnityEvent<ToryValue.ToryBool>
    {}

    public class TorySimpleToggle : Toggle, IDefaultValueSetter<bool>
    {
		public Image untickedObject;
		public Image tickedObject;
		public Text labelText;
		public Text valueText;

		public string onText;
		public string offText;

		public Color onTextColor = Color.black;
		public Color offTextColor = new Color(1f, 1f, 1f, 0.392f);

		private RectTransform rectTransform;

		[SerializeField]
		public TorySimpleBoolEvent toryBoolProperty;
		public List<ToryValue.ToryBool> boundToryBools;

		bool hasBoundToryValue;

		public bool DefaultValue
		{
			get
			{
				if (hasBoundToryValue)
				{
					return boundToryBools[0].DefaultValue;
				}
				return false;
			}
			set
			{
				if (hasBoundToryValue)
				{
					for (int i = 0; i < boundToryBools.Count; i++)
					{
						boundToryBools[i].DefaultValue = value;
					}
				}
			}
		}

		protected override void Awake()
		{
			base.Awake();
			FetchObjects();
			rectTransform = GetComponent<RectTransform>();
		}

		protected override void Start()
		{
			base.Start();

			onValueChanged.AddListener(Toggle);
			BindToryValue();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			onValueChanged.RemoveAllListeners();
		}

		public void BindToryValue(bool updateSavedValue = true)
		{
			boundToryBools = new List<ToryValue.ToryBool>();

			for (int i = 0; i < toryBoolProperty.GetPersistentEventCount(); i++)
			{
				if (toryBoolProperty.GetPersistentTarget(i) != null && !string.IsNullOrEmpty(toryBoolProperty.GetPersistentMethodName(i)))
				{
					try
					{
						string propertyName = toryBoolProperty.GetPersistentMethodName(i).Substring(4);
						boundToryBools.Add((ToryValue.ToryBool)toryBoolProperty.GetPersistentTarget(i).GetType().GetProperty(propertyName).GetValue(toryBoolProperty.GetPersistentTarget(i), null));
					}
					catch (UnityException e)
					{
						Debug.LogError("Binding ToryValue failed by " + e);
					}
				}
			}

			hasBoundToryValue = boundToryBools.Count > 0;
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			if (PlayerPrefsElite.key != null)
			{
				if (hasBoundToryValue)
				{
					boundToryBools[0].LoadSavedValue();
					isOn = boundToryBools[0].Value;
					UpdateValue();
				}
			}
		}

		public void RevertToDefault()
		{
			if (hasBoundToryValue)
			{
				isOn = boundToryBools[0].DefaultValue;
				UpdateValue();
			}
		}

		void FetchObjects()
		{
			try
			{
				labelText = transform.Find("Label").GetComponent<Text>();
			}
			catch
			{
				Debug.LogErrorFormat("TorySimpleToggle {0} cannot find Text object named \"Label\" among its children.", name);
			}
			if (labelText == null)
			{
				Debug.LogErrorFormat("TorySimpleToggle {0} cannot find Text object named \"Label\" among its children.", name);
			}

			try
			{
				tickedObject = transform.Find("Ticked").GetComponent<Image>();
			}
			catch
			{
				Debug.LogErrorFormat("TorySimpleToggle {0} cannot find Image object named \"Ticked\" among its children.", name);
			}
			if (tickedObject == null)
			{
				Debug.LogErrorFormat("TorySimpleToggle {0} cannot find Image object named \"Ticked\" among its children.", name);
			}
			else
			{
				try
				{
					valueText = tickedObject.transform.Find("Text").GetComponent<Text>();
				}
				catch
				{
					Debug.LogErrorFormat("TorySimpleToggle {0} cannot find Text object named \"Ticked/Text\" among its children.", name);
				}
				if (valueText == null)
				{
					Debug.LogErrorFormat("TorySimpleToggle {0} cannot find Text object named \"Ticked/Text\" among its children.", name);
				}
			}

			try
			{
				untickedObject = transform.Find("Unticked").GetComponent<Image>();
			}
			catch
			{
				Debug.LogErrorFormat("TorySimpleToggle {0} cannot find Image object named \"Unticked\" among its children.", name);
			}
			if (untickedObject == null)
			{
				Debug.LogErrorFormat("TorySimpleToggle {0} cannot find Image object named \"Unticked\" among its children.", name);
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

		public void UpdateValue()
		{
#if UNITY_EDITOR
			if (tickedObject == null || untickedObject == null || valueText == null)
			{
				FetchObjects();
			}
#endif

			tickedObject.enabled = isOn;
			untickedObject.enabled = !isOn;

			if (isOn)
			{
				valueText.text = onText;
				valueText.color = onTextColor;
			}
			else
			{
				valueText.text = offText;
				valueText.color = offTextColor;
			}

			if (hasBoundToryValue)
			{
				if (PlayerPrefsElite.key != null)
				{
					boundToryBools[0].Value = isOn;
					boundToryBools[0].Save();
				}
			}
		}

		public void Toggle(bool value)
		{
			UpdateValue();

			if (Application.isPlaying && SettingsUI.Instance != null && SettingsUI.Instance.toggleSound != null && interactable)
			{
				UISound.Play(SettingsUI.Instance.toggleSound);
			}
		}
	}
}