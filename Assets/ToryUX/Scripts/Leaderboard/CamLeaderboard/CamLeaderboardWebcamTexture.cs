using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    [RequireComponent(typeof(RawImage))]
    public class CamLeaderboardWebcamTexture : MonoBehaviour
    {
        public RawImage RawImage
        {
            get;
            private set;
        }

        private RectTransform parentRectTransform;
        private RectTransform rectTransform;

        void Awake()
        {
            parentRectTransform = transform.parent.GetComponent<RectTransform>();
            rectTransform = GetComponent<RectTransform>();
            RawImage = GetComponent<RawImage>();

            #if TORY_EYE
            if (WebcamTextureManager.Instance.UseToryEyeCameraTexture && ToryEye.Instance.ChangeCameraContextWhenNeed)
            {
                ToryEye.Instance.onWebcamSizeChanged.AddListener(delegate()
                {
                    if (gameObject.activeInHierarchy)
                    {
                        WaitAndResizeToFit();
                    }
                });
            }
            #endif
        }

        void OnEnable()
        {
            if (Time.frameCount > 0)
            {
                if (waitAndStartCameraCoroutine == null)
                {
                    waitAndStartCameraCoroutine = StartCoroutine(WaitAndStartCameraCoroutine());
                }
                WaitAndResizeToFit();
            }
        }

        void OnDisable()
        {
            if (Time.frameCount > 0)
            {
                WebcamTextureManager.Instance.StopCamera(RawImage);
            }

            if (waitAndStartCameraCoroutine != null)
            {
                StopCoroutine(waitAndStartCameraCoroutine);
                waitAndStartCameraCoroutine = null;
            }

            if (waitAndResizeToFitCoroutine != null)
            {
                StopCoroutine(waitAndResizeToFitCoroutine);
                waitAndResizeToFitCoroutine = null;
            }
        }

        Coroutine waitAndStartCameraCoroutine;
        IEnumerator WaitAndStartCameraCoroutine()
        {
            yield return new WaitForEndOfFrame();
            WebcamTextureManager.Instance.StartCamera(RawImage);
        }

        void WaitAndResizeToFit()
        {
            if (waitAndResizeToFitCoroutine == null)
            {
                waitAndResizeToFitCoroutine = StartCoroutine(WaitAndResizeToFitCoroutine());
            }
        }

        Coroutine waitAndResizeToFitCoroutine;
        IEnumerator WaitAndResizeToFitCoroutine()
        {
            float waitTime = Time.timeSinceLevelLoad;
            yield return new WaitUntil(() =>(WebcamTextureManager.Instance != null && WebcamTextureManager.Instance.IsReady) || Time.timeSinceLevelLoad > waitTime + 2f);

            if ((WebcamTextureManager.Instance != null && WebcamTextureManager.Instance.IsReady))
            {
                int prevWidth = WebcamTextureManager.Instance.Texture.width;
                int prevHeight = WebcamTextureManager.Instance.Texture.height;

                ResizeToFit();

                waitTime = Time.timeSinceLevelLoad;
                yield return new WaitUntil(() => prevWidth != WebcamTextureManager.Instance.Texture.width ||
                    prevHeight != WebcamTextureManager.Instance.Texture.height ||
                    Time.timeSinceLevelLoad > waitTime + 2f);

                // size changed after a while. Resize again.
                if (Time.timeSinceLevelLoad < waitTime + 2f)
                {
                    ResizeToFit();
                }
            }
            else
            {
                ResizeToFit();
            }

            waitAndResizeToFitCoroutine = null;
        }

        // Should be called after some time until the camera texture resolution becomes final.
        public void ResizeToFit()
        {
            CamAdjustment.Instance.LoadSettings();
            MirrorImage(CamAdjustment.Instance.IsMirrored.Value);
            RotateImage(CamAdjustment.Instance.RotateAxis.Value);
            ZoomImage(CamAdjustment.Instance.ZoomLevel.Value);
            OffsetImage(CamAdjustment.Instance.OffsetX.Value, CamAdjustment.Instance.OffsetY.Value);
        }

        public void MirrorImage(bool mirror)
        {
            if (rectTransform != null)
            {
                rectTransform.localScale = mirror ? new Vector3(-1f, 1f, 1f) : Vector3.one;
            }
        }

        public void RotateImage(int rotationIndex)
        {
            if (rectTransform != null)
            {
                rectTransform.localRotation = Quaternion.Euler(0f, 0f, 90f * rotationIndex);
            }
        }

        public void ZoomImage(float zoomLevel)
        {
            if (WebcamTextureManager.Instance == null || !WebcamTextureManager.Instance.IsReady)
            {
                return;
            }

            if (rectTransform != null && WebcamTextureManager.Instance.Texture != null)
            {
                rectTransform.sizeDelta = new Vector2(
                    WebcamTextureManager.Instance.Texture.width * parentRectTransform.sizeDelta.y / WebcamTextureManager.Instance.Texture.height * zoomLevel,
                    parentRectTransform.sizeDelta.y * zoomLevel
                );
            }
        }

        public void OffsetImage(float offsetX, float offsetY)
        {
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector2(
                    offsetX * parentRectTransform.sizeDelta.x,
                    offsetY * parentRectTransform.sizeDelta.y
                );
            }
        }

        public void OffsetXImage(float offsetX)
        {
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector2(
                    offsetX * parentRectTransform.sizeDelta.x,
                    rectTransform.anchoredPosition.y
                );
            }
        }

        public void OffsetYImage(float offsetY)
        {
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector2(
                    rectTransform.anchoredPosition.x,
                    offsetY * parentRectTransform.sizeDelta.y
                );
            }
        }

        public void SelectCamera(int webcamIndex)
        {
            if (WebcamTextureManager.Instance == null || !WebcamTextureManager.Instance.IsReady)
            {
                return;
            }

            WebcamTextureManager.Instance.SelectCamera(webcamIndex);
        }
    }
}