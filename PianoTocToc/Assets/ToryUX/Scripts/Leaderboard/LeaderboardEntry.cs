using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ToryUX
{
    [Serializable]
    public class LeaderboardEntry
    {
        public long utcDateTimeBinary;
        public int score;
        public float timeRecord;

        private Texture2D portraitPhoto = null;
        public Texture2D PortraitPhoto
        {
            get
            {
                if (portraitPhoto == null)
                {
                    try
                    {
                        portraitPhoto = new Texture2D(1, 1);
                        portraitPhoto.LoadImage(File.ReadAllBytes(Path.Combine(Leaderboard.TorywardDirectory, string.Format(ToryCare.Config.TorywardImageFormat, utcDateTimeBinary.ToString()))));
                    }
                    catch (Exception)
                    {
                        portraitPhoto = null;
                    }
                }
                return portraitPhoto;
            }
        }

        public DateTime UtcDateTime
        {
            get
            {
                return DateTime.FromBinary(utcDateTimeBinary).ToUniversalTime();
            }
        }

        /// <summary>
        /// Create LeaderboardEntry using current record, depend on leaderboard's record type
        /// </summary>
        public LeaderboardEntry()
        {
            this.utcDateTimeBinary = System.DateTime.UtcNow.ToBinary();
            if (Leaderboard.RecordType == LeaderboardRecordType.Score)
            {
                this.score = Score.CurrentScorePoint;
            }
            else
            {
                this.timeRecord = Timer.CurrentTime;
            }
        }

        /// <summary>
        /// Create LeaderboardEntry with photo using current record, depend on leaderboard's record type
        /// </summary>
        public LeaderboardEntry(byte[] imageBytes) : this()
        {
            try
            {
                string filepath = Path.Combine(Leaderboard.TorywardDirectory, string.Format(ToryCare.Config.TorywardImageFormat, utcDateTimeBinary.ToString()));
                File.WriteAllBytes(filepath, imageBytes);
            }
            catch (ArgumentException)
            {
                Debug.LogError("Creating new LeaderboardEntry with imageBytes failed because the path was invalid.");
            }
        }

        /*
        /// <summary>
        /// Create LeaderboardEntry using both <c>score</c> and <c>timeRecord</c>.
        /// </summary>
        public LeaderboardEntry(DateTime utcDateTime, int score, float timeRecord)
        {
            this.utcDateTimeBinary = utcDateTime.ToBinary();
            this.score = score;
            this.timeRecord = timeRecord;
        }

        /// <summary>
        /// Create LeaderboardEntry using <c>score</c>.
        /// </summary>
        public LeaderboardEntry(DateTime utcDateTime, int score) : this(utcDateTime, score, 0f)
        {}

        /// <summary>
        /// Create LeaderboardEntry using <c>timeRecord</c>.
        /// </summary>
        public LeaderboardEntry(DateTime utcDateTime, float timeRecord) : this(utcDateTime, 0, timeRecord)
        {}

        /// <summary>
        /// Create LeaderboardEntry with photo using both <c>score</c> and <c>timeRecord</c>.
        /// </summary>
        public LeaderboardEntry(byte[] imageBytes, DateTime utcDateTime, int score, float timeRecord) : this(utcDateTime, score, timeRecord)
        {
            try
            {
                File.WriteAllBytes(Path.Combine(Application.persistentDataPath, string.Format(FileNameFormat, utcDateTimeBinary.ToString())), imageBytes);
            }
            catch (ArgumentException)
            {
                Debug.LogError("Creating new LeaderboardEntry with imageBytes failed because the path was invalid.");
            }
        }

        /// <summary>
        /// Create LeaderboardEntry with photo using <c>score</c>.
        /// </summary>
        public LeaderboardEntry(byte[] imageBytes, DateTime utcDateTime, int score) : this(imageBytes, utcDateTime, score, 0f)
        {}

        /// <summary>
        /// Create LeaderboardEntry with photo using <c>timeRecord</c>.
        /// </summary>
        public LeaderboardEntry(byte[] imageBytes, DateTime utcDateTime, float timeRecord) : this(imageBytes, utcDateTime, 0, timeRecord)
        {}
        */

        public void NullifyPortraitPhoto()
        {
            portraitPhoto = null;
        }
    }
}