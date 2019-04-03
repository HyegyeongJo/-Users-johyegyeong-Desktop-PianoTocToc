using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ToryUX
{
    public class SettingsUI : MonoBehaviour
    {
        #region Singleton
        static volatile SettingsUI instance;
        static readonly object syncRoot = new object();

        public static SettingsUI Instance
        {
            get
            {
                if (instance == null)
                {
                    lock(syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = FindObjectOfType<SettingsUI>();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        public static bool IsShown
        {
            get
            {
                if (Instance.advancedSettingsPanel != null)
                {
                    return Instance.basicSettingsPanel.activeInHierarchy || Instance.advancedSettingsPanel.activeInHierarchy;
                }
                else
                {
                    return Instance.basicSettingsPanel.activeInHierarchy;
                }
            }
        }

        public static bool PreventEscapeKeyFromClosingSettingsUI
        {
            get;
            set;
        }

        private RectTransform rectTransform;
        public Scrollbar scrollbar;

        [Space]
        public GameObject basicSettingsPanel;
        public Selectable firstSelectedItemForBasicSettingsPanel;

        [Space]
        public GameObject advancedSettingsPanel;
        public Selectable firstSelectedItemForAdvancedSettingsPanel;

        [Space]
        public AudioClip selectionSound;
        public AudioClip toggleSound;
        public AudioClip slideSound;
        public AudioClip buttonSound;
        public AudioClip inputFieldFocusingSound;
        public AudioClip inputFieldCancelSound;
        public AudioClip inputfieldSubmitSound;

        [HideInInspector]
        public bool saveSharedSettingsJson = false;

        void Awake()
        {
            #region Create Singleton Instance
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning("SettingsUI component can only be one in a scene. Destroying duplicate.");
                #endif
                Destroy(gameObject);
            }
            #endregion

            rectTransform = GetComponent<RectTransform>();

            // Hide on start.
            Instance.gameObject.SetActive(false);
        }

        void OnDisable()
        {
            if (Instance.saveSharedSettingsJson)
            {
                ToryCare.Config.Instance.SaveToFile();
                Instance.saveSharedSettingsJson = false;
            }
        }

        void OnDestroy()
        {
            if (SettingsUI.instance == this)
            {
                SettingsUI.instance = null;
            }
        }

        /// <summary>
        /// Shows settings UI.
        /// </summary>
        /// <param name="advanced">If set to <c>true</c>, show advanced panel instead of basic one.</param>
        public static void Show(bool advanced = false)
        {
            Instance.gameObject.SetActive(true);
            if (advanced)
            {
                Instance.basicSettingsPanel.SetActive(false);
                Instance.advancedSettingsPanel.SetActive(true);
                if (Instance.firstSelectedItemForAdvancedSettingsPanel != null)
                {
                    Instance.StartCoroutine(Instance.SetSelectedGameObject(Instance.firstSelectedItemForAdvancedSettingsPanel));
                }
            }
            else
            {
                Instance.advancedSettingsPanel.SetActive(false);
                Instance.basicSettingsPanel.SetActive(true);
                if (Instance.firstSelectedItemForBasicSettingsPanel != null)
                {
                    Instance.StartCoroutine(Instance.SetSelectedGameObject(Instance.firstSelectedItemForBasicSettingsPanel));
                }
            }
            Instance.scrollbar.value = 1f;
        }

        IEnumerator SetSelectedGameObject(Selectable target)
        {
            if (EventSystem.current == null)
            {
                Debug.LogWarning("EventSystem doesn't exist in the scene. Add EventSystem to scene.");
            }

            EventSystem.current.SetSelectedGameObject(null);
            yield return new WaitForEndOfFrame();
            EventSystem.current.SetSelectedGameObject(target.gameObject);
        }

        /// <summary>
        /// Shows advanced settings panel.
        /// </summary>
        public static void ShowAdvancedPanel()
        {
            if (IsShown)
            {
                SettingsUI.Hide();
            }
            Show(true);
        }

        /// <summary>
        /// Hides settings UI.
        /// </summary>
        public static void Hide()
        {
            Instance.basicSettingsPanel.SetActive(false);
            Instance.advancedSettingsPanel.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);

            Instance.gameObject.SetActive(false);
        }

        public static void ScrollTo(RectTransform targetRectTransform)
        {
            if (Instance.smoothSnapScrollCoroutine != null)
            {
                Instance.StopCoroutine(Instance.smoothSnapScrollCoroutine);
            }

            var selectables = targetRectTransform.transform.parent.GetComponentsInChildren<Selectable>();
            for (int i = 0; i < selectables.Length; i++)
            {
                if (selectables[i].interactable)
                {
                    if (targetRectTransform.transform.parent.GetComponent<ScrollRect>() != null)
                    {
                        if (selectables[i].transform.Equals(targetRectTransform.transform))
                        {
                            Instance.smoothSnapScrollCoroutine = Instance.StartCoroutine(Instance.SmoothSnapScrollCoroutine(1f));
                            break;
                        }
                        else if (selectables[selectables.Length - 1].transform.Equals(targetRectTransform.transform))
                        {
                            Instance.smoothSnapScrollCoroutine = Instance.StartCoroutine(Instance.SmoothSnapScrollCoroutine(0f));
                            break;
                        }
                    }

                    float yOnScreen = Instance.rectTransform.rect.height * 0.5f - Instance.transform.InverseTransformPoint(targetRectTransform.transform.position).y;
                    if (yOnScreen < targetRectTransform.rect.height)
                    {
                        Instance.smoothSnapScrollCoroutine = Instance.StartCoroutine(Instance.SmoothSnapScrollCoroutine(Instance.scrollbar.value - (yOnScreen - targetRectTransform.rect.height) / Instance.rectTransform.rect.height));
                    }
                    else if (yOnScreen >= Instance.rectTransform.rect.height - targetRectTransform.rect.height)
                    {
                        Instance.smoothSnapScrollCoroutine = Instance.StartCoroutine(Instance.SmoothSnapScrollCoroutine(Instance.scrollbar.value - (yOnScreen - (Instance.rectTransform.rect.height - targetRectTransform.rect.height)) / Instance.rectTransform.rect.height));
                    }
                    break;
                }
            }
        }

        Coroutine smoothSnapScrollCoroutine;
        IEnumerator SmoothSnapScrollCoroutine(float targetScrollValue)
        {
            for (int i = 0; i < 10; i++)
            {
                scrollbar.value += (targetScrollValue - scrollbar.value) * 0.3f;
                yield return new WaitForEndOfFrame();
            }
            scrollbar.value = targetScrollValue;

            smoothSnapScrollCoroutine = null;
        }
    }
}