using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2021
{
    public class Day2
    {
        public record Submarine
        {
            public int Aim = 0;
            public int Horizontal = 0;
            public int Depth = 0;
            public int Multiplied => Horizontal * Depth;
        }

        public record Command
        {
            public string Text;
            public int Units;
        }

        private interface ICommandProcessor
        {
            Submarine Forward(int x, Submarine sub);
            Submarine Down(int y, Submarine sub);
            Submarine Up(int y, Submarine sub);
        }

        private class Part2 : ICommandProcessor
        {
            public Submarine Forward(int x, Submarine sub) =>
                new() { Horizontal = sub.Horizontal + x, Depth = sub.Depth + (x * sub.Aim), Aim = sub.Aim};

            public Submarine Down(int y, Submarine sub) =>
                new() { Horizontal = sub.Horizontal, Depth = sub.Depth, Aim = sub.Aim + y };

            public Submarine Up(int y, Submarine sub) =>
                Down(-y, sub);
        }

        private class Part1 : ICommandProcessor
        {
            public Submarine Forward(int x, Submarine sub) =>
                new() { Horizontal = sub.Horizontal + x, Depth = sub.Depth };

            public Submarine Down(int y, Submarine sub) =>
                new() { Horizontal = sub.Horizontal, Depth = sub.Depth + y };

            public Submarine Up(int y, Submarine sub) =>
                Down(-y, sub);
        }

        private static void ProcessCommands(IEnumerable<Command> commands, ICommandProcessor processor)
        {
            var sub = new Submarine();
            foreach (var command in commands)
            {
                switch (command.Text)
                {
                    case "forward": sub = processor.Forward(command.Units, sub); break;
                    case "down": sub = processor.Down(command.Units, sub); break;
                    case "up": sub = processor.Up(command.Units, sub); break;
                }
            }

            Console.WriteLine($"Horizontal: {sub.Horizontal} Depth: {sub.Depth} Multiplied: {sub.Multiplied}");
        }


        public static void Run()
        {
            Console.WriteLine("--- ADVENT OF CODE DAY 2 ---");
            var commands = File.ReadAllText("day2/input.txt")
                .Split("\r\n")
                .Select(x => x.Split(" "))
                .Select(x => new Command
                {
                    Text = x[0],
                    Units = int.Parse(x[1])
                });

            Console.WriteLine("Part 1");
            ProcessCommands(commands, new Part1());
            Console.WriteLine("Part 2");
            ProcessCommands(commands, new Part2());
        }
    }
}
