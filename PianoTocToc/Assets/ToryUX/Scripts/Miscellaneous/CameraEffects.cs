using System.Collections;
using System.Collections.Generic;
using LeTai.Asset.TranslucentImage;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

namespace ToryUX
{
    public class CameraEffects : MonoBehaviour
    {
        public static CameraEffects Instance
        {
            get
            {
                return instance;
            }
        }
        private static CameraEffects instance;

        private GameObject toryUXBase;

        /// <summary>
        /// An <c>UI.Image</c> used for visual fade effects.
        /// Better not to modify anything; just use <c>CameraEffects.FadeIn()</c>, <c>CameraEffects.FadeOut()</c> or <c>CameraEffects.Fade</c> methods instead.
        /// Default color of the image is black.
        /// </summary>
        public static Image CurtainImage
        {
            get;
            private set;
        }

        /// <summary>
        /// A <c>CanvasRenderer</c> component of <c>CurtainImage</c>.
        /// Fade in/out effects are executed by changing alpha value of this object.
        /// No need to do anything with this for common uses.
        /// </summary>
        public static CanvasRenderer CurtainCanvasRenderer
        {
            get
            {
                if (curtainCanvasRenderer == null)
                {
                    curtainCanvasRenderer = CurtainImage.GetComponent<CanvasRenderer>();
                }
                return curtainCanvasRenderer;
            }
        }
        private static CanvasRenderer curtainCanvasRenderer;

        /// <summary>
        /// An alpha value of <c>CurtainImage</c>'s <c>CanvasRenderer</c>.
        /// Reading this value can be a clue for ongoing fading effects.
        /// Manually changing the value is possible but may cause unexpected behaviour if done along with default fading effects such as <c>FadeIn()</c> and <c>FadeOut()</c>.
        /// </summary>
        public static float CurtainAlpha
        {
            get
            {
                return CurtainCanvasRenderer.GetAlpha();
            }
            set
            {
                CurtainCanvasRenderer.SetAlpha(value);
            }
        }

        /// <summary>
        /// Occurs when the result screen times out and completely faded out.
        /// Good place to register game restart code.
        /// </summary>
        public static event FadedOutAction OnFadedOut;
        public delegate void FadedOutAction();

        /// <summary>
        /// A UI layer that blurs what's seen on main camera.
        /// </summary>
        /// <value>The translucent layer.</value>
        public static TranslucentImage TranslucentLayer
        {
            get;
            private set;
        }
        float translucentLayerVibrancy;
        float translucentLayerBrightness;
        float translucentLayerFlatten;

        public Camera mainCamera;
        private PostProcessingProfile postProcessingProfile;
        private PostProcessingBehaviour postProcessingBehaviour;

        public GameObject translucentLayerPrefab;

        public static float TranslucentLayerBlurStrength
        {
            get
            {
                return translucentLayerBlurStrength;
            }
            set
            {
                translucentLayerBlurStrength = value;
            }
        }
        private static float translucentLayerBlurStrength = 30f;
        private TranslucentImageSource translucentImageSource;

        [Space]
        public GameObject curtainLayerPrefab;

        /// <summary>
        /// Gets/sets intensity of bloom effect on <c>FlashBloom()</c> call.
        /// The value will be added to existing bloom intensity.
        /// Default value is 5f.
        /// </summary>
        public static float BloomFlashIntensity
        {
            get
            {
                return bloomFlashIntensity;
            }
            set
            {
                bloomFlashIntensity = value;
            }
        }
        private static float bloomFlashIntensity = 5f;

        /// <summary>
        /// Gets/sets bloom settings of currently effective post processing behaviour.
        /// </summary>
        public static BloomModel.Settings BloomSettings
        {
            get
            {
                return Instance.postProcessingBehaviour.profile.bloom.settings;
            }
            set
            {
                Instance.postProcessingBehaviour.profile.bloom.settings = value;
            }
        }

        private bool originalBloomEnableState;
        private float originalBloomIntensity = 0;
        private bool isBloomBeingModified = false;

        /// <summary>
        /// Gets/sets intensity of vignette effect on <c>ShowVignetteEffect()</c> call.
        /// The value will be added to existing vignette intensity.
        /// Default value is 0.45f.
        /// </summary>
        public static float VignetteIntensity
        {
            get
            {
                return vignetteIntensity;
            }
            set
            {
                vignetteIntensity = value;
            }
        }
        private static float vignetteIntensity = 0.45f;

