using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace ToryUX
{
    public class CamLeaderboardOnTitleUIController : MonoBehaviour
    {
        public float recordBarWidth = 1320f;
        public int numberOfFatBars = 1;

        [Space]
        public GameObject todayTitle;
        public GameObject weeklyTitle;
        public RectTransform firstRecordBar;
        public RectTransform secondRecordBar;
        public RectTransform thirdRecordBar;
        public RectTransform fourthRecordBar;
        public RectTransform fifthRecordBar;
        public RawImage firstRecordPhoto;
        public RawImage secondRecordPhoto;
        public RawImage thirdRecordPhoto;
        public RawImage fourthRecordPhoto;
        public RawImage fifthRecordPhoto;
        public Text firstRecordText;
        public Text secondRecordText;
        public Text thirdRecordText;
        public Text fourthRecordText;
        public Text fifthRecordText;
        public Texture2D defaultPortraitPhoto;

        public void Show(System.Action callback, System.Action rejectionCallback)
        {
            if (Leaderboard.EntriesLastSevenDays.Count <= 0)
            {
                rejectionCallback.Invoke();
                return;
            }

            if (sequenceCoroutine != null)
            {
                StopCoroutine(sequenceCoroutine);
            }
            sequenceCoroutine = StartCoroutine(SequenceCoroutine(callback));
        }

        void OnDisable()
        {
            if (sequenceCoroutine != null)
            {
                StopCoroutine(sequenceCoroutine);
            }
            sequenceCoroutine = null;
            
            gameObject.SetActive(false);
        }

        Coroutine sequenceCoroutine;
        IEnumerator SequenceCoroutine(System.Action callback)
        {
            float startTime;
            float endTime;
            weeklyTitle.SetActive(false);
            todayTitle.SetActive(false);

            // Load photo settings.
            CamAdjustment.Instance.LoadSettings();
            var mirrorScale = CamAdjustment.Instance.IsMirrored.Value ? new Vector3(-1f, 1f, 1f) : Vector3.one;
            var rotationAmount = Quaternion.Euler(0f, 0f, 90f * CamAdjustment.Instance.RotateAxis.Value);
            var zoomLevel = CamAdjustment.Instance.ZoomLevel.Value;
            var imageOffset = new Vector2(CamAdjustment.Instance.OffsetX.Value, CamAdjustment.Instance.OffsetY.Value);

            /*
            if (Leaderboard.EntriesToday.Count > 0)
            {
                // Ready to show today rank.
                for (int i = 0; i < 5; i++)
                {
                    GetRecordBar(i).sizeDelta = new Vector2(0, GetRecordBar(i).sizeDelta.y);
                    GetRecordBar(i).gameObject.SetActive(false);
                }

                var todaySolids = GetComponentsInChildren<TagClasses.CamLeaderboardUITodaySolidColorObject>(true);
                for (int i = 0; i < todaySolids.Length; i++)
                {
                    todaySolids[i].GetComponent<Graphic>().color = CamLeaderboardOnTitleUI.Instance.todaySolidColor;
                }
                var todayGradients = GetComponentsInChildren<TagClasses.CamLeaderboardUITodayGradientColorObject>(true);
                for (int i = 0; i < todayGradients.Length; i++)
                {
                    todayGradients[i].GetComponent<Graphic>().color = CamLeaderboardOnTitleUI.Instance.todayGradientColor;
                }
                var todayTexts = GetComponentsInChildren<TagClasses.CamLeaderboardUITodayTextColorObject>(true);
                for (int i = 0; i < todayTexts.Length; i++)
                {
                    todayTexts[i].GetComponent<Graphic>().color = CamLeaderboardOnTitleUI.Instance.todayTextColor;
                }

                // Set photos and texts.
                for (int i = 0; i < Mathf.Min(5, Leaderboard.EntriesToday.Count); i++)
                {
                    var maskSizeDelta = GetRecordPhoto(i).transform.parent.GetComponent<RectTransform>().sizeDelta;
                    var rectTransform = GetRecordPhoto(i).GetComponent<RectTransform>();
                    if (Leaderboard.EntriesToday[i].PortraitPhoto == null)
                    {
                        GetRecordPhoto(i).texture = defaultPortraitPhoto;
                        rectTransform.localScale = Vector3.one;
                        rectTransform.sizeDelta = maskSizeDelta;
                        rectTransform.anchoredPosition = Vector2.zero;
                    }
                    else
                    {
                        GetRecordPhoto(i).texture = Leaderboard.EntriesToday[i].PortraitPhoto;
                        rectTransform.localScale = mirrorScale;
                        rectTransform.localRotation = rotationAmount;
                        rectTransform.sizeDelta = new Vector2(
                            Leaderboard.EntriesToday[i].PortraitPhoto.width * maskSizeDelta.y / Leaderboard.EntriesToday[i].PortraitPhoto.height * zoomLevel,
                            maskSizeDelta.y * zoomLevel
                        );
                        rectTransform.anchoredPosition = Vector2.Scale(imageOffset, maskSizeDelta);
                    }

                    // GetRecordText(i).text = string.Format(i < numberOfFatBars ? ScoreTextFormatFat : ScoreTextFormatSlim, (i + 1).ToString(), Leaderboard.EntriesToday[i].score.ToString());
                    GetRecordText(i).text = Leaderboard.GetFormattedRecordString(i + 1, Leaderboard.EntriesToday, i >= numberOfFatBars);
                }

                // Animate in today rank record bars.
                todayTitle.SetActive(true);
                yield return new WaitForSeconds(0.2f);
                startTime = Time.timeSinceLevelLoad;
                endTime = startTime + 7f;
                while (Time.timeSinceLevelLoad < endTime)
                {
                    float time = Time.timeSinceLevelLoad - startTime;

                    var w = new float[Mathf.Min(5, Leaderboard.EntriesToday.Count)];
                    for (int i = 0; i < w.Length; i++)
                    {
                        // w[i] = Mathf.Lerp(0, Mathf.Max(CamLeaderboardOnTitleUI.Instance.recordBarMinWidth, recordBarWidth * ((float) Leaderboard.EntriesToday[i].score / (float) Leaderboard.EntriesToday[0].score * 0.75f + 0.25f)), Mathf.Pow(Mathf.Clamp01(time - 1f - i * 0.25f), 0.5f));
                        w[i] = Mathf.Lerp(0, Mathf.Max(CamLeaderboardOnTitleUI.Instance.recordBarMinWidth, GetRelativeRecordBarWidth(Leaderboard.EntriesToday, i + 1, recordBarWidth)), Mathf.Pow(Mathf.Clamp01(time - 1f - i * 0.25f), 0.5f));

                        GetRecordBar(i).sizeDelta = new Vector2(w[i], GetRecordBar(i).sizeDelta.y);
                        if (!GetRecordBar(i).gameObject.activeInHierarchy && w[i] > 0)
                        {
                            GetRecordBar(i).gameObject.SetActive(true);
                        }
                    }

                    yield return new WaitForEndOfFrame();
                }

                // Animate out today rank record bars.
                todayTitle.GetComponent<ShowAndHideAnimationPlayer>().PlayHideAnimation();
                for (int i = 0; i < Mathf.Min(5, Leaderboard.EntriesToday.Count); i++)
                {
                    GetRecordBar(i).GetComponent<ShowAndHideAnimationPlayer>().PlayHideAnimation();
                }
                startTime = Time.timeSinceLevelLoad;
                endTime = startTime + 1f;
                while (Time.timeSinceLevelLoad < endTime)
                {
                    float time = Time.timeSinceLevelLoad - startTime;

                    var w = new float[Mathf.Min(5, Leaderboard.EntriesToday.Count)];
                    for (int i = 0; i < w.Length; i++)
                    {
                        // w[i] = Mathf.Lerp(Mathf.Max(CamLeaderboardOnTitleUI.Instance.recordBarMinWidth, recordBarWidth * ((float) Leaderboard.EntriesToday[i].score / (float) Leaderboard.EntriesToday[0].score * 0.75f + 0.25f)), 0, Mathf.Pow(time, 4f));
                        w[i] = Mathf.Lerp(Mathf.Max(CamLeaderboardOnTitleUI.Instance.recordBarMinWidth, GetRelativeRecordBarWidth(Leaderboard.EntriesToday, i + 1, recordBarWidth)), 0, Mathf.Pow(time, 4f));

                        GetRecordBar(i).sizeDelta = new Vector2(w[i], GetRecordBar(i).sizeDelta.y);
                    }

                    yield return new WaitForEndOfFrame();
                }
                for (int i = 0; i < 5; i++)
                {
                    GetRecordBar(i).sizeDelta = new Vector2(0, GetRecordBar(i).sizeDelta.y);
                }
            }
            */

            // Ready to show weekly rank.
            for (int i = 0; i < 5; i++)
            {
                GetRecordBar(i).sizeDelta = new Vector2(0, GetRecordBar(i).sizeDelta.y);
                GetRecordBar(i).gameObject.SetActive(false);
            }

            var weeklySolids = GetComponentsInChildren<TagClasses.CamLeaderboardUITodaySolidColorObject>(true);
            for (int i = 0; i < weeklySolids.Length; i++)
            {
                weeklySolids[i].GetComponent<Graphic>().color = CamLeaderboardOnTitleUI.Instance.weeklySolidColor;
            }
            var weeklyGradients = GetComponentsInChildren<TagClasses.CamLeaderboardUITodayGradientColorObject>(true);
            for (int i = 0; i < weeklyGradients.Length; i++)
            {
                weeklyGradients[i].GetComponent<Graphic>().color = CamLeaderboardOnTitleUI.Instance.weeklyGradientColor;
            }
            var weeklyTexts = GetComponentsInChildren<TagClasses.CamLeaderboardUITodayTextColorObject>(true);
            for (int i = 0; i < weeklyTexts.Length; i++)
            {
                weeklyTexts[i].GetComponent<Graphic>().color = CamLeaderboardOnTitleUI.Instance.weeklyTextColor;
            }

            // Set photos and texts.
            for (int i = 0; i < Mathf.Min(5, Leaderboard.EntriesLastSevenDays.Count); i++)
            {
                var maskSizeDelta = GetRecordPhoto(i).transform.parent.GetComponent<RectTransform>().sizeDelta;
                var rectTransform = GetRecordPhoto(i).GetComponent<RectTransform>();
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

                // GetRecordText(i).text = string.Format(i < numberOfFatBars ? ScoreTextFormatFat : ScoreTextFormatSlim, (i + 1).ToString(), Leaderboard.EntriesLastSevenDays[i].score.ToString());
                GetRecordText(i).text = Leaderboard.GetFormattedRecordString(i + 1, Leaderboard.EntriesLastSevenDays, i >= numberOfFatBars);
            }

            // Animate in weekly rank score bars.
            weeklyTitle.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            startTime = Time.timeSinceLevelLoad;
            endTime = startTime + 7f;
            while (Time.timeSinceLevelLoad < endTime)
            {
                float time = Time.timeSinceLevelLoad - startTime;

                var w = new float[Mathf.Min(5, Leaderboard.EntriesLastSevenDays.Count)];
                for (int i = 0; i < w.Length; i++)
                {
                    // w[i] = Mathf.Lerp(0, Mathf.Max(CamLeaderboardOnTitleUI.Instance.recordBarMinWidth, recordBarWidth * ((float) Leaderboard.EntriesLastSevenDays[i].score / (float) Leaderboard.EntriesLastSevenDays[0].score * 0.75f + 0.25f)), Mathf.Pow(Mathf.Clamp01(time - 1f - i * 0.25f), 0.5f));
                    w[i] = Mathf.Lerp(0, Mathf.Max(CamLeaderboardOnTitleUI.Instance.recordBarMinWidth, GetRelativeRecordBarWidth(Leaderboard.EntriesLastSevenDays, i + 1, recordBarWidth)), Mathf.Pow(Mathf.Clamp01(time - 1f - i * 0.25f), 0.5f));

                    GetRecordBar(i).sizeDelta = new Vector2(w[i], GetRecordBar(i).sizeDelta.y);
                    if (!GetRecordBar(i).gameObject.activeInHierarchy && w[i] > 0)
                    {
                        GetRecordBar(i).gameObject.SetActive(true);
                    }
                }

                yield return new WaitForEndOfFrame();
            }

            // Animate out weekly rank score bars.
            weeklyTitle.GetComponent<ShowAndHideAnimationPlayer>().PlayHideAnimation();
            for (int i = 0; i < Mathf.Min(5, Leaderboard.EntriesLastSevenDays.Count); i++)
            {
                GetRecordBar(i).GetComponent<ShowAndHideAnimationPlayer>().PlayHideAnimation();
            }
            startTime = Time.timeSinceLevelLoad;
            endTime = startTime + 1f;
            while (Time.timeSinceLevelLoad < endTime)
            {
                float time = Time.timeSinceLevelLoad - startTime;

                var w = new float[Mathf.Min(5, Leaderboard.EntriesLastSevenDays.Count)];
                for (int i = 0; i < w.Length; i++)
                {
                    // w[i] = Mathf.Lerp(Mathf.Max(CamLeaderboardOnTitleUI.Instance.recordBarMinWidth, recordBarWidth * ((float) Leaderboard.EntriesLastSevenDays[i].score / (float) Leaderboard.EntriesLastSevenDays[0].score * 0.75f + 0.25f)), 0, Mathf.Pow(time, 4f));
                    w[i] = Mathf.Lerp(Mathf.Max(CamLeaderboardOnTitleUI.Instance.recordBarMinWidth, GetRelativeRecordBarWidth(Leaderboard.EntriesLastSevenDays, i + 1, recordBarWidth)), 0, Mathf.Pow(time, 4f));

                    GetRecordBar(i).sizeDelta = new Vector2(w[i], GetRecordBar(i).sizeDelta.y);
                }

                yield return new WaitForEndOfFrame();
            }
            for (int i = 0; i < 5; i++)
            {
                GetRecordBar(i).sizeDelta = new Vector2(0, GetRecordBar(i).sizeDelta.y);
            }

            for (int i = 0; i < Mathf.Min(5, Leaderboard.EntriesLastSevenDays.Count); i++)
            {
                GetRecordPhoto(i).texture = null;
                Resources.UnloadUnusedAssets();
                System.GC.Collect();
            }

            // Callback.
            callback.Invoke();
            sequenceCoroutine = null;
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
                case 4:
                    return fifthRecordBar;
                default:
                    return null;
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
                case 4:
                    return fifthRecordPhoto;
                default:
                    return null;
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
                case 4:
                    return fifthRecordText;
                default:
                    return null;
            }
        }

        float GetRelativeRecordBarWidth(System.Collections.ObjectModel.ReadOnlyCollection<LeaderboardEntry> rangedEntries, int ranking, float barWidth)
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

            return barWidth * (relativeSize * 0.75f + 0.25f);
        }
    }
}