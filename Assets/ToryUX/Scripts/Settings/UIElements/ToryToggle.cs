using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ToryUX
{
    [System.Serializable]
    public class ToryToggleIntEvent : UnityEvent<ToryValue.ToryInt>
    {}

    [System.Serializable]
    public class ToggleEvent : UnityEvent<int>
    {}

    public class ToryToggle : Button, IDefaultValueSetter<int>
    {
        public string[] options;

        private int currentOptionIndex;
        public int CurrentOptionIndex
        {
            get
            {
                return currentOptionIndex;
            }
            set
            {
                currentOptionIndex = value;
                UpdateValue();
            }
        }

        public string SelectedOption
        {
            get
            {
                return options[CurrentOptionIndex];
            }
        }

        public ToggleEvent onToggle = new ToggleEvent();

        public Text labelText;
        public Text valueText;

        private RectTransform rectTransform;

        [SerializeField]
        public ToryToggleIntEvent toryToggleIntProperty;
        public List<ToryValue.ToryInt> boundToryInts;

        bool hasBoundToryValue;

        public int DefaultValue
        {
            get
            {
                if (hasBoundToryValue)
                {
                    return boundToryInts[0].DefaultValue;
                }
                return 0;
            }
            set
            {
                if (hasBoundToryValue)
                {
                    for (int i = 0; i < boundToryInts.Count; i++)
                    {
                        boundToryInts[i].DefaultValue = value;
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

			onClick.AddListener(Toggle);
			BindToryValue();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			onClick.RemoveAllListeners();
		}

		public void BindToryValue()
        {
            boundToryInts = new List<ToryValue.ToryInt>();

            for (int i = 0; i < toryToggleIntProperty.GetPersistentEventCount(); i++)
            {
                if (toryToggleIntProperty.GetPersistentTarget(i) != null && !string.IsNullOrEmpty(toryToggleIntProperty.GetPersistentMethodName(i)))
                {
                    try
                    {
                        string propertyName = toryToggleIntProperty.GetPersistentMethodName(i).Substring(4);
                        boundToryInts.Add((ToryValue.ToryInt) toryToggleIntProperty.GetPersistentTarget(i).GetType().GetProperty(propertyName).GetValue(toryToggleIntProperty.GetPersistentTarget(i), null));
                    }
                    catch (UnityException e)
                    {
                        Debug.LogError("Binding ToryValue failed by " + e);
                    }
                }
            }

            hasBoundToryValue = boundToryInts.Count > 0;
			
			if (hasBoundToryValue && PlayerPrefsElite.key != null)
			{
				boundToryInts[0].LoadSavedValue();
				CurrentOptionIndex = boundToryInts[0].Value;
				
				UpdateValue();
			}
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (hasBoundToryValue)
            {
                if (PlayerPrefsElite.key != null)
                {
                    boundToryInts[0].LoadSavedValue();
                    CurrentOptionIndex = boundToryInts[0].Value;
                }
                UpdateValue();
            }
        }

        public void RevertToDefault()
        {
            if (hasBoundToryValue)
            {
                CurrentOptionIndex = boundToryInts[0].DefaultValue;
                UpdateValue();
            }
        }

        void FetchTextObjects()
        {
            try
            {
                labelText = transform.Find("Label").GetComponent<Text>();;
            }
            catch
            {
                Debug.LogErrorFormat("ToryToggle {0} cannot find Text object named \"Label\" among its children.", name);
            }
            if (labelText == null)
            {
                Debug.LogErrorFormat("ToryToggle {0} cannot find Text object named \"Label\" among its children.", name);
            }

            try
            {
                valueText = transform.Find("Value").GetComponent<Text>();
            }
            catch
            {
                Debug.LogErrorFormat("ToryToggle {0} cannot find Text object named \"Value\" among its children.", name);
            }
            if (valueText == null)
            {
                Debug.LogErrorFormat("ToryToggle {0} cannot find Text object named \"Value\" among its children.", name);
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
            if (valueText == null)
            {
                FetchTextObjects();
            }
            #endif

            try
            {
                valueText.text = SelectedOption;
            }
            catch
            {
                valueText.text = "-";
            }

            if (hasBoundToryValue)
            {
                if (PlayerPrefsElite.key != null)
                {
                    boundToryInts[0].Value = CurrentOptionIndex;
                    boundToryInts[0].Save();
                }
            }
        }

        public void SetOptionIndexSimple(int index)
        {
            SetOptionIndex(index, true);
        }

        public bool SetOptionIndex(int index, bool shouldTriggerEvent = true)
        {
            if (index >= 0 && index < options.Length)
            {
                CurrentOptionIndex = index;
                if (shouldTriggerEvent)
                {
                    onToggle.Invoke(CurrentOptionIndex);
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public string GetOptionText(int index)
        {
            try
            {
                return options[index];
            }
            #pragma warning disable 0168
            catch (System.IndexOutOfRangeException e)
            {
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogError(e);
                #endif
            }
            return null;
        }

		public void Toggle()
		{
			if (CurrentOptionIndex >= options.Length - 1)
			{
				CurrentOptionIndex = 0;
			}
			else
			{
				CurrentOptionIndex += 1;
			}
			onToggle.Invoke(CurrentOptionIndex);

			UpdateValue();

			if (SettingsUI.Instance.toggleSound != null && interactable)
			{
				UISound.Play(SettingsUI.Instance.toggleSound);
			}
		}

		public void ToggleReverse()
		{
			if (CurrentOptionIndex <= 0)
			{
				CurrentOptionIndex = options.Length - 1;
			}
			else
			{
				CurrentOptionIndex -= 1;
			}
			onToggle.Invoke(CurrentOptionIndex);

			if (SettingsUI.Instance.toggleSound != null && interactable)
			{
				UISound.Play(SettingsUI.Instance.toggleSound);
			}

			UpdateValue();
		}

		public override void OnMove(AxisEventData eventData)
		{
			if (eventData.moveDir == MoveDirection.Left)
			{
				ToggleReverse();
			}
			else if (eventData.moveDir == MoveDirection.Right)
			{
				Toggle();
			}
			else
			{
				base.OnMove(eventData);
			}
		}
	}
}