        /// <summary>
        /// Gets/sets vignette settings of currently effective post processing behaviour.
        /// </summary>
        public static VignetteModel.Settings VignetteSettings
        {
            get
            {
                return Instance.postProcessingBehaviour.profile.vignette.settings;
            }
            set
            {
                Instance.postProcessingBehaviour.profile.vignette.settings = value;
            }
        }

        private bool originalVignetteEnableState;
        private float originalVignetteIntensity = 0;
        private bool isVignetteBeingModified = false;

        void Awake()
        {
            // Check singleton validity.
            if (CameraEffects.Instance != null && CameraEffects.Instance != this)
            {
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning("CameraEffects component can only be one in a scene. Destroying duplicate.");
                #endif
                Destroy(this.gameObject);
            }
            else
            {
                CameraEffects.instance = this;
            }

            // Get base UI layer GameObject;
            toryUXBase = GetComponentInChildren<UISound>().gameObject;

            // Deal with curtain image object.
            if (CurtainImage == null)
            {
                var curtainLayer = Instantiate(curtainLayerPrefab, toryUXBase.transform);
                curtainLayer.name = "CurtainLayer";
                CurtainImage = curtainLayer.GetComponent<Image>();
                CurtainImage.transform.SetAsLastSibling();
                CurtainImage.raycastTarget = false;
                StartCoroutine(CurtainCheckCoroutine());
            }

            // Deal with translucent layer.
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            translucentImageSource = mainCamera.gameObject.AddComponent<TranslucentImageSource>();
            translucentImageSource.Strength = translucentLayerBlurStrength;

            if (TranslucentLayer == null)
            {
                var translucentLayer = Instantiate(translucentLayerPrefab, toryUXBase.transform);
                translucentLayer.name = "TranslucentLayer";
                translucentLayer.transform.SetAsFirstSibling();
                TranslucentLayer = translucentLayer.GetComponent<TranslucentImage>();
                TranslucentLayer.source = translucentImageSource;

                translucentLayerVibrancy = TranslucentLayer.vibrancy;
                translucentLayerBrightness = TranslucentLayer.brightness;
                translucentLayerFlatten = TranslucentLayer.flatten;
				
                translucentLayer.SetActive(false);
            }

            // Add post processing behaviour on camera if it is not there.
            postProcessingBehaviour = mainCamera.GetComponent<PostProcessingBehaviour>();
            if (postProcessingBehaviour == null)
            {
                postProcessingBehaviour = mainCamera.gameObject.AddComponent<PostProcessingBehaviour>();
            }

            // Use given post processing profile or create one.
            postProcessingProfile = postProcessingBehaviour.profile;
            if (postProcessingProfile == null)
            {
                postProcessingProfile = ScriptableObject.CreateInstance<PostProcessingProfile>();
                postProcessingBehaviour.profile = postProcessingProfile;

                // If the profile is being created byisTranslucentLayerBlurBeingModified this script, set default vignette intensity as 0.
                VignetteModel.Settings defaultVignetteSettings = postProcessingProfile.vignette.settings;
                defaultVignetteSettings.intensity = 0;
                postProcessingProfile.vignette.settings = defaultVignetteSettings;
            }
        }

        IEnumerator CurtainCheckCoroutine()
        {
            yield return new WaitForEndOfFrame();
            if (TitleUI.Instance == null)
            {
                CurtainImage.CrossFadeAlpha(0, 0, true);
            }
        }

        /// <summary>
        /// Fade out effects.
        /// The curtain image will be absolutely clear at start, then fade to be opaque.
        /// Call <c>Fade()</c> instead to fade from current alpha to target alpha.
        /// </summary>
        public static void FadeOut(float duration = 1f, bool ignoreTimeScale = false)
        {
            CurtainImage.CrossFadeAlpha(0, 0, true);
            CurtainImage.gameObject.SetActive(true);
            CurtainImage.CrossFadeAlpha(1, duration, ignoreTimeScale);

            if (Instance.hideCurtainAfterRealTimeCoroutine != null)
            {
                Instance.StopCoroutine(Instance.hideCurtainAfterRealTimeCoroutine);
                Instance.hideCurtainAfterRealTimeCoroutine = null;
            }
            if (Instance.hideCurtainAfterGameTimeCoroutine != null)
            {
                Instance.StopCoroutine(Instance.hideCurtainAfterGameTimeCoroutine);
                Instance.hideCurtainAfterGameTimeCoroutine = null;
            }

            if (Instance.fadeOutCoroutine != null)
            {
                Instance.StopCoroutine(Instance.fadeOutCoroutine);
                Instance.fadeOutCoroutine = null;
            }
            Instance.fadeOutCoroutine = Instance.StartCoroutine(Instance.FadeOutCoroutine(duration));
        }

