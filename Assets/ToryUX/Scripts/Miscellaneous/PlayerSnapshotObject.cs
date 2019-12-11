using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    public class PlayerSnapshotObject : MonoBehaviour
    {
        public RawImage photoImage;

        RectTransform photoRectTransform;
        public RectTransform parentRectTransform;

        RenderTexture renderTexture;

        System.Action OnResizedToFit = delegate
        {};

        void Awake()
        {
            photoRectTransform = photoImage.GetComponent<RectTransform>();
            renderTexture = GetComponentInChildren<Camera>().targetTexture;

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

            OnResizedToFit += () =>
            {
                StopCoroutine(CreateClippedSnapTexture());
                StartCoroutine(CreateClippedSnapTexture());
            };
        }

        void Start()
        {
            if (Time.frameCount > 0)
            {
                // photoImage.texture = WebcamTextureManager.Instance.Texture;
                photoImage.texture = TorywardManager.Instance.Snap;
                WaitAndResizeToFit();
            }
        }

        IEnumerator CreateClippedSnapTexture()
        {
            yield return new WaitForEndOfFrame();

            if (TorywardManager.Instance.ClippedSnap == null ||
                TorywardManager.Instance.ClippedSnap.width != renderTexture.width ||
                TorywardManager.Instance.ClippedSnap.height != renderTexture.height)
            {
                TorywardManager.Instance.ClippedSnap = null;
                Resources.UnloadUnusedAssets();
                System.GC.Collect();

                TorywardManager.Instance.ClippedSnap = new Texture2D(renderTexture.width, renderTexture.height);
            }

            yield return new WaitForEndOfFrame();

            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = renderTexture;

            TorywardManager.Instance.ClippedSnap.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            TorywardManager.Instance.ClippedSnap.Apply();

            RenderTexture.active = currentRT;

            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
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
            yield return new WaitUntil(() =>(TorywardManager.Instance.Snap != null || Time.timeSinceLevelLoad > waitTime + 2f));

            if (TorywardManager.Instance.Snap != null)
            {
                int prevWidth = TorywardManager.Instance.Snap.width;
                int prevHeight = TorywardManager.Instance.Snap.height;

                ResizeToFit();

                waitTime = Time.timeSinceLevelLoad;
                yield return new WaitUntil(() => prevWidth != TorywardManager.Instance.Snap.width ||
                    prevHeight != TorywardManager.Instance.Snap.height ||
                    Time.timeSinceLevelLoad > waitTime + 1f);

                // size changed after a while. Resize again.
                if (Time.timeSinceLevelLoad <= waitTime + 1f)
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

        public void ResizeToFit()
        {
            CamAdjustment.Instance.LoadSettings();
            MirrorImage(CamAdjustment.Instance.IsMirrored.Value);
            RotateImage(CamAdjustment.Instance.RotateAxis.Value);
            ZoomImage(CamAdjustment.Instance.ZoomLevel.Value);
            OffsetImage(CamAdjustment.Instance.OffsetX.Value, CamAdjustment.Instance.OffsetY.Value);

            OnResizedToFit();
        }

        public void MirrorImage(bool mirror)
        {
            if (photoRectTransform != null)
            {
                photoRectTransform.localScale = mirror ? new Vector3(-1f, 1f, 1f) : Vector3.one;
            }
        }

        public void RotateImage(int rotationIndex)
        {
            if (photoRectTransform != null)
            {
                photoRectTransform.localRotation = Quaternion.Euler(0f, 0f, 90f * rotationIndex);
            }
        }

        public void ZoomImage(float zoomLevel)
        {
            if (TorywardManager.Instance.Snap == null)
            {
                return;
            }

            if (photoRectTransform != null && TorywardManager.Instance.Snap != null)
            {
                photoRectTransform.sizeDelta = new Vector2(
                    TorywardManager.Instance.Snap.width * parentRectTransform.sizeDelta.y / TorywardManager.Instance.Snap.height * zoomLevel,
                    parentRectTransform.sizeDelta.y * zoomLevel
                );
            }
        }

        public void OffsetImage(float offsetX, float offsetY)
        {
            if (photoRectTransform != null)
            {
                photoRectTransform.anchoredPosition = new Vector2(
                    offsetX * parentRectTransform.sizeDelta.x,
                    offsetY * parentRectTransform.sizeDelta.y
                );
            }
        }

        public void OffsetXImage(float offsetX)
        {
            if (photoRectTransform != null)
            {
                photoRectTransform.anchoredPosition = new Vector2(
                    offsetX * parentRectTransform.sizeDelta.x,
                    photoRectTransform.anchoredPosition.y
                );
            }
        }

        public void OffsetYImage(float offsetY)
        {
            if (photoRectTransform != null)
            {
                photoRectTransform.anchoredPosition = new Vector2(
                    photoRectTransform.anchoredPosition.x,
                    offsetY * parentRectTransform.sizeDelta.y
                );
            }
        }
    }
}