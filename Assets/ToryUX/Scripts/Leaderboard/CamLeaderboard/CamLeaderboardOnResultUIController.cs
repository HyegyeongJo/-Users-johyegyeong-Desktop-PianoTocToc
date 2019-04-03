using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    public class CamLeaderboardOnResultUIController : MonoBehaviour
    {
        public UnityEngine.Timeline.TimelineAsset timeline;
        public UnityEngine.Timeline.TimelineAsset backupTimeline;

        [Space]
        public GameObject photoScene;
        public CamLeaderboardWebcamTexture webcam;
        public CamLeaderboardCountThreeAnimationHandler countThree;
        [HideInInspector] public bool shouldUsePhoto = false;

        [Space]
        public GameObject leaderboardScene;
        public UISwitchableObjectController titleWrapper;

        [Space]
        public RectTransform firstRecordBar;
        public RectTransform secondRecordBar;
        public RectTransform thirdRecordBar;
        public RectTransform fourthRecordBar;
        public RectTransform belowRecordBar;
        public RawImage firstRecordPhoto;
        public RawImage secondRecordPhoto;
        public RawImage thirdRecordPhoto;
        public RawImage fourthRecordPhoto;
        public RawImage belowRecordPhoto;
        public Text firstRecordText;
        public Text secondRecordText;
        public Text thirdRecordText;
        public Text fourthRecordText;
        public Text belowRecordText;
        public ShowAndHideAnimationPlayer crown;
        public Texture2D defaultPortraitPhoto;

        [Space]
        public GameObject countDownUI;

        [Space]
        public GameObject rewardCardSceneObjectPrefab;
        public GameObject rewardCardScene;

        private HSVController myBarSolid;
        private HSVController myBarGradient;
        private float myBarSolidOriginalHue;
        private float myBarGradientOriginalHue;

        void OnEnable()
        {
            if (Time.frameCount > 0)
            {
                photoScene.SetActive(false);
                leaderboardScene.SetActive(false);
                rewardCardScene.SetActive(false);
                countDownUI.SetActive(false);

                if (sequenceCoroutine != null)
                {
                    StopCoroutine(sequenceCoroutine);
                    sequenceCoroutine = null;
                }
                sequenceCoroutine = StartCoroutine(SequenceCoroutine());
            }
        }

        void OnDisable()
        {
            StopAllCoroutines();

            // Restore score bar color.
            if (myBarSolid != null)
            {
                myBarSolid.hue = myBarSolidOriginalHue;
            }
            if (myBarGradient != null)
            {
                myBarGradient.hue = myBarGradientOriginalHue;
            }

            for (int i = 0; i < Mathf.Min(4, Leaderboard.EntriesToday.Count); i++)
            {
                GetRecordPhoto(i).texture = null;
                Resources.UnloadUnusedAssets();
                System.GC.Collect();
            }
        }

        Coroutine sequenceCoroutine;
        IEnumerator SequenceCoroutine()
        {
            yield return new WaitForEndOfFrame();

            // See if the record is high enough to take a picture.
            Leaderboard.Sort();
            // int todayRank = Leaderboard.GetTodayRank();
            int sevenDaysRank = Leaderboard.GetLastSevenDaysRank();
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (Leaderboard.RecordType == LeaderboardRecordType.Score)
            {
                Debug.LogFormat("Score: {0}", Score.CurrentScorePoint);
            }
            else
            {
                Debug.LogFormat("Record Time: {0}", Timer.CurrentTime);
            }
            // Debug.LogFormat("Today rank: {0}", todayRank);
            Debug.LogFormat("7-day rank: {0}", sevenDaysRank);
            #endif

            if (ToryCare.Config.ShowRewardCardSequence && sevenDaysRank <= ToryCare.Config.LimitRankForShowRewardCard)
            {
                if (Leaderboard.RecordType.Equals(LeaderboardRecordType.Score))
                {
                    if (Score.CurrentScorePoint >= ToryCare.Config.MinScoreForShowRewardCard)
                    {
                        Debug.Log("Show reward card sequence");
                        yield return StartCoroutine(RewardCardSequenceCoroutine());
                    }
                }
                else
                {
                    Debug.Log("Show reward card sequence");
                    yield return StartCoroutine(RewardCardSequenceCoroutine());
                }
            }

            /* 
            // UPDATE 201809. Remove take photo step.

            Texture2D snap = new Texture2D(1, 1);
            if (shouldTakePhoto)
            {
                UISound.Play(CamLeaderboardOnResultUI.Instance.goodJobSfx);
                countThree.gameObject.SetActive(false);
                photoScene.SetActive(true);

                bool countFinished = false;
                countThree.OnCountFinish += () =>
                {
                    countFinished = true;
                };
                while (!countFinished)
                {
                    yield return new WaitForEndOfFrame();
                }

                CameraEffects.CurtainImage.color = Color.white;
                CameraEffects.FadeIn(2f);

                if (WebcamTextureManager.Instance.Texture != null)
                {
                    RenderTexture currentRT = RenderTexture.active;

                    RenderTexture renderTexture = new RenderTexture(WebcamTextureManager.Instance.Texture.width, WebcamTextureManager.Instance.Texture.height, 32);
                    Graphics.Blit(WebcamTextureManager.Instance.Texture, renderTexture);
                    RenderTexture.active = renderTexture;

                    snap = new Texture2D(WebcamTextureManager.Instance.Texture.width, WebcamTextureManager.Instance.Texture.height);
                    // snap.SetPixels(WebcamTextureManager.Instance.Texture.GetPixels());
                    snap.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                    snap.Apply();

                    RenderTexture.active = currentRT;

                    byte[] snapData = snap.EncodeToPNG();
                    Leaderboard.AddEntry(new LeaderboardEntry(snapData));
                    if (TorywardManager.Instance != null)
                    {
                        TorywardManager.Instance.UploadEntry(snapData, todayRank, Leaderboard.GetRecordString(todayRank, Leaderboard.EntriesToday));
                    }
                }

                yield return new WaitForEndOfFrame();
                UISound.Play(CamLeaderboardOnResultUI.Instance.cameraSfx);
                countThree.gameObject.SetActive(false);
            }
            else
            {
                Leaderboard.AddEntry(new LeaderboardEntry());
            }
            */

            // Save leaderbaord and snap image and write leaderboard and upload to server if needed.
            if (TorywardManager.Instance != null)
            {
                if (TorywardManager.Instance.Snap != null)
                {
                    byte[] snapData = TorywardManager.Instance.Snap.EncodeToPNG();
                    Leaderboard.AddEntry(new LeaderboardEntry(snapData));
                }
                else
                {
                    Leaderboard.AddEntry(new LeaderboardEntry());
                }

                // Create small snap image and update current entry information to server.
                if (ToryCare.Config.DoUploadTorywardEntry &&
                    sevenDaysRank <= ToryCare.Config.TorywardUploadEntryLimitRank)
                {
                    if (TorywardManager.Instance.ClippedSnap != null)
                    {
                        byte[] clippedSnapData = TorywardManager.Instance.ClippedSnap.EncodeToPNG();
                        TorywardManager.Instance.UploadEntry(clippedSnapData, sevenDaysRank, Leaderboard.GetRecordString(sevenDaysRank, Leaderboard.EntriesToday));
                    }
                    else
                    {
                        TorywardManager.Instance.UploadEntry(sevenDaysRank, Leaderboard.GetRecordString(sevenDaysRank, Leaderboard.EntriesToday));
                    }
                }
            }
            else
            {
                Leaderboard.AddEntry(new LeaderboardEntry());
            }
            Leaderboard.WriteToFile();

            // Ready to show today rank.
            float barWidth = firstRecordBar.sizeDelta.x;
            for (int i = 0; i < 5; i++)
            {
                GetRecordBar(i).anchoredPosition = new Vector2(-barWidth, GetRecordBar(i).anchoredPosition.y);
            }
            photoScene.SetActive(false);
            leaderboardScene.SetActive(true);
            titleWrapper.Reset();

            // Load photo settings.
            CamAdjustment.Instance.LoadSettings();
            Vector3 mirrorScale = CamAdjustment.Instance.IsMirrored.Value ? new Vector3(-1f, 1f, 1f) : Vector3.one;
            Quaternion rotationAmount = Quaternion.Euler(0f, 0f, CamAdjustment.Instance.RotateAxis.Value * 90f);
            float zoomLevel = CamAdjustment.Instance.ZoomLevel.Value;
            Vector2 imageOffset = new Vector2(CamAdjustment.Instance.OffsetX.Value, CamAdjustment.Instance.OffsetY.Value);

            // Set photos and texts.
            Vector2 maskSizeDelta = GetRecordPhoto(0).transform.parent.GetComponent<RectTransform>().sizeDelta;
            RectTransform photoRectTransform;
            for (int i = 0; i < Mathf.Min(4, Leaderboard.EntriesToday.Count); i++)
            {
                GetRecordText(i).text = Leaderboard.GetFormattedRecordString(i + 1, Leaderboard.EntriesToday);

                photoRectTransform = GetRecordPhoto(i).GetComponent<RectTransform>();
                if (Leaderboard.EntriesToday[i].PortraitPhoto == null)
                {
                    GetRecordPhoto(i).texture = defaultPortraitPhoto;
                    photoRectTransform.localScale = Vector3.one;
                    photoRectTransform.sizeDelta = maskSizeDelta;
                    photoRectTransform.anchoredPosition = Vector2.zero;
                }
                else
                {
                    GetRecordPhoto(i).texture = Leaderboard.EntriesToday[i].PortraitPhoto;
                    photoRectTransform.localScale = mirrorScale;
                    photoRectTransform.localRotation = rotationAmount;
                    photoRectTransform.sizeDelta = new Vector2(
                        Leaderboard.EntriesToday[i].PortraitPhoto.width * maskSizeDelta.y / Leaderboard.EntriesToday[i].PortraitPhoto.height * zoomLevel,
                        maskSizeDelta.y * zoomLevel
                    );
                    photoRectTransform.anchoredPosition = Vector2.Scale(imageOffset, maskSizeDelta);
                }
            }

            /*
            // UPDATE 20180927. Remove today rank step.

            belowRecordText.text = Leaderboard.GetFormattedRecordString(todayRank);

            photoRectTransform = belowRecordPhoto.GetComponent<RectTransform>();
            if (shouldUsePhoto && TorywardManager.Instance.Snap != null && todayRank > 4)
            {
                belowRecordPhoto.texture = TorywardManager.Instance.Snap;
                photoRectTransform.localScale = mirrorScale;
                photoRectTransform.localRotation = rotationAmount;
                photoRectTransform.sizeDelta = new Vector2(
                    TorywardManager.Instance.Snap.width * maskSizeDelta.y / TorywardManager.Instance.Snap.height * zoomLevel,
                    maskSizeDelta.y * zoomLevel
                );
                photoRectTransform.anchoredPosition = Vector2.Scale(imageOffset, maskSizeDelta);
            }
            else
            {
                belowRecordPhoto.texture = defaultPortraitPhoto;
                photoRectTransform.localScale = Vector3.one;
                photoRectTransform.sizeDelta = maskSizeDelta;
                photoRectTransform.anchoredPosition = Vector2.zero;
            }

            firstRecordBar.gameObject.SetActive(true);
            if (Leaderboard.EntriesToday.Count > 1)
            {
                secondRecordBar.gameObject.SetActive(true);
                if (Leaderboard.EntriesToday.Count > 2)
                {
                    thirdRecordBar.gameObject.SetActive(true);
                    if (Leaderboard.EntriesToday.Count > 3)
                    {
                        if (todayRank <= 4)
                        {
                            fourthRecordBar.gameObject.SetActive(true);
                            belowRecordBar.gameObject.SetActive(false);
                        }
                        else
                        {
                            fourthRecordBar.gameObject.SetActive(false);
                            belowRecordBar.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        fourthRecordBar.gameObject.SetActive(false);
                        belowRecordBar.gameObject.SetActive(false);
                    }
                }
                else
                {
                    thirdRecordBar.gameObject.SetActive(false);
                    fourthRecordBar.gameObject.SetActive(false);
                    belowRecordBar.gameObject.SetActive(false);
                }
            }
            else
            {
                secondRecordBar.gameObject.SetActive(false);
                thirdRecordBar.gameObject.SetActive(false);
                fourthRecordBar.gameObject.SetActive(false);
                belowRecordBar.gameObject.SetActive(false);
            }
            crown.gameObject.SetActive(false);

            // Animate today rank record bars.
            yield return new WaitForSeconds(0.2f);
            titleWrapper.Show(0);
            myBarSolid = GetRecordBar(todayRank - 1).Find("Solid").GetComponent<HSVController>();
            myBarGradient = GetRecordBar(todayRank - 1).Find("Gradient").GetComponent<HSVController>();
            myBarSolidOriginalHue = myBarSolid.hue;
            myBarGradientOriginalHue = myBarGradient.hue;
            float startTime = Time.timeSinceLevelLoad;
            float endTime = startTime + 7f;
                
            while (Time.timeSinceLevelLoad < endTime)
            {
                float time = Time.timeSinceLevelLoad - startTime;
                if (!countDownUI.activeSelf && time >= 3f)
                {
                    countDownUI.SetActive(true);
                }
                if (time >= 2f)
                {
                    crown.gameObject.SetActive(true);
                }

                // At least, there is one record to show.
                float x1 = Mathf.Clamp01((time - 1f) * 3f);
                firstRecordBar.anchoredPosition = new Vector2(-barWidth + barWidth * Mathf.Lerp(0, 1f, x1), firstRecordBar.anchoredPosition.y);

                if (Leaderboard.EntriesToday.Count > 1)
                {
                    float x2 = Mathf.Clamp01((time - 1.25f) * 3f);
                    secondRecordBar.anchoredPosition = new Vector2(GetRelativeRecordBarAnchoredPositionX(Leaderboard.EntriesToday, 2, x2, barWidth), secondRecordBar.anchoredPosition.y);

                    if (Leaderboard.EntriesToday.Count > 2)
                    {
                        float x3 = Mathf.Clamp01((time - 1.5f) * 3f);
                        thirdRecordBar.anchoredPosition = new Vector2(GetRelativeRecordBarAnchoredPositionX(Leaderboard.EntriesToday, 3, x3, barWidth), thirdRecordBar.anchoredPosition.y);

                        if (Leaderboard.EntriesToday.Count > 3)
                        {
                            if (todayRank <= 4)
                            {
                                float x4 = Mathf.Clamp01((time - 1.75f) * 3f);
                                fourthRecordBar.anchoredPosition = new Vector2(GetRelativeRecordBarAnchoredPositionX(Leaderboard.EntriesToday, 4, x4, barWidth), fourthRecordBar.anchoredPosition.y);
                            }
                            else
                            {
                                float xBelow = Mathf.Clamp01((time - 2f) * 2f);
                                belowRecordBar.anchoredPosition = new Vector2(GetRelativeRecordBarAnchoredPositionX(Leaderboard.EntriesToday, Leaderboard.GetTodayRank(), xBelow, barWidth), belowRecordBar.anchoredPosition.y);
                            }
                        }
                    }
                }

                myBarSolid.hue = Mathf.Sin(Time.timeSinceLevelLoad * 2f) * 0.5f + 0.5f;
                myBarGradient.hue = Mathf.Sin(Time.timeSinceLevelLoad * 2f) * 0.5f + 0.5f;
                yield return new WaitForEndOfFrame();
            }

            // Animate out today rank record bars and title.
            titleWrapper.HideAll();
            if (crown.isActiveAndEnabled)
            {
                crown.PlayHideAnimation();
            }
            startTime = Time.timeSinceLevelLoad;
            endTime = startTime + 1f;
            while (Time.timeSinceLevelLoad < endTime)
            {
                float time = Time.timeSinceLevelLoad - startTime;

                float[] w = new float[Mathf.Min(5, Leaderboard.EntriesToday.Count)];
                for (int i = 0; i < w.Length; i++)
                {
                    w[i] = Mathf.Lerp(GetRecordBar(i).anchoredPosition.x, -barWidth, Mathf.Pow(time, 4f));
                    GetRecordBar(i).anchoredPosition = new Vector2(w[i], GetRecordBar(i).anchoredPosition.y);
                }
                yield return new WaitForEndOfFrame();
            }
            */

            // Ready to show weekly rank.
            CameraEffects.CurtainImage.color = Color.black;
            // CameraEffects.FadeOut(0.25f);
            // yield return new WaitForSeconds(0.25f);
            // CameraEffects.FadeIn(0.5f);

            for (int i = 0; i < 5; i++)
            {
                GetRecordBar(i).anchoredPosition = new Vector2(-barWidth, GetRecordBar(i).anchoredPosition.y);
            }
            crown.gameObject.SetActive(false);

            /*
            // UPDATE 20180927. Remove today rank step.

            // Restore bar colors.
            myBarSolid.hue = myBarSolidOriginalHue;
            myBarGradient.hue = myBarGradientOriginalHue;
            */

            // Set photos and texts.
            for (int i = 0; i < Mathf.Min(4, Leaderboard.EntriesLastSevenDays.Count); i++)
            {
                GetRecordText(i).text = Leaderboard.GetFormattedRecordString(i + 1, Leaderboard.EntriesLastSevenDays);

                RectTransform rectTransform = GetRecordPhoto(i).GetComponent<RectTransform>();
                if (Leaderboard.EntriesLastSevenDays[i].PortraitPhoto == null)
                {
                    GetRecordPhoto(i).texture = defaultPortraitPhoto;
                    rectTransform.localScale = Vector3.one;
                    rectTransform.sizeDelta = maskSizeDelta;
                    rectTransform.anchoredPosition = Vector2.zero;
                }
                else
                {
                    GetRecordPhoto(i).texture = Leaderboard.EntriesLastSevenDays[i].PortraitPhoto;
                    rectTransform.localScale = mirrorScale;
                    rectTransform.localRotation = rotationAmount;
                    rectTransform.sizeDelta = new Vector2(
                        Leaderboard.EntriesLastSevenDays[i].PortraitPhoto.width * maskSizeDelta.y / Leaderboard.EntriesLastSevenDays[i].PortraitPhoto.height * zoomLevel,
                        maskSizeDelta.y * zoomLevel
                    );
                    rectTransform.anchoredPosition = Vector2.Scale(imageOffset, maskSizeDelta);
                }
            }
            belowRecordText.text = Leaderboard.GetFormattedRecordString(sevenDaysRank);

            photoRectTransform = belowRecordPhoto.GetComponent<RectTransform>();
            if (shouldUsePhoto && TorywardManager.Instance.Snap != null)
            {
                belowRecordPhoto.texture = TorywardManager.Instance.Snap;
                photoRectTransform.localScale = mirrorScale;
                photoRectTransform.localRotation = rotationAmount;
                photoRectTransform.sizeDelta = new Vector2(
                    TorywardManager.Instance.Snap.width * maskSizeDelta.y / TorywardManager.Instance.Snap.height * zoomLevel,
                    maskSizeDelta.y * zoomLevel
                );
                photoRectTransform.anchoredPosition = Vector2.Scale(imageOffset, maskSizeDelta);
            }
            else
            {
                belowRecordPhoto.texture = defaultPortraitPhoto;
                photoRectTransform.localScale = Vector3.one;
                photoRectTransform.sizeDelta = maskSizeDelta;
                photoRectTransform.anchoredPosition = Vector2.zero;
            }

            firstRecordBar.gameObject.SetActive(true);
            if (Leaderboard.EntriesLastSevenDays.Count > 1)
            {
                secondRecordBar.gameObject.SetActive(true);
                if (Leaderboard.EntriesLastSevenDays.Count > 2)
                {
                    thirdRecordBar.gameObject.SetActive(true);
                    if (Leaderboard.EntriesLastSevenDays.Count > 3)
                    {
                        if (sevenDaysRank <= 4)
                        {
                            fourthRecordBar.gameObject.SetActive(true);
                            belowRecordBar.gameObject.SetActive(false);
                        }
                        else
                        {
                            fourthRecordBar.gameObject.SetActive(false);
                            belowRecordBar.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        fourthRecordBar.gameObject.SetActive(false);
                        belowRecordBar.gameObject.SetActive(false);
                    }
                }
                else
                {
                    thirdRecordBar.gameObject.SetActive(false);
                    fourthRecordBar.gameObject.SetActive(false);
                    belowRecordBar.gameObject.SetActive(false);
                }
            }
            else
            {
                secondRecordBar.gameObject.SetActive(false);
                thirdRecordBar.gameObject.SetActive(false);
                fourthRecordBar.gameObject.SetActive(false);
                belowRecordBar.gameObject.SetActive(false);
            }
            crown.gameObject.SetActive(false);

            // Animate weekly rank record bars and title.
            yield return new WaitForSeconds(0.2f);
            titleWrapper.Show(1);
            myBarSolid = GetRecordBar(sevenDaysRank - 1).Find("Solid").GetComponent<HSVController>();
            myBarGradient = GetRecordBar(sevenDaysRank - 1).Find("Gradient").GetComponent<HSVController>();
            myBarSolidOriginalHue = myBarSolid.hue;
            myBarGradientOriginalHue = myBarGradient.hue;

            // UPDATE 20180927. Remove today rank step.
            // startTime = Time.timeSinceLevelLoad;
            // endTime = startTime + 6f;
            float startTime = Time.timeSinceLevelLoad;
            float endTime = startTime + 15f;

            while (Time.timeSinceLevelLoad < endTime)
            {
                float time = Time.timeSinceLevelLoad - startTime;

                // UPDATE 20180927. Remove today rank step.
                if (!countDownUI.activeSelf && time >= 3f)
                {
                    countDownUI.SetActive(true);
                }

                if (time >= 2f)
                {
                    crown.gameObject.SetActive(true);
                }

                float x1 = Mathf.Clamp01((time - 1f) * 3f);
                firstRecordBar.anchoredPosition = new Vector2(-barWidth + barWidth * Mathf.Lerp(0, 1f, x1), firstRecordBar.anchoredPosition.y);

                if (Leaderboard.EntriesLastSevenDays.Count > 1)
                {
                    float x2 = Mathf.Clamp01((time - 1.25f) * 3f);
                    secondRecordBar.anchoredPosition = new Vector2(GetRelativeRecordBarAnchoredPositionX(Leaderboard.EntriesLastSevenDays, 2, x2, barWidth), secondRecordBar.anchoredPosition.y);

                    if (Leaderboard.EntriesLastSevenDays.Count > 2)
                    {
                        float x3 = Mathf.Clamp01((time - 1.5f) * 3f);
                        thirdRecordBar.anchoredPosition = new Vector2(GetRelativeRecordBarAnchoredPositionX(Leaderboard.EntriesLastSevenDays, 3, x3, barWidth), thirdRecordBar.anchoredPosition.y);

                        if (Leaderboard.EntriesLastSevenDays.Count > 3)
                        {
                            if (sevenDaysRank <= 4)
                            {
                                float x4 = Mathf.Clamp01((time - 1.75f) * 3f);
                                fourthRecordBar.anchoredPosition = new Vector2(GetRelativeRecordBarAnchoredPositionX(Leaderboard.EntriesLastSevenDays, 4, x4, barWidth), fourthRecordBar.anchoredPosition.y);
                            }
                            else
                            {
                                float xBelow = Mathf.Clamp01((time - 2f) * 2f);
                                belowRecordBar.anchoredPosition = new Vector2(GetRelativeRecordBarAnchoredPositionX(Leaderboard.EntriesLastSevenDays, Leaderboard.GetLastSevenDaysRank(), xBelow, barWidth), belowRecordBar.anchoredPosition.y);
                            }
                        }
                    }
                }

                myBarSolid.hue = Mathf.Sin(Time.timeSinceLevelLoad * 2f) * 0.5f + 0.5f;
                myBarGradient.hue = Mathf.Sin(Time.timeSinceLevelLoad * 2f) * 0.5f + 0.5f;
                yield return new WaitForEndOfFrame();
            }

            sequenceCoroutine = null;
        }

        IEnumerator RewardCardSequenceCoroutine()
        {
            CameraEffects.CurtainImage.color = Color.white;
            CameraEffects.FadeOut(0.1f);

            yield return new WaitForSeconds(0.2f);
            CameraEffects.FadeIn(0.5f);

            rewardCardScene.SetActive(true);
            GameObject rewardCardSceneObject = GameObject.Instantiate(rewardCardSceneObjectPrefab);

            yield return new WaitForSeconds(ToryCare.Config.RewardCardShowDuration);
            rewardCardScene.GetComponent<ShowAndHideAnimationPlayer>().PlayHideAnimation();

            // Destory instantiated prefab
            GameObject.Destroy(rewardCardSceneObject, 2f);
        }

        RectTransform GetRecordBar(int index)
        {
            switch (index)
            {
                case 0:
                    return firstRecordBar;
                case 1:
                    return secondRecordBar;
                case 2:
                    return thirdRecordBar;
                case 3:
                    return fourthRecordBar;
                default:
                    return belowRecordBar;
            }
        }

        RawImage GetRecordPhoto(int index)
        {
            switch (index)
            {
                case 0:
                    return firstRecordPhoto;
                case 1:
                    return secondRecordPhoto;
                case 2:
                    return thirdRecordPhoto;
                case 3:
                    return fourthRecordPhoto;
                default:
                    return belowRecordPhoto;
            }
        }

        Text GetRecordText(int index)
        {
            switch (index)
            {
                case 0:
                    return firstRecordText;
                case 1:
                    return secondRecordText;
                case 2:
                    return thirdRecordText;
                case 3:
                    return fourthRecordText;
                default:
                    return belowRecordText;
            }
        }

        float GetRelativeRecordBarAnchoredPositionX(System.Collections.ObjectModel.ReadOnlyCollection<LeaderboardEntry> rangedEntries, int ranking, float appearTiming, float barWidth)
        {
            float relativeSize = 0f;

            if (rangedEntries.Count >= ranking && ranking >= 1)
            {
                switch (Leaderboard.RecordType)
                {
                    case LeaderboardRecordType.Score:
                        relativeSize = (float) rangedEntries[ranking - 1].score / (float) rangedEntries[0].score;
                        break;
                    case LeaderboardRecordType.Stopwatch:
                        relativeSize = (float) rangedEntries[0].timeRecord / (float) rangedEntries[ranking - 1].timeRecord;
                        break;
                    case LeaderboardRecordType.Countdown:
                        relativeSize = (float) rangedEntries[ranking - 1].timeRecord / (float) rangedEntries[0].timeRecord;
                        break;
                }
            }

            return -barWidth + barWidth * Mathf.Lerp(0, relativeSize * 0.7f + 0.3f, appearTiming);
        }
    }
}