        Coroutine fadeOutCoroutine;
        IEnumerator FadeOutCoroutine(float duration)
        {
            yield return new WaitForSeconds(duration);

            if (OnFadedOut != null)
            {
                OnFadedOut();
            }

            fadeOutCoroutine = null;
        }

        /// <summary>
        /// Fade in effects.
        /// The curtain image will be absolutely opaque at start, then fade to be clear.
        /// Call <c>Fade()</c> instead to fade from current alpha to target alpha.
        /// </summary>
        public static void FadeIn(float duration = 1f, bool ignoreTimeScale = false)
        {
            CurtainImage.CrossFadeAlpha(1, 0, true);
            CurtainImage.gameObject.SetActive(true);
            CurtainImage.CrossFadeAlpha(0, duration, ignoreTimeScale);

            if (Instance.hideCurtainAfterRealTimeCoroutine != null)
            {
                Instance.StopCoroutine(Instance.hideCurtainAfterRealTimeCoroutine);
                Instance.hideCurtainAfterRealTimeCoroutine = null;
            }
            if (Instance.hideCurtainAfterGameTimeCoroutine != null)
            {
                Instance.StopCoroutine(Instance.hideCurtainAfterGameTimeCoroutine);
                Instance.hideCurtainAfterGameTimeCoroutine = null;
            }

            if (ignoreTimeScale)
            {
                Instance.hideCurtainAfterRealTimeCoroutine = Instance.StartCoroutine(Instance.HideCurtainAfterRealTimeCoroutine(duration));
            }
            else
            {
                Instance.hideCurtainAfterGameTimeCoroutine = Instance.StartCoroutine(Instance.HideCurtainAfterGameTimeCoroutine(duration));
            }
        }

        /// <summary>
        /// Fade effect to target alpha from current alpha.
        /// Useful to cancel out <c>FadeIn()</c> and <c>FadeOut()</c> effects.
        /// </summary>
        public static void Fade(float targetAlpha, float duration = 1f, bool ignoreTimeScale = false)
        {
            CurtainImage.CrossFadeAlpha(targetAlpha, duration, ignoreTimeScale);

            if (Instance.hideCurtainAfterRealTimeCoroutine != null)
            {
                Instance.StopCoroutine(Instance.hideCurtainAfterRealTimeCoroutine);
                Instance.hideCurtainAfterRealTimeCoroutine = null;
            }
            if (Instance.hideCurtainAfterGameTimeCoroutine != null)
            {
                Instance.StopCoroutine(Instance.hideCurtainAfterGameTimeCoroutine);
                Instance.hideCurtainAfterGameTimeCoroutine = null;
            }

            if (targetAlpha <= 0)
            {
                if (ignoreTimeScale)
                {
                    Instance.hideCurtainAfterRealTimeCoroutine = Instance.StartCoroutine(Instance.HideCurtainAfterRealTimeCoroutine(duration));
                }
                else
                {
                    Instance.hideCurtainAfterGameTimeCoroutine = Instance.StartCoroutine(Instance.HideCurtainAfterGameTimeCoroutine(duration));
                }
            }
        }

        Coroutine hideCurtainAfterGameTimeCoroutine;
        IEnumerator HideCurtainAfterGameTimeCoroutine(float time)
        {
            yield return new WaitForSeconds(time);
            CurtainImage.gameObject.SetActive(false);

            hideCurtainAfterGameTimeCoroutine = null;
        }

        Coroutine hideCurtainAfterRealTimeCoroutine;
        IEnumerator HideCurtainAfterRealTimeCoroutine(float time)
        {
            yield return new WaitForSecondsRealtime(time);
            CurtainImage.gameObject.SetActive(false);

            hideCurtainAfterRealTimeCoroutine = null;
        }

