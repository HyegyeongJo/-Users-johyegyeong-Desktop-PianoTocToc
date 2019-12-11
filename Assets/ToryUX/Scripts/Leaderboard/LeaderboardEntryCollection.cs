using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ToryUX
{
    [Serializable]
    public class LeaderboardEntryCollection
    {
        public List<LeaderboardEntry> entries;

        public LeaderboardEntryCollection(List<LeaderboardEntry> entries)
        {
            this.entries = entries;
        }

        public LeaderboardEntryCollection()
        {
            this.entries = new List<LeaderboardEntry>();
        }

        public List<LeaderboardEntry> QueryToday()
        {
            IEnumerable<LeaderboardEntry> e = from entry in entries where(entry.UtcDateTime.ToLocalTime().Year == DateTime.Now.Year &&
                entry.UtcDateTime.ToLocalTime().Month == DateTime.Now.Month &&
                entry.UtcDateTime.ToLocalTime().Day == DateTime.Now.Day) select entry;

            return SortEntries(e);
        }

        public List<LeaderboardEntry> QueryThisWeek()
        {
            var calendar = new GregorianCalendar();
            // Below returns a list of entries that match the week number of this week without checking year; data from the same week of any year could be mixed if the leadeerboard has more than enough entries.

            IEnumerable<LeaderboardEntry> e = from entry in entries where calendar.GetWeekOfYear(entry.UtcDateTime.ToLocalTime(), CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday) == calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday) select entry;

            return SortEntries(e);
        }

        public List<LeaderboardEntry> QueryThisMonth()
        {
            IEnumerable<LeaderboardEntry> e = from entry in entries where entry.UtcDateTime.ToLocalTime().Year == DateTime.Now.Year &&
                entry.UtcDateTime.ToLocalTime().Month == DateTime.Now.Month select entry;

            return SortEntries(e);
        }

        public List<LeaderboardEntry> QueryLastNDays(int n)
        {
            IEnumerable<LeaderboardEntry> e = from entry in entries where(entry.UtcDateTime > DateTime.UtcNow.AddDays(-n) &&
                entry.UtcDateTime <= DateTime.UtcNow) select entry;

            return SortEntries(e);
        }

        public List<LeaderboardEntry> SortEntries(IEnumerable<LeaderboardEntry> e)
        {
            if (Leaderboard.RecordType == LeaderboardRecordType.Score)
            {
                return e.OrderByDescending(entry => entry.score).ThenByDescending(entry => entry.UtcDateTime).ToList();
            }
            else if (Leaderboard.RecordType == LeaderboardRecordType.Stopwatch)
            {
                return e.OrderBy(entry => entry.timeRecord).ThenByDescending(entry => entry.UtcDateTime).ToList();
            }
            else // if (Leaderboard.RecordType == LeaderboardRecordType.Countdown)
            {
                return e.OrderByDescending(entry => entry.timeRecord).ThenByDescending(entry => entry.UtcDateTime).ToList();
            }
        }

        public List<LeaderboardEntry> QueryRecentNEntries(int n)
        {
            return entries.OrderByDescending(entry => entry.UtcDateTime).Take(Math.Max(5, n)).ToList();
        }

        public List<LeaderboardEntry> QueryRecentNEntriesOrInLastMDays(int n, int m)
        {
            List<LeaderboardEntry> orderedEntries = entries.OrderByDescending(entry => entry.UtcDateTime).ToList();

            IEnumerable<LeaderboardEntry> e = from entry in orderedEntries
            where
                (
                    orderedEntries.IndexOf(entry) < n ||
                    (entry.UtcDateTime > DateTime.UtcNow.AddDays(-m) && entry.UtcDateTime <= DateTime.UtcNow)
                )
            select entry;

            return SortEntries(e);
        }
    }
}