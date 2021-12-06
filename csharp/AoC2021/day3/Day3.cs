using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2021
{
    class Day3
    {
        public static T LeastCommon<T>(IEnumerable<T> characters)
        {
            return characters.GroupBy(x => x)
                .OrderBy(x => x.Count())
                .ThenBy(x => x.Key)
                .First().Key;
        }

        public static T MostCommon<T>(IEnumerable<T> characters)
        {
            return characters.GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .ThenByDescending(x => x.Key)
                .First().Key;
        }

        public static int BinaryToInt(string binary)
        {
            var intValue = 1;
            var result = 0;
            for (var i = binary.Length - 1; i >= 0; i--)
            {
                if (binary[i] == '1') result += intValue;
                intValue *= 2;
            }
            return result;
        }

        public static string Part2FilterAlgorithm(List<string> lines, Func<IEnumerable<char>, char> filterFunc)
        {
            var filteredDigits = lines.ToList();

            var index = 0;
            while (filteredDigits.Count > 1)
            {
                var bitGroups = GetBitGroups(filteredDigits);
                var filterResult = filterFunc(bitGroups[index]);

                filteredDigits = filteredDigits
                    .Where(x => x[index] == filterResult)
                    .ToList();

                index++;
            }

            return filteredDigits.First();
        }

        private static List<List<char>> GetBitGroups(List<string> lines)
        {
            return lines
                .SelectMany((digit) => digit.Select((x, i) => new { Index = i, Char = x }))
                .GroupBy(x => x.Index)
                .Select(x => x.Select(y => y.Char).ToList())
                .ToList();
        }

        public static void Part2(List<string> lines)
        {
            var co2ScrubberRatingBinary = Part2FilterAlgorithm(lines, x => LeastCommon(x));
            var co2ScrubberRatingDecimal = BinaryToInt(co2ScrubberRatingBinary);

            var oxygenRatingBinary = Part2FilterAlgorithm(lines, x => MostCommon(x));
            var oxygenRatingDecimal = BinaryToInt(oxygenRatingBinary);

            var lifeSupportRating = oxygenRatingDecimal * co2ScrubberRatingDecimal;

            Console.WriteLine($"O2: {oxygenRatingBinary} = {oxygenRatingDecimal} CO2: {co2ScrubberRatingBinary} = {co2ScrubberRatingDecimal} Life Support: {lifeSupportRating}");
        }

        public static void Part1(List<string> lines)
        {
            var bitGroups = GetBitGroups(lines);

            var mostCommonBits = string.Join("", bitGroups.Select(x => MostCommon(x)));
            var leastCommonBits = string.Join("", bitGroups.Select(x => LeastCommon(x)));

            var mostCommonInt = BinaryToInt(mostCommonBits);
            var leastCommonInt = BinaryToInt(leastCommonBits);

            var powerConsumption = mostCommonInt * leastCommonInt;
            Console.WriteLine($"{mostCommonBits} = {mostCommonInt} {leastCommonBits} = {leastCommonInt}. {leastCommonInt} * {mostCommonInt} = {powerConsumption}");
        }

        public static void Run()
        {
            var lines = File.ReadAllText("day3/Input.txt").Split("\r\n").ToList();
            Part1(lines);
            Part2(lines);
        }
    }
}