        /// <summary>
        /// Shows the translucent layer that blurs out what is shown to <c>mainCamera</c>.
        /// It does not affect what is shown to <c>uiCamera</c>; useful to blur out background but UI.
        /// </summary>
        /// <param name="duration">How long the blur effect will take to show up. Default value is 0.5f.</param>
        public static void ShowTranslucentLayer(float duration = 0.5f)
        {
            instance.StopCoroutine("ShowTranslucentLayerCoroutine");
            instance.StopCoroutine("HideTranslucentLayerCoroutine");

            if (Instance.showTranslucentLayerCoroutine != null)
            {
                Instance.StopCoroutine(Instance.showTranslucentLayerCoroutine);
                Instance.showTranslucentLayerCoroutine = null;
            }
            if (Instance.hideTranslucentLayerCoroutine != null)
            {
                Instance.StopCoroutine(Instance.hideTranslucentLayerCoroutine);
                Instance.hideTranslucentLayerCoroutine = null;
            }

            Instance.showTranslucentLayerCoroutine = Instance.StartCoroutine(Instance.ShowTranslucentLayerCoroutine(duration));
        }

        /// <summary>
        /// Shows the translucent layer that blurs out what is shown to <c>mainCamera</c>.
        /// It does not affect what is shown to <c>uiCamera</c>; useful to blur out background but UI.
        /// </summary>
        /// <param name="duration">How long the blur effect will take to show up. Default value is 0.5f.</param>
        public static void HideTranslucentLayer(float duration = 0.5f)
        {
            if (Instance.showTranslucentLayerCoroutine != null)
            {
                Instance.StopCoroutine(Instance.showTranslucentLayerCoroutine);
                Instance.showTranslucentLayerCoroutine = null;
            }
            if (Instance.hideTranslucentLayerCoroutine != null)
            {
                Instance.StopCoroutine(Instance.hideTranslucentLayerCoroutine);
                Instance.hideTranslucentLayerCoroutine = null;
            }

            Instance.hideTranslucentLayerCoroutine = Instance.StartCoroutine(Instance.HideTranslucentLayerCoroutine(duration));
        }

        Coroutine showTranslucentLayerCoroutine;
        IEnumerator ShowTranslucentLayerCoroutine(float duration)
        {
            TranslucentLayer.gameObject.SetActive(true);

            float startTime = Time.timeSinceLevelLoad;
            float endTime = startTime + duration;
            while (Time.timeSinceLevelLoad <= endTime)
            {
                translucentImageSource.Strength = Mathf.Lerp(translucentImageSource.Strength, CameraEffects.TranslucentLayerBlurStrength, (Time.timeSinceLevelLoad - startTime) / (endTime - startTime));

                TranslucentLayer.vibrancy = Mathf.Lerp(TranslucentLayer.vibrancy, translucentLayerVibrancy, (Time.timeSinceLevelLoad - startTime) / (endTime - startTime));
                TranslucentLayer.brightness = Mathf.Lerp(TranslucentLayer.brightness, translucentLayerBrightness, (Time.timeSinceLevelLoad - startTime) / (endTime - startTime));
                TranslucentLayer.flatten = Mathf.Lerp(TranslucentLayer.flatten, translucentLayerFlatten, (Time.timeSinceLevelLoad - startTime) / (endTime - startTime));

                yield return new WaitForEndOfFrame();
            }

            translucentImageSource.Strength = CameraEffects.TranslucentLayerBlurStrength;

            TranslucentLayer.vibrancy = translucentLayerVibrancy;
            TranslucentLayer.brightness = translucentLayerBrightness;
            TranslucentLayer.flatten = translucentLayerFlatten;

            showTranslucentLayerCoroutine = null;
        }

