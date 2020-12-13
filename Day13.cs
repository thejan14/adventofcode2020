namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class Day13
    {
        public static void Solve()
        {
            var data = File.ReadAllLines("Day13.data");
            var departureTime = int.Parse(data[0]);
            var busIds = data[1].Split(",").Where(s => s != "x").Select(s => int.Parse(s)).ToList();
            var timesToNextDeparture = busIds.Select(id => id - (departureTime % id)).ToList();
            var nextBusIdIndex = timesToNextDeparture.IndexOf(timesToNextDeparture.Min());
            Console.WriteLine($"{busIds[nextBusIdIndex]} * {timesToNextDeparture[nextBusIdIndex]} = {busIds[nextBusIdIndex] * timesToNextDeparture[nextBusIdIndex]}");
        }
    }
}
