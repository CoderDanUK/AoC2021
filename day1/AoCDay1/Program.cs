using System;
using System.IO;
using System.Linq;

namespace AoCDay1
{
    class Program
    {
        static void Main(string[] args)
        {
            var readings = File.ReadAllText("input.txt")
                .Split("\r\n")
                .Select(x => int.Parse(x))
                .ToList();

            var larger = 0;
            for (var i = 1; i < readings.Count(); i++)
            {
                if (readings[i] > readings[i - 1]) larger++;
            }
            Console.WriteLine($"{larger} readings are larger than the previous");
        }
    }
}