        Coroutine hideTranslucentLayerCoroutine;
        IEnumerator HideTranslucentLayerCoroutine(float duration)
        {
            float currentTranslucentLayerBlurStrength = translucentImageSource.Strength;
            float currentTranslucentLayerVibrancy = TranslucentLayer.vibrancy;
            float currentTranslucentLayerBrightness = TranslucentLayer.brightness;
            float currentTranslucentLayerFlatten = TranslucentLayer.flatten;

            float startTime = Time.timeSinceLevelLoad;
            float endTime = startTime + duration;
            while (Time.timeSinceLevelLoad <= endTime)
            {
                translucentImageSource.Strength = Mathf.Lerp(currentTranslucentLayerBlurStrength, 0f, (Time.timeSinceLevelLoad - startTime) / (endTime - startTime));

                TranslucentLayer.vibrancy = Mathf.Lerp(currentTranslucentLayerVibrancy, 1f, (Time.timeSinceLevelLoad - startTime) / (endTime - startTime));
                TranslucentLayer.brightness = Mathf.Lerp(currentTranslucentLayerBrightness, 0f, (Time.timeSinceLevelLoad - startTime) / (endTime - startTime));
                TranslucentLayer.flatten = Mathf.Lerp(currentTranslucentLayerFlatten, 0f, (Time.timeSinceLevelLoad - startTime) / (endTime - startTime));

                yield return new WaitForEndOfFrame();
            }

            translucentImageSource.Strength = 0f;

            TranslucentLayer.vibrancy = 1f;
            TranslucentLayer.brightness = 0f;
            TranslucentLayer.flatten = 0f;

            TranslucentLayer.gameObject.SetActive(false);

            hideTranslucentLayerCoroutine = null;
        }

        /// <summary>
        /// Shows burst of bloom effect. Useful for screen transitioning effect.
        /// It only changes intensity of the effect; other settings will be remained the same as it is or will default to initial values.
        /// Use <c>BloomSettings</c> property to have more control.
        /// </summary>
        /// <param name="duration">How long the flash effect will take. Default value is 1f.</param>
        public static void FlashBloom(float duration = 1f)
        {
            // Remeber bloom intensity to restore later on.
            if (!instance.isBloomBeingModified)
            {
                instance.originalBloomIntensity = instance.postProcessingProfile.bloom.settings.bloom.intensity;
                instance.originalBloomEnableState = instance.postProcessingProfile.bloom.enabled;
            }

            if (Instance.bloomFlashCoroutine != null)
            {
                Instance.StopCoroutine(Instance.bloomFlashCoroutine);
                Instance.bloomFlashCoroutine = null;
            }

            Instance.bloomFlashCoroutine = Instance.StartCoroutine(Instance.BloomFlashCoroutine(duration));
        }

        Coroutine bloomFlashCoroutine;
        IEnumerator BloomFlashCoroutine(float duration)
        {
            isBloomBeingModified = true;
            BloomModel.Settings bloomSettings = postProcessingProfile.bloom.settings;
            postProcessingProfile.bloom.enabled = true;

            float startTime = Time.timeSinceLevelLoad;
            float endTime = startTime + duration;
            while (Time.timeSinceLevelLoad <= endTime)
            {
                bloomSettings.bloom.intensity = Mathf.Lerp(originalBloomIntensity + CameraEffects.BloomFlashIntensity, originalBloomIntensity, (Time.timeSinceLevelLoad - startTime) / (endTime - startTime));
                postProcessingProfile.bloom.settings = bloomSettings;
                yield return new WaitForEndOfFrame();
            }

            bloomSettings.bloom.intensity = originalBloomIntensity;
            postProcessingProfile.bloom.settings = bloomSettings;
            postProcessingProfile.bloom.enabled = originalBloomEnableState;
            isBloomBeingModified = false;

            bloomFlashCoroutine = null;
        }

        /// <summary>
        /// Shows vignette effect.
        /// It only changes intensity of the effect; other settings will be remained the same as it is or will default to initial values.
        /// Use <c>VignetteSettings</c> property to have more control.
        /// </summary>
        /// <param name="duration">How long the vignette effect will take to show up. Default value is 0.5f.</param>
        public static void ShowVignetteEffect(float duration = 0.5f)
        {
            // Remeber bloom intensity to restore later on.
            if (!instance.isVignetteBeingModified)
            {
                instance.originalVignetteIntensity = instance.postProcessingProfile.vignette.settings.intensity;
                instance.originalVignetteEnableState = instance.postProcessingProfile.vignette.enabled;
            }

            if (Instance.vignetteEffectShowCoroutine != null)
            {
                Instance.StopCoroutine(Instance.vignetteEffectShowCoroutine);
                Instance.vignetteEffectShowCoroutine = null;
            }
            if (Instance.vignetteEffectHideCoroutine != null)
            {
                Instance.StopCoroutine(Instance.vignetteEffectHideCoroutine);
                Instance.vignetteEffectHideCoroutine = null;
            }

            Instance.vignetteEffectShowCoroutine = Instance.StartCoroutine(Instance.VignetteEffectShowCoroutine(duration));
        }

