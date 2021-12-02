using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2021
{
    public class Day1
    {
        private static void Part1(List<Reading> readings)
        {
            var larger = readings
                .Count(x => x.Index > 0 && x.Value > readings[x.Index - 1].Value);

            Console.WriteLine($"Part 1 {larger} readings are larger than the previous");
        }

        private static void Part2(List<Reading> readings)
        {
            var slidingReadings = readings
                .Where(x => x.Index < readings.Count - 2)
                .Select((x, i) => new SlidingReading() 
                { 
                    Index = i, 
                    Readings = new List<Reading>() 
                    { 
                        readings[i], 
                        readings[i + 1],
                        readings[i + 2] 
                    } 
                })
                .ToList();

            var larger = slidingReadings.Where(x => x.Index > 0).Count(x => slidingReadings[x.Index].Total > slidingReadings[x.Index - 1].Total);
            Console.WriteLine($"Part 2 {larger} readings are larger than the previous");
        }

        public static void Run()
        {
            var readings = File.ReadAllText("day1/input.txt")
                .Split("\r\n").Select((x, i) => new Reading
                {
                    Value = int.Parse(x),
                    Index = i
                }).ToList();

            Part1(readings);
            Part2(readings);
        }

        private record Reading
        {
            public int Value;
            public int Index;
        }

        private record SlidingReading
        {
            public int Index;
            public List<Reading> Readings;
            public int Total => Readings.Sum(x => x.Value);
        }
    }
}
