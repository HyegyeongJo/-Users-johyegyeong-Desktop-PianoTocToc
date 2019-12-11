using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ToryUX
{
    public class ToryConsole : Selectable
    {
        public ToryConsoleItem consoleItemPrefab;
        public Transform itemsContainer;

        public ScrollRect ScrollRect
        {
            get
            {
                if (scrollRect == null)
                {
                    scrollRect = GetComponent<ScrollRect>();
                }
                return scrollRect;
            }
        }
        private ScrollRect scrollRect;

        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                {
                    rectTransform = GetComponent<RectTransform>();
                }
                return rectTransform;
            }
        }
        private RectTransform rectTransform;

        protected override void Awake()
        {
            base.Awake();
        }

        public void Initialize(int itemCount)
        {

            // Destroy dummy items.
            for (int i = itemsContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(itemsContainer.GetChild(i).gameObject);
            }

            // Instantiate log items.
            for (int i = 0; i < itemCount; i++)
            {
                var item = Instantiate(consoleItemPrefab, itemsContainer);
                item.Message.fontSize = ToryConsoleSetup.DefaultFontSize;
                item.gameObject.SetActive(false);
            }
        }

        public void HandleLog(string message, string stackTrace, LogType type)
        {
            var newLog = itemsContainer.GetChild(itemsContainer.childCount - 1).GetComponent<ToryConsoleItem>();
            bool shouldAddStackTrace;
            switch (type)
            {
                case LogType.Log:
                    newLog.Background.color = ToryConsoleSetup.RegularColor;
                    shouldAddStackTrace = ToryConsoleSetup.ShowStackTraceForRegular;
                    break;
                case LogType.Warning:
                    newLog.Background.color = ToryConsoleSetup.WarningColor;
                    shouldAddStackTrace = ToryConsoleSetup.ShowStackTraceForWarning;
                    break;
                case LogType.Error:
                    newLog.Background.color = ToryConsoleSetup.ErrorColor;
                    shouldAddStackTrace = ToryConsoleSetup.ShowStackTraceForError;
                    break;
                case LogType.Exception:
                    newLog.Background.color = ToryConsoleSetup.ExceptionColor;
                    shouldAddStackTrace = ToryConsoleSetup.ShowStackTraceForException;
                    break;
                case LogType.Assert:
                    newLog.Background.color = ToryConsoleSetup.AssertColor;
                    shouldAddStackTrace = ToryConsoleSetup.ShowStackTraceForAssert;
                    break;
                default:
                    newLog.Background.color = ToryConsoleSetup.RegularColor;
                    shouldAddStackTrace = ToryConsoleSetup.ShowStackTraceForRegular;
                    break;
            }
            if (shouldAddStackTrace)
            {
                newLog.Message.text = string.Format("{0}\n<size={1}>{2}</size>", message, ToryConsoleSetup.SmallerFontSize, stackTrace);
            }
            else
            {
                newLog.Message.text = message;
            }
            newLog.transform.SetAsFirstSibling();
            newLog.gameObject.SetActive(true);

            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
            {
                if (!EventSystem.current.currentSelectedGameObject.Equals(ScrollRect.verticalScrollbar.gameObject))
                {
                    ScrollRect.verticalScrollbar.value = 1f;
                }
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.delta.magnitude > 0)
            {
                base.OnPointerEnter(eventData);
                EventSystem.current.SetSelectedGameObject(gameObject);
            }
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            if (SettingsUI.Instance.selectionSound != null)
            {
                UISound.Play(SettingsUI.Instance.selectionSound);
            }
            SettingsUI.ScrollTo(RectTransform);
            keyInputMonitorCoroutine = StartCoroutine(KeyInputMonitorCoroutine());
        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
            if (keyInputMonitorCoroutine != null)
            {
                StopCoroutine(keyInputMonitorCoroutine);
            }
        }

        Coroutine keyInputMonitorCoroutine;
        IEnumerator KeyInputMonitorCoroutine()
        {
            while (true)
            {
                if (Input.GetButtonDown("Submit") && ScrollRect.verticalScrollbar.size < 0.99f)
                {
                    SettingsUI.PreventEscapeKeyFromClosingSettingsUI = true;
                    ScrollRect.verticalScrollbar.Select();
                    break;
                }
                yield return null;
            }
            while (true)
            {
                if (Input.GetButtonDown("Cancel"))
                {
                    Select();
                    break;
                }
                yield return null;
            }

            SettingsUI.PreventEscapeKeyFromClosingSettingsUI = false;
            keyInputMonitorCoroutine = null;
        }
    }
}