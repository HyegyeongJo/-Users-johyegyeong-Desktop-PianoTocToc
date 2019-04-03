using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    public class WebcamTextureManager : MonoBehaviour
    {
        #region Singleton
        static volatile WebcamTextureManager instance;
        static readonly object syncRoot = new object();

        public static WebcamTextureManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock(syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = FindObjectOfType<WebcamTextureManager>();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        private WebCamTexture webcamTexture;

        private Texture texture;
        public Texture Texture
        {
            get
            {
                #if TORY_EYE
                if (UseToryEyeCameraTexture)
                {
                    return ToryEye.Instance.WebcamTexture;
                }
                #endif

                return webcamTexture;
            }
        }

        public bool IsReady
        {
            get
            {
                #if TORY_EYE
                if (UseToryEyeCameraTexture)
                {
                    return Texture != null && Texture.width > 32 && ToryEye.Instance.WebcamTextureAvailable;
                }
                #endif

                return Texture != null && Texture.width > 32;
            }
        }

        int WebcamIndex
        {
            get
            {
                #if TORY_EYE
                if (UseToryEyeCameraTexture)
                {
                    return ToryEye.Instance.Settings.CameraIndex;
                }
                #endif

                if (PlayerPrefsElite.key != null)
                {
                    CamAdjustment.Instance.CameraIndex.LoadSavedValue();
                }
                return Mathf.Min(CamAdjustment.Instance.CameraIndex.Value, WebCamTexture.devices.Length - 1);
            }
            set
            {
                #if TORY_EYE
                if (UseToryEyeCameraTexture)
                {
                    ToryEye.Instance.Settings.CameraIndex = value;
                    return;
                }
                #endif

                CamAdjustment.Instance.CameraIndex.Value = value % WebCamTexture.devices.Length;
                if (PlayerPrefsElite.key != null)
                {
                    CamAdjustment.Instance.CameraIndex.Save();
                }
            }
        }

        public bool useToryEyeCameraTexture = true;
        public bool UseToryEyeCameraTexture
        {
            get
            {
                #if TORY_EYE
                {
                    #if UNITY_EDITOR
                    return ToryEye.Instance != null && useToryEyeCameraTexture;
                    #else
                    return ToryEye.Instance != null && ToryCare.Config.UseToryEyeCameraTexture;
                    #endif
                }
                #else
                return false;
                #endif
            }
        }

        [HideInInspector]
        public List<Graphic> referencingGraphics;

        void Awake()
        {
            #region Create Singleton Instance
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            #endregion

            referencingGraphics = new List<Graphic>();
        }
        
        public void StartCamera(Graphic graphic)
        {
            if (graphic != null && !referencingGraphics.Contains(graphic))
            {
                if (graphic.GetType().Equals(typeof(RawImage)) ||
                    graphic.GetType().Equals(typeof(Image)))
                {
                    referencingGraphics.Add(graphic);
                }
            }

            #if TORY_EYE
            if (UseToryEyeCameraTexture)
            {
                if (playCameraSafeOnToryEyeCoroutine != null)
                {
                    StopCoroutine(playCameraSafeOnToryEyeCoroutine);
                }
                playCameraSafeOnToryEyeCoroutine = StartCoroutine(PlayCameraSafeOnToryEye());
                return;
            }
            #endif

            if (webcamTexture == null)
            {
                if (WebcamIndex < 0)
                {
                    Debug.LogWarning("Camera device not found. Stop playing webcam.");
                    return;
                }

                if (PlayerPrefs.HasKey("WebcamFailed"))
                {
                    WebcamIndex++;
                }

                webcamTexture = new WebCamTexture();
                webcamTexture.deviceName = WebCamTexture.devices[WebcamIndex].name;
            }

            PlayCamera();
        }

        void PlayCamera()
        {
            PlayerPrefs.SetInt("WebcamFailed", 1);
            PlayerPrefs.Save();

            webcamTexture.Play();

            PlayerPrefs.DeleteKey("WebcamFailed");
            PlayerPrefs.Save();

            ApplyReferencingTexture();
        }

        #if TORY_EYE
        Coroutine playCameraSafeOnToryEyeCoroutine;
        IEnumerator PlayCameraSafeOnToryEye()
        {
            // Make sure if Syphon received texture is ready.
            float waitTime = Time.timeSinceLevelLoad;
            yield return new WaitUntil(() => ToryEye.Instance.WebcamTextureAvailable || Time.timeSinceLevelLoad > waitTime + 3f);

            if (ToryEye.Instance.WebcamTextureAvailable)
            {
                ApplyReferencingTexture();

                if (ToryEye.Instance.ChangeCameraContextWhenNeed)
                {
                    ToryEye.Instance.SetWebcamTextureContext();
                    yield return new WaitUntil(() => ToryEye.Instance.webcamSizeModified);

                    yield return new WaitForSeconds(.1f);
                    ApplyReferencingTexture();
                }
            }
            else
            {
                Debug.LogWarning("WebcamTexture is NULL. Try to check ToryEye application is running.");
            }

            playCameraSafeOnToryEyeCoroutine = null;
        }
        #endif

        void ApplyReferencingTexture()
        {
            foreach (Graphic graphic in referencingGraphics)
            {
                if (graphic.GetType().Equals(typeof(RawImage)))
                {
                    ((RawImage) graphic).texture = Texture;
                    ((RawImage) graphic).material.mainTexture = Texture;
                }
                else if (graphic.GetType().Equals(typeof(Image)))
                {
                    ((Image) graphic).material.mainTexture = Texture;
                }
            }
        }

        public void StopCamera(Graphic graphic, bool keepTexture = false)
        {
            if (referencingGraphics.Contains(graphic))
            {
                referencingGraphics.Remove(graphic);

                if (graphic.GetType().Equals(typeof(RawImage)))
                {
                    if (!keepTexture)
                    {
                        ((RawImage) graphic).texture = null;
                        ((RawImage) graphic).material.mainTexture = null;
                    }
                }
                else if (graphic.GetType().Equals(typeof(Image)))
                {
                    if (!keepTexture)
                    {
                        ((Image) graphic).material.mainTexture = null;
                    }
                }
            }

            if (referencingGraphics.Count < 1)
            {
                if (Texture != null)
                {
                    #if TORY_EYE
                    if (UseToryEyeCameraTexture)
                    {
                        if (ToryEye.Instance.ChangeCameraContextWhenNeed)
                        {
                            ToryEye.Instance.SetSensingContext();
                        }
                        return;
                    }
                    #endif

                    if (!ToryCare.Config.PlayBackgroundWebcam)
                    {
                        webcamTexture.Stop();
                        webcamTexture = null;
                    }
                }
            }
        }

        public void SelectCamera(int webcamIndex)
        {
            #if TORY_EYE
            if (UseToryEyeCameraTexture)
            {
                return;
            }
            #endif

            if (WebcamIndex != webcamIndex)
            {
                WebcamIndex = webcamIndex;
                Debug.Log("Switch webcam to " + WebCamTexture.devices[WebcamIndex].name);

                if (webcamTexture.isPlaying)
                {
                    webcamTexture.Stop();
                }
                webcamTexture.deviceName = WebCamTexture.devices[WebcamIndex].name;
                PlayCamera();
            }
        }

        void OnDestroy()
        {
            #if TORY_EYE
            if (UseToryEyeCameraTexture)
            {
                return;
            }
            #endif

            if (webcamTexture != null)
            {
                webcamTexture.Stop();
                webcamTexture = null;
            }
        }
    }
}