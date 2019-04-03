using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    [ExecuteInEditMode, RequireComponent(typeof(RectTransform))]
    public class ToryTextBox : MonoBehaviour
    {
        RectTransform rectTransform;
        RectTransform childRectTransform;
        ContentSizeFitter childContentSizeFitter;
        public Text uiText;

        public string Content
        {
            get
            {
                return uiText.text;
            }
            set
            {
                uiText.text = value;
                ResizeToFit();
            }
        }

        void Awake()
        {
            FetchRectTransforms();
        }

        void OnEnable()
        {
            StartCoroutine(DelayedResizeCoroutine());
        }

        IEnumerator DelayedResizeCoroutine()
        {
            yield return new WaitForEndOfFrame();
            ResizeToFit();
        }

        void FetchRectTransforms()
        {
            rectTransform = GetComponent<RectTransform>();

            try
            {
                childRectTransform = transform.GetChild(0).GetComponent<RectTransform>();
                uiText = childRectTransform.GetChild(0).GetComponent<Text>();
            }
            catch
            {
                Debug.LogErrorFormat("ToryTextBox {0} does not have needed child structure.", name);
            }

            try
            {
                childContentSizeFitter = transform.GetChild(0).GetComponent<ContentSizeFitter>();
            }
            catch
            {
                Debug.LogErrorFormat("ToryTextBox {0} does not have needed child structure.", name);
            }
        }

        public void ResizeToFit()
        {
            if (rectTransform == null || childRectTransform == null)
            {
                FetchRectTransforms();
            }
            childContentSizeFitter.SetLayoutVertical();
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, childRectTransform.rect.size.y);
        }
    }
}