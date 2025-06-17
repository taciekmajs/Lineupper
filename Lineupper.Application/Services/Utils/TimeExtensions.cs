using Lineupper.Application.Algorithms;
using Lineupper.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineupper.Application.Services.Utils
{
    public static class TimeExtensions
    {
        public static int Hours(this TimeSpan ts) => ts.Hours + ts.Days * 24;

        public static List<(int Start, int End)> GenerateAvailableSlots(Festival festival)
        {
            var slots = new List<(int Start, int End)>();

            var totalDays = (festival.EndDate.Date - festival.StartDate.Date).Days + 1;

            for (int day = 0; day < totalDays; day++)
            {
                int startMinutes = LineupGenerator.ToMinutes(festival.ConcertStartTime.Hours(), day);
                int endMinutes = LineupGenerator.ToMinutes(festival.ConcertEndTime.Hours(), day);

                slots.Add((startMinutes, endMinutes));
            }

            return slots;
        }
    }

}
