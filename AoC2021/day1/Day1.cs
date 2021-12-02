using System;
using System.IO;
using System.Linq;

namespace AoC2021
{
    public class Day1
    {
        public static void Run()
        {
            var readings = File.ReadAllText("day1/input.txt")
                .Split("\r\n").Select((x, i) => new
                {
                    Value = int.Parse(x),
                    Index = i
                }).ToList();

            var larger = readings
                .Count(x => x.Index > 0 && x.Value > readings[x.Index - 1].Value);

            Console.WriteLine($"{larger} readings are larger than the previous");
        }
    }
}
