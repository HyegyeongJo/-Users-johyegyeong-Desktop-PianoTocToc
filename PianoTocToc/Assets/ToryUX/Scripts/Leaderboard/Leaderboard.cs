using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ToryUX
{
    public enum LeaderboardRecordType
    {
        Score,
        Stopwatch,
        Countdown
    }

    public static class Leaderboard
    {
        /// <summary>
        /// Record type of leaderboard. Use one of the <c>Score</c>, <c>Stopwatch</c> or <c>Countdown</c>.
        /// </summary>
        /// <returns></returns>
        public static LeaderboardRecordType RecordType
        {
            get
            {
                return recordType;
            }
            set
            {
                recordType = value;
            }
        }
        private static LeaderboardRecordType recordType;

        private static string ScoreTextFormat = "<size=96>{0}등</size>\n<size=64>{1}점</size>";
        private static string TimeRecordTextFormat = "<size=96>{0}등</size>\n<size=64>{1}</size>";
        private static string TimeRecordTextFormatMilliseconds = "<size=96>{0}등</size>\n<size=64>{1}</size><size=48>.{2:D2}</size>";
        private static string ScoreTextFormatSmall = "<size=64>{0}등</size>\n<size=48>{1}점</size>";
        private static string TimeRecordTextFormatSmall = "<size=64>{0}등</size>\n<size=48>{1}</size>";
        private static string TimeRecordTextFormatSmallMilliseconds = "<size=64>{0}등</size>\n<size=48>{1}</size><size=36>.{2:D2}</size>";
        /// <summary>
        /// List of <c>LeaderboardEntry</c>.
        /// </summary>
        public static System.Collections.ObjectModel.ReadOnlyCollection<LeaderboardEntry> Entries
        {
            get
            {
                if (collection == null)
                {
                    ReadFromFile();
                }
                return collection.entries.AsReadOnly();
            }
        }

        /// <summary>
        /// Filtered list of <c>LeaderboardEntry</c>.
        /// The list should only contain entries of today.
        /// </summary>
        public static System.Collections.ObjectModel.ReadOnlyCollection<LeaderboardEntry> EntriesToday
        {
            get
            {
                if (collection == null)
                {
                    ReadFromFile();
                }
                return collection.QueryToday().AsReadOnly();
            }
        }

        /// <summary>
        /// Filtered list of <c>LeaderboardEntry</c>.
        /// The list should only contain entries of this week.
        /// </summary>
        /*
        public static System.Collections.ObjectModel.ReadOnlyCollection<LeaderboardEntry> EntriesThisWeek
        {
            get
            {
                if (collection == null)
                {
                    ReadFromFile();
                }
                return collection.QueryThisWeek().AsReadOnly();
            }
        }
        */

        /// <summary>
        /// Filtered list of <c>LeaderboardEntry</c>.
        /// The list should only contain entries of this month.
        /// </summary>
        public static System.Collections.ObjectModel.ReadOnlyCollection<LeaderboardEntry> EntriesThisMonth
        {
            get
            {
                if (collection == null)
                {
                    ReadFromFile();
                }
                return collection.QueryThisMonth().AsReadOnly();
            }
        }

        public static System.Collections.ObjectModel.ReadOnlyCollection<LeaderboardEntry> EntriesLastSevenDays
        {
            get
            {
                if (collection == null)
                {
                    ReadFromFile();
                }
                return collection.QueryLastNDays(7).AsReadOnly();
            }
        }

        public static string TorywardDirectory
        {
            get
            {
                string directory = ToryCare.Config.TorywardDirectory;

                // Check if toryward directory is exist.
                if (!System.IO.Directory.Exists(directory))
                {
                    Debug.Log(string.Format("Toryward data folder is not exist. Create folder {0}", directory));
                    System.IO.Directory.CreateDirectory(directory);
                }

                return directory;
            }
        }

        private static string torywardFilePath;
        public static string TorywardFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(torywardFilePath))
                {
                    torywardFilePath = Path.Combine(TorywardDirectory, ToryCare.Config.TorywardFilename);
                }
                return torywardFilePath;
            }
        }

        // Though player ranking is 9999, show this ranking for boys being ambitious.
        private static int veryLastRankingNumber = 50;

        private static LeaderboardEntryCollection collection;

        /// <summary>
        /// Sorts leaderboard entries by score and time.
        /// Sorting happens automatically in most situations (i.e., when reading from/writing to a file) which means that there's no need to call this method explicitly unless something's messed up.
        /// </summary>
        public static void Sort()
        {
            switch (RecordType)
            {
                case LeaderboardRecordType.Score:
                    collection.entries = collection.entries.OrderByDescending(entry => entry.score).ThenByDescending(entry => entry.UtcDateTime).ToList<LeaderboardEntry>();
                    break;
                case LeaderboardRecordType.Stopwatch:
                    collection.entries = collection.entries.OrderBy(entry => entry.timeRecord).ThenByDescending(entry => entry.UtcDateTime).ToList<LeaderboardEntry>();
                    break;
                case LeaderboardRecordType.Countdown:
                    collection.entries = collection.entries.OrderByDescending(entry => entry.timeRecord).ThenByDescending(entry => entry.UtcDateTime).ToList<LeaderboardEntry>();
                    break;
            }
        }

        /// <summary>
        /// Reads leaderboard file to generate <c>Entries</c> and such.
        /// </summary>
        /// <returns><c>true</c> on successful read, <c>false</c> otherwise.</returns>
        public static bool ReadFromFile()
        {
            if (!File.Exists(TorywardFilePath))
            {
                Debug.LogWarning("Leaderboard file couldn't be found.\nNew file will be created when needed.");
                collection = new LeaderboardEntryCollection();
            }
            else
            {
                try
                {
                    collection = JsonUtility.FromJson<LeaderboardEntryCollection>(File.ReadAllText(TorywardFilePath));
                    #if UNITY_EDITOR || DEVELOPMENT_BUILD
                    Debug.LogFormat("Leaderboard loaded from a file: {0}", TorywardFilePath);
                    #endif
                }
                catch (FileNotFoundException e)
                {
                    Debug.LogWarning(e);
                }
                catch
                {
                    return false;
                }
            }

            if (ToryCare.Config.DoMakeUpToryward)
            {
                MakeUpEntries();
            }
            return true;
        }

        /// <summary>
        /// Writes to leaderboard file.
        /// Current <c>Entries</c> content will always overwrite what is there in a file.
        /// </summary>
        /// <returns><c>true</c> on successful write, <c>false</c> otherwise.</returns>
        public static bool WriteToFile()
        {
            if (ToryCare.Config.DoMakeUpToryward)
            {
                MakeUpEntries();
                RemoveIsolatedPhotoFiles();
            }

            try
            {
                File.WriteAllText(TorywardFilePath, JsonUtility.ToJson(collection, true));
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogFormat("Leaderboard saved to a file: {0}", TorywardFilePath);
                #endif
            }
            catch (ArgumentException)
            {
                Debug.LogError("Leaderboard.WriteToFile(); failed because the path was invalid.");
                return false;
            }
            catch
            {
                return false;
            }
            return true;
        }

        private static int GetRank(System.Collections.ObjectModel.ReadOnlyCollection<LeaderboardEntry> rangedEntries)
        {
            if (RecordType == LeaderboardRecordType.Score)
            {
                return GetRank(rangedEntries, Score.CurrentScorePoint);
            }
            else
            {
                return GetRank(rangedEntries, Timer.CurrentTime);
            }
        }

        /// <summary>
        /// Gets all time rank of current record, depend on leaderboard's record type.
        /// Note that even rank overs 9999, max rank number string is displayed as <c>veryLastRankingNumber</c>.
        /// </summary>
        public static int GetAllTimeRank()
        {
            return GetRank(Entries);
        }

        /// <summary>
        /// Gets today's rank of current record, depend on leaderboard's record type.
        /// </summary>
        public static int GetTodayRank()
        {
            return GetRank(EntriesToday);
        }

        /// <summary>
        /// Gets this month's rank of current record, depend on leaderboard's record type.
        /// </summary>
        public static int GetMontlyRank()
        {
            return GetRank(EntriesThisMonth);
        }

        /// <summary>
        /// Gets last 7 day's rank of current record, depend on leaderboard's record type.
        /// </summary>
        public static int GetLastSevenDaysRank()
        {
            return GetRank(EntriesLastSevenDays);
        }

        /// <summary>
        /// Gets this week's rank of current record, depend on leaderboard's record type.
        /// </summary>
        // public static int GetWeeklyRank()
        // {
        //     return GetRank(EntriesThisWeek);
        // }

        private static int GetRank(System.Collections.ObjectModel.ReadOnlyCollection<LeaderboardEntry> rangedEntries, int score)
        {
            if (rangedEntries.Count == 0)
            {
                return 1;
            }

            for (int i = 0; i < rangedEntries.Count; i++)
            {
                if (score >= rangedEntries[i].score)
                {
                    return i + 1;
                }
            }

            return rangedEntries.Count + 1;
        }

        /// <summary>
        /// Gets all time rank of given score.
        /// </summary>
        public static int GetAllTimeRank(int score)
        {
            return GetRank(Entries, score);
        }

        /// <summary>
        /// Gets today's rank of given score.
        /// </summary>
        public static int GetTodayRank(int score)
        {
            return GetRank(EntriesToday, score);
        }

        /// <summary>
        /// Gets this month's rank of given score.
        /// </summary>
        public static int GetMontlyRank(int score)
        {
            return GetRank(EntriesThisMonth, score);
        }

        /// <summary>
        /// Gets last 7 day's rank of given score.
        /// </summary>
        public static int GetLastSevenDaysRank(int score)
        {
            return GetRank(EntriesLastSevenDays, score);
        }

        /// <summary>
        /// Gets this week's rank of given score.
        /// </summary>
        // public static int GetWeeklyRank(int score)
        // {
        //     return GetRank(EntriesThisWeek, score);
        // }

        private static int GetRank(System.Collections.ObjectModel.ReadOnlyCollection<LeaderboardEntry> rangedEntries, float timeRecord)
        {
            if (rangedEntries.Count == 0)
            {
                return 1;
            }

            if (RecordType == LeaderboardRecordType.Stopwatch)
            {
                for (int i = 0; i < rangedEntries.Count; i++)
                {
                    if (timeRecord <= rangedEntries[i].timeRecord)
                    {
                        return i + 1;
                    }
                }
            }
            else if (RecordType == LeaderboardRecordType.Countdown)
            {
                for (int i = 0; i < rangedEntries.Count; i++)
                {
                    if (timeRecord >= rangedEntries[i].timeRecord)
                    {
                        return i + 1;
                    }
                }
            }

            return rangedEntries.Count + 1;
        }

        /// <summary>
        /// Gets all time rank of given time record.
        /// </summary>
        public static int GetAllTimeRank(float timeRecord)
        {
            return GetRank(Entries, timeRecord);
        }

        /// <summary>
        /// Gets today's rank of given time record.
        /// </summary>
        public static int GetTodayRank(float timeRecord)
        {
            return GetRank(EntriesToday, timeRecord);
        }

        /// <summary>
        /// Gets this month's rank of given time record.
        /// </summary>
        public static int GetMontlyRank(float timeRecord)
        {
            return GetRank(EntriesThisMonth, timeRecord);
        }

        /// <summary>
        /// Gets last 7 day's rank of given time record.
        /// </summary>
        public static int GetLastSevenDaysRank(float timeRecord)
        {
            return GetRank(EntriesLastSevenDays, timeRecord);
        }

        /// <summary>
        /// Gets this week's rank of given time record.
        /// </summary>
        // public static int GetWeeklyRank(float timeRecord)
        // {
        //     return GetRank(EntriesThisWeek, timeRecord);
        // }

        public static void RemoveIsolatedPhotoFiles(int rankCut = -1)
        {
            string[] files = Directory.GetFiles(TorywardDirectory, ToryCare.Config.TorywardImageFormat.Replace("{0}", "*"));
            for (int i = 0; i < collection.entries.Count; i++)
            {
                for (int j = 0; j < files.Length; j++)
                {
                    if (collection.entries[i].UtcDateTime.ToBinary().ToString() == System.Text.RegularExpressions.Regex.Match(files[j], (ToryCare.Config.TorywardImageFormat + "$").Replace("{0}", "(\\d+)")).Groups[1].Value)
                    {
                        if (rankCut <= 0 || GetTodayRank(collection.entries[i].score) <= rankCut || GetLastSevenDaysRank(collection.entries[i].score) <= rankCut) // || GetMontlyRank(collection.entries[i].score) <= rankCut || GetWeeklyRank(collection.entries[i].score) <= rankCut))
                        {
                            files[j] = string.Empty;
                            collection.entries[i].NullifyPortraitPhoto();
                        }
                        continue;
                    }
                }
            }
            for (int i = 0; i < files.Length; i++)
            {
                if (!string.IsNullOrEmpty(files[i]))
                {
                    File.Delete(files[i]);
                }
            }
        }

        /// <summary>
        /// Resets the leaderboard compeletely.
        /// The change will clear local json file, remove all save local photo files and reset both high score and high record time.
        /// </summary>
        public static void Clear()
        {
            Debug.Log("Reset Leaderboard records and delete all local leaderboard photos.");
            collection = new LeaderboardEntryCollection();
            RemoveIsolatedPhotoFiles();
            WriteToFile();

            Score.ResetHighScore();
            Timer.ResetHighRecordTime();
        }

        /// <summary>
        /// Remove order entries, and sort leaderboard.
        /// The change will not be saved until <c>WriteToFile()</c> is explicitly called.
        /// </summary>
        public static bool MakeUpEntries()
        {
            int minRecordCount = ToryCare.Config.TorywardMinRecordCount;
            int recordLifeAsDay = ToryCare.Config.TorywardRecordLifeAsDay;

            Debug.LogFormat("Make up Toryward records. Left records in {0} days, but keep at least {1} records.", recordLifeAsDay, minRecordCount);

            int entryCount = collection.entries.Count;
            collection.entries = collection.QueryRecentNEntriesOrInLastMDays(minRecordCount, recordLifeAsDay);
            Debug.LogFormat("{0} Toryward records left after make up.", collection.entries.Count);

            Score.ResetHighScore();
            Timer.ResetHighRecordTime();

            // if entries are modified -> return true
            return entryCount != collection.entries.Count;
        }

        /*
        /// <summary>
        /// Trims the leaderboard to only have data of last seven days.
        /// The change will not be saved until <c>WriteToFile()</c> is explicitly called.
        /// </summary>
        public static void TrimToLastNDays(int n)
        {
            collection.entries = collection.QueryLastNDays(n);
            RemoveIsolatedPhotoFiles();
        }

        /// <summary>
        /// Trims the leaderboard to only have data of this month.
        /// The change will not be saved until <c>WriteToFile()</c> is explicitly called.
        /// </summary>
        public static void TrimToThisMonth()
        {
            collection.entries = collection.QueryThisMonth();
            RemoveIsolatedPhotoFiles();
        }

        /// <summary>
        /// Trims the leaderboard to only have data of this week.
        /// The change will not be saved until <c>WriteToFile()</c> is explicitly called.
        /// </summary>
        //        public static void TrimToThisWeek()
        //        {
        //            collection.entries = collection.QueryThisWeek();
        //            RemoveIsolatedPhotoFiles();
        //        }

        /// <summary>
        /// Trims the leaderboard to only have data of today.
        /// The change will not be saved until <c>WriteToFile()</c> is explicitly called.
        /// </summary>
        public static void TrimToToday()
        {
            collection.entries = collection.QueryToday();
            RemoveIsolatedPhotoFiles();
        }

        /// <summary>
        /// Trims the leaderboard to only have data of recent N entries.
        /// The change will not be saved until <c>WriteToFile()</c> is explicitly called.
        /// </summary>
        public static void TrimToRecentNEntries(int n)
        {
            collection.entries = collection.QueryRecentNEntries(n);
            RemoveIsolatedPhotoFiles();
        }
        */

        /// <summary>
        /// Adds new leaderboard entry after checking 
        /// The change will not be saved until <c>WriteToFile()</c> is explicitly called.
        /// </summary>
        public static void AddEntry(LeaderboardEntry entry)
        {
            AddEntry(entry, -1);
        }

        /// <summary>
        /// Adds new leaderboard entry after checking.
        /// The change will not be saved until <c>WriteToFile()</c> is explicitly called.
        /// <param name="rank">If given negative value, it will automatically check ranks to find out relevant index.</param>
        /// </summary>
        public static void AddEntry(LeaderboardEntry entry, int rank)
        {
            // Does not create entry if score or time remaining is zero.
            if ((RecordType == LeaderboardRecordType.Score && entry.score <= 0) ||
                (RecordType == LeaderboardRecordType.Stopwatch && entry.timeRecord <= 0) ||
                (RecordType == LeaderboardRecordType.Countdown && entry.timeRecord <= 0))
            {
                return;
            }

            if (rank <= 0)
            {
                if (RecordType == LeaderboardRecordType.Score)
                {
                    rank = GetAllTimeRank(entry.score);
                }
                else if (RecordType == LeaderboardRecordType.Stopwatch || RecordType == LeaderboardRecordType.Countdown)
                {
                    rank = GetAllTimeRank(entry.timeRecord);
                }
            }

            collection.entries.Insert(rank - 1, entry);
        }

        public static string GetFormattedRecordString(int ranking, System.Collections.ObjectModel.ReadOnlyCollection<LeaderboardEntry> rangedEntries, bool useSmallFont = false)
        {
            if (ranking > rangedEntries.Count || ranking < 1)
            {
                return "";
            }
            string rankingText = (ranking < veryLastRankingNumber) ? ranking.ToString() : veryLastRankingNumber.ToString();

            if (RecordType == LeaderboardRecordType.Score)
            {
                return string.Format((useSmallFont ? ScoreTextFormatSmall : ScoreTextFormat), rankingText, rangedEntries[ranking - 1].score);
            }
            else
            {
                string timeRecordString = TimerUI.SecondsToTimespanString(rangedEntries[ranking - 1].timeRecord, false);
                if (Timer.timerObject != null && Timer.timerObject.showMilliseconds)
                {
                    int milliseconds = TimeSpan.FromSeconds(rangedEntries[ranking - 1].timeRecord).Milliseconds;
                    return string.Format((useSmallFont ? TimeRecordTextFormatSmallMilliseconds : TimeRecordTextFormatMilliseconds), rankingText, timeRecordString, Mathf.RoundToInt(milliseconds * .1f));
                }
                else
                {
                    return string.Format((useSmallFont ? TimeRecordTextFormatSmall : TimeRecordTextFormat), rankingText, timeRecordString);
                }
            }
        }

        public static string GetFormattedRecordString(int ranking, bool useSmallFont = false)
        {
            string rankingText = (ranking < veryLastRankingNumber) ? ranking.ToString() : veryLastRankingNumber.ToString();

            if (RecordType == LeaderboardRecordType.Score)
            {
                return string.Format((useSmallFont ? ScoreTextFormatSmall : ScoreTextFormat), rankingText, Score.CurrentScorePoint);
            }
            else
            {
                string timeRecordString = TimerUI.SecondsToTimespanString(Timer.CurrentTime, false);
                if (Timer.timerObject != null && Timer.timerObject.showMilliseconds)
                {
                    int milliseconds = TimeSpan.FromSeconds(Timer.CurrentTime).Milliseconds;
                    return string.Format((useSmallFont ? TimeRecordTextFormatSmallMilliseconds : TimeRecordTextFormatMilliseconds), rankingText, timeRecordString, Mathf.RoundToInt(milliseconds * .1f));
                }
                else
                {
                    return string.Format((useSmallFont ? TimeRecordTextFormatSmall : TimeRecordTextFormat), rankingText, timeRecordString);
                }
            }
        }

        public static string GetRecordString(int ranking, System.Collections.ObjectModel.ReadOnlyCollection<LeaderboardEntry> rangedEntries)
        {
            if (RecordType == LeaderboardRecordType.Score && ranking >= 1)
            {
                if (rangedEntries.Count >= ranking)
                {
                    return string.Format("{0}점", rangedEntries[ranking - 1].score);
                }
                else
                {
                    return string.Format("0점");
                }
            }
            else
            {
                if (rangedEntries.Count >= ranking)
                {
                    return TimerUI.SecondsToTimespanString(rangedEntries[ranking - 1].timeRecord, false);
                }
                else
                {
                    return "기록없음";
                }
            }
        }

        public static string GetRecordString(int ranking)
        {
            if (RecordType == LeaderboardRecordType.Score)
            {
                return string.Format("{0}점", Score.CurrentScorePoint);
            }
            else
            {
                return TimerUI.SecondsToTimespanString(Timer.CurrentTime, false);
            }
        }
    }
}