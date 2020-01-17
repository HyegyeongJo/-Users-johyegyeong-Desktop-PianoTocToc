using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ToryUX
{
    [System.Serializable]
    public class ToryInputFieldStringEvent : UnityEvent<ToryValue.ToryString>
    {}

    public class ToryInputField : InputField, IDefaultValueSetter<string>
    {
        public Text labelText;
        public RectTransform boxRectTransform;

        private RectTransform rectTransform;

        private bool justSelected = false;
        private bool beenFocusedAwhile = false;
        private Color originalSelectionColor;
        private Color originalCaretColor;

        [SerializeField]
        public ToryInputFieldStringEvent toryStringProperty;
        public List<ToryValue.ToryString> boundToryStrings;

        bool hasBoundToryValue;

        public string DefaultValue
        {
            get
            {
                if (hasBoundToryValue)
                {
                    return boundToryStrings[0].DefaultValue;
                }
                return string.Empty;
            }
            set
            {
                if (hasBoundToryValue)
                {
                    for (int i = 0; i < boundToryStrings.Count; i++)
                    {
                        boundToryStrings[i].DefaultValue = value;
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

			onEndEdit.AddListener(UpdateValue);
			originalSelectionColor = selectionColor;
			originalCaretColor = caretColor;
			BindToryValue();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			onEndEdit.RemoveAllListeners();
		}

		public void BindToryValue()
        {
            boundToryStrings = new List<ToryValue.ToryString>();

            for (int i = 0; i < toryStringProperty.GetPersistentEventCount(); i++)
            {
                if (toryStringProperty.GetPersistentTarget(i) != null && !string.IsNullOrEmpty(toryStringProperty.GetPersistentMethodName(i)))
                {
                    try
                    {
                        string propertyName = toryStringProperty.GetPersistentMethodName(i).Substring(4);
                        boundToryStrings.Add((ToryValue.ToryString) toryStringProperty.GetPersistentTarget(i).GetType().GetProperty(propertyName).GetValue(toryStringProperty.GetPersistentTarget(i), null));
                    }
                    catch (UnityException e)
                    {
                        Debug.LogError("Binding ToryValue failed by " + e);
                    }
                }
            }

            hasBoundToryValue = boundToryStrings.Count > 0;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (hasBoundToryValue)
            {
                if (PlayerPrefsElite.key != null)
                {
                    boundToryStrings[0].LoadSavedValue();
                    text = boundToryStrings[0].Value;
                }
                UpdateValue(text);
            }
        }

        public void RevertToDefault()
        {
            if (hasBoundToryValue)
            {
                text = boundToryStrings[0].DefaultValue;
                UpdateValue(text);
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
                Debug.LogErrorFormat("ToryInputField {0} cannot find Text object named \"Label\" among its children.", name);
            }
            if (labelText == null)
            {
                Debug.LogErrorFormat("ToryInputField {0} cannot find Text object named \"Label\" among its children.", name);
            }

            try
            {
                boxRectTransform = transform.Find("Box").GetComponent<RectTransform>();
            }
            catch
            {
                Debug.LogErrorFormat("ToryInputField {0} cannot find GameObject named \"\").\" among its children.", name);
            }
            if (boxRectTransform == null)
            {
                Debug.LogErrorFormat("ToryInputField {0} cannot find GameObject named \"Label\" among its children.", name);
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

			justSelected = true;
			selectionColor = Color.clear;
			caretColor = Color.clear;
		}

		public override void OnUpdateSelected(BaseEventData eventData)
        {
            base.OnUpdateSelected(eventData);
            if (justSelected)
            {
                DeactivateInputField();
                justSelected = false;
                beenFocusedAwhile = false;
                selectionColor = originalSelectionColor;
                caretColor = originalCaretColor;
            }
            else if (isFocused)
            {
                if (!beenFocusedAwhile)
                {
                    SettingsUI.PreventEscapeKeyFromClosingSettingsUI = true;
                    beenFocusedAwhile = true;
                    if (SettingsUI.Instance != null)
                    {
                        PlayStartSfx();
                    }
                }
            }
            else if (Input.GetButtonDown("Cancel"))
            {
                // HACK: This takes up one frame before executing the next block, effectively delaying one frame.
            }
            else if (SettingsUI.PreventEscapeKeyFromClosingSettingsUI)
            {
                // HACK: Doing this without the block above will cause immediate closing of settings UI panel on ESC hit.
                SettingsUI.PreventEscapeKeyFromClosingSettingsUI = false;
            }
        }

        public void UpdateValue(string text)
        {
            if (!justSelected)
            {
                beenFocusedAwhile = false;
                if (Application.isPlaying && SettingsUI.Instance != null)
                {
                    PlayEndSfx(wasCanceled);
                }
            }

            this.text = text;

            if (hasBoundToryValue)
            {
                if (PlayerPrefsElite.key != null)
                {
                    boundToryStrings[0].Value = text;
                    boundToryStrings[0].Save();
                }
            }
        }

        public void PlayStartSfx()
        {
            if (SettingsUI.Instance.inputFieldFocusingSound != null && interactable)
            {
                UISound.Play(SettingsUI.Instance.inputFieldFocusingSound);
            }
        }

        public void PlayEndSfx(bool wasCanceled)
        {
            if (wasCanceled)
            {
                if (SettingsUI.Instance.inputFieldCancelSound != null)
                {
                    UISound.Play(SettingsUI.Instance.inputFieldCancelSound);
                }
            }
            else
            {
                if (SettingsUI.Instance.inputfieldSubmitSound != null)
                {
                    UISound.Play(SettingsUI.Instance.inputfieldSubmitSound);
                }
            }
        }
    }
}