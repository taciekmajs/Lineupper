using System;
using System.Collections.Generic;
using System.Linq;

namespace Lineupper.Application.Algorithms
{
    public static class LineupGenerator
    {
        public record PerformanceSlot(Guid BandId, int Stage, int StartMinute, int EndMinute);

        private const int BreakBetweenSets = 15; // w minutach

        public static List<PerformanceSlot> GenerateLineup(
            Dictionary<Guid, Dictionary<Guid, int>> votesAsDict,
            Dictionary<Guid, int> setDurations,
            List<(int Start, int End)> availableSlots)
        {
            // 1. Oblicz wszystkie minuty dostępne na wszystkich scenach
            var totalAvailableMinutes = availableSlots.Sum(slot => slot.End - slot.Start);
            var totalSetMinutes = setDurations.Values.Sum() + BreakBetweenSets * setDurations.Count;

            int minStages = (int)Math.Ceiling((double)totalSetMinutes / totalAvailableMinutes);

            // Sortuj zespoły od najczęściej głosowanych
            var bandPopularity = votesAsDict
                .SelectMany(userVotes => userVotes.Value.Keys)
                .GroupBy(bid => bid)
                .ToDictionary(g => g.Key, g => g.Count());

            var bandsSorted = bandPopularity
                .OrderByDescending(kv => kv.Value)
                .Select(kv => kv.Key)
                .ToList();

            var usedSlots = new List<PerformanceSlot>();

            foreach (var bandId in bandsSorted)
            {
                int setDuration = setDurations[bandId];
                int durationWithBreak = setDuration + BreakBetweenSets;

                bool placed = false;
                for (int stage = 0; stage < minStages && !placed; stage++)
                {
                    foreach (var slot in availableSlots)
                    {
                        for (int minute = slot.Start; minute + durationWithBreak <= slot.End; minute++)
                        {
                            int endTime = minute + setDuration;

                            bool overlaps = usedSlots.Any(s =>
                                s.Stage == stage &&
                                !(endTime + BreakBetweenSets <= s.StartMinute || minute >= s.EndMinute + BreakBetweenSets));

                            bool similarityConflict = usedSlots.Any(s =>
                                s.StartMinute < endTime &&
                                minute < s.EndMinute &&
                                GetSimilarity(bandId, s.BandId, votesAsDict) > 0.6);

                            if (!overlaps && !similarityConflict)
                            {
                                usedSlots.Add(new PerformanceSlot(bandId, stage, minute, endTime));
                                placed = true;
                                break;
                            }
                        }
                        if (placed) break;
                    }
                }

                if (!placed)
                    throw new Exception($"Nie udało się przypisać zespołu {bandId} do żadnej sceny i przedziału czasu.");
            }

            return usedSlots;
        }

        private static double GetSimilarity(Guid a, Guid b, Dictionary<Guid, Dictionary<Guid, int>> votes)
        {
            var commonUsers = votes.Where(v => v.Value.ContainsKey(a) && v.Value.ContainsKey(b)).ToList();
            if (!commonUsers.Any()) return 0;

            var aVotes = commonUsers.Select(v => v.Value[a]).ToList();
            var bVotes = commonUsers.Select(v => v.Value[b]).ToList();

            double avgA = aVotes.Average();
            double avgB = bVotes.Average();

            double numerator = 0, denomA = 0, denomB = 0;
            for (int i = 0; i < aVotes.Count; i++)
            {
                var da = aVotes[i] - avgA;
                var db = bVotes[i] - avgB;
                numerator += da * db;
                denomA += da * da;
                denomB += db * db;
            }

            return denomA == 0 || denomB == 0 ? 0 : numerator / Math.Sqrt(denomA * denomB);
        }

        public static int ToMinutes(int hour, int dayIndex) => dayIndex * 24 * 60 + hour * 60;
    }
}