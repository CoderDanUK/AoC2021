using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2021
{
    class Day4
    {
        public class BingoSquare
        {
            public int Row;
            public int Column;
            public int Value;
        }

        public class BingoCard
        {
            public int CardId;
            public List<BingoSquare> Squares;
            public int CardScore = 0;
            public int WinningNumber = 0;
            public int WinningRead = 0;

            public string WinningText() => $"Card {CardId} won with a score of {CardScore} and the last number read was {WinningNumber}";

            public List<int> Numbers() => Squares.Select(x => x.Value).ToList();
        }

        public static bool HasWon(BingoCard card, IEnumerable<int> readNumbers) =>
            card.Squares.GroupBy(x => x.Column).Any(x => x.All(y => readNumbers.Contains(y.Value))) ||
            card.Squares.GroupBy(x => x.Row).Any(x => x.All(y => readNumbers.Contains(y.Value)));

       public static int GetCardScore(BingoCard card, IEnumerable<int> readNumbers)
       {
            var unMarked = card.Numbers().Where(x => !readNumbers.Contains(x));
            return unMarked.Sum();
       }
    
        public static void Run()
        {
            var inputText = File.ReadAllLines("day4/Input.txt");
            var numbers = inputText[0].Split(",").Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x)).ToList();
            var cards = inputText.Where((x, i) => i > 1 && !string.IsNullOrEmpty(x))
                .Select((x, i) => new { Numbers = x.Split(" ").Where(y => !string.IsNullOrEmpty(y)).Select(y => int.Parse(y)), Index = i })
                .GroupBy(x => x.Index / 5)
                .Select((cardGroup, cardId) => new BingoCard()
                {
                    CardId = cardGroup.Key,
                    Squares = cardGroup.SelectMany((row, rowId) => row.Numbers.Select((value, columnId) => new BingoSquare()
                    {
                        Row = rowId, 
                        Column = columnId, 
                        Value = value
                    })).ToList()
                })
                .OrderBy(x => x.CardId)
                .ToList();

            var wonCards = new List<int>();

            for (var i = 0; i < numbers.Count(); i++)
            {
                var readNumbers = numbers.Take(i + 1);
                for (var j = 0; j < cards.Count(); j++)
                {
                    if (HasWon(cards[j], readNumbers) && cards[j].WinningRead == 0)
                    {
                        wonCards.Add(cards[j].CardId);
                        cards[j].WinningRead = i;
                        cards[j].CardScore = GetCardScore(cards[j], readNumbers);
                        cards[j].WinningNumber = numbers[i];
                    }
                }
            }

            var ordered = cards.Where(x => x.WinningRead > 0).OrderBy(x => x.WinningRead);

            var first = ordered.First();
            var last = ordered.Last();

            Console.WriteLine(first.WinningText());
            Console.WriteLine(last.WinningText());
        }
    }
}
