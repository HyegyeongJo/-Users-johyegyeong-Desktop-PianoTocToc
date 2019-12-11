using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToryUX
{
    public class CamLeaderboardUI : MonoBehaviour
    {
        private static int trimCountApplied = int.MaxValue;

        public static void Init(int trimCount = -1)
        {
            if (trimCount == -1)
            {
                trimCount += int.MaxValue;
            }

            // In case Init() is called multiple times.
            if (trimCount < trimCountApplied)
            {
                Leaderboard.ReadFromFile();
                // Leaderboard.TrimToLastNDays(7);
                // Leaderboard.RemoveIsolatedPhotoFiles(trimCount);

                try
                {
                    Score.ResetHighScore(Leaderboard.EntriesLastSevenDays[0].score);
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    // Score.ResetHighScore();
                }

                try
                {
                    Timer.ResetHighRecordTime(Leaderboard.EntriesLastSevenDays[0].timeRecord);
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    // Timer.ResetHighRecordTime();
                }
            }
        }

        public virtual void OnDestroy()
        {
            trimCountApplied = int.MaxValue;
        }
    }
}