        /// <summary>
        /// Restores vignette effect as it was before called <c>ShowVignetteEffect()</c>.
        /// Do not call this if <c>ShowVignetteEffect()</c> was never called before.
        /// </summary>
        /// <param name="duration">How long the vignette effect will take to be restored. Default value is 0.5f.</param>
        public static void HideVignetteEffect(float duration = 0.5f)
        {
            if (Instance.vignetteEffectShowCoroutine != null)
            {
                Instance.StopCoroutine(Instance.vignetteEffectShowCoroutine);
                Instance.vignetteEffectShowCoroutine = null;
            }
            if (Instance.vignetteEffectHideCoroutine != null)
            {
                Instance.StopCoroutine(Instance.vignetteEffectHideCoroutine);
                Instance.vignetteEffectHideCoroutine = null;
            }

            Instance.vignetteEffectHideCoroutine = Instance.StartCoroutine(Instance.VignetteEffectHideCoroutine(duration));
        }

        Coroutine vignetteEffectShowCoroutine;
        IEnumerator VignetteEffectShowCoroutine(float duration)
        {
            isVignetteBeingModified = true;
            VignetteModel.Settings vignetteSettings = postProcessingProfile.vignette.settings;
            postProcessingProfile.vignette.enabled = true;

            float startTime = Time.timeSinceLevelLoad;
            float endTime = startTime + duration;
            while (Time.timeSinceLevelLoad <= endTime)
            {
                vignetteSettings.intensity = Mathf.Lerp(originalVignetteIntensity, CameraEffects.VignetteIntensity, (Time.timeSinceLevelLoad - startTime) / (endTime - startTime));
                postProcessingProfile.vignette.settings = vignetteSettings;
                yield return new WaitForEndOfFrame();
            }

            vignetteSettings.intensity = CameraEffects.vignetteIntensity;
            postProcessingProfile.vignette.settings = vignetteSettings;

            vignetteEffectShowCoroutine = null;
        }

        Coroutine vignetteEffectHideCoroutine;
        IEnumerator VignetteEffectHideCoroutine(float duration)
        {
            VignetteModel.Settings vignetteSettings = postProcessingProfile.vignette.settings;
            float startTime = Time.timeSinceLevelLoad;
            float endTime = startTime + duration;
            while (Time.timeSinceLevelLoad <= endTime)
            {
                vignetteSettings.intensity = Mathf.Lerp(CameraEffects.VignetteIntensity, originalVignetteIntensity, (Time.timeSinceLevelLoad - startTime) / (endTime - startTime));
                postProcessingProfile.vignette.settings = vignetteSettings;
                yield return new WaitForEndOfFrame();
            }

            vignetteSettings.intensity = originalVignetteIntensity;
            postProcessingProfile.vignette.settings = vignetteSettings;
            postProcessingProfile.vignette.enabled = originalVignetteEnableState;
            isVignetteBeingModified = false;

            vignetteEffectHideCoroutine = null;
        }

        void OnDisable()
        {
            try
            {
                // Revert bloom settings.
                var bloomSettings = postProcessingProfile.bloom.settings;
                bloomSettings.bloom.intensity = originalBloomIntensity;
                postProcessingProfile.bloom.settings = bloomSettings;
                postProcessingProfile.bloom.enabled = originalBloomEnableState;

                // Revert vignetting settings.
                var vignetteSettings = postProcessingProfile.vignette.settings;
                vignetteSettings.intensity = originalVignetteIntensity;
                postProcessingProfile.vignette.settings = vignetteSettings;
                postProcessingProfile.vignette.enabled = originalVignetteEnableState;
            }
            catch (System.NullReferenceException)
            {}
        }

        void OnDestroy()
        {
            if (CameraEffects.Instance == this)
            {
                CameraEffects.instance = null;
                OnFadedOut = null;
            }
        }
    }
}