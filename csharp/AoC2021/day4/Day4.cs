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
                .OrderBy(x => x.CardId);

            var wonCards = new List<int>();

            for (var i = 0; i < numbers.Count(); i++)
            {
                var readNumbers = numbers.Take(i + 1);
                foreach (var card in cards.Where(x => !wonCards.Contains(x.CardId)))
                {
                    if (HasWon(card, readNumbers))
                    {
                        wonCards.Add(card.CardId);
                        Console.WriteLine($"Card {card.CardId} won with a score of {GetCardScore(card, readNumbers)} and the last number read was {numbers[i]}");
                    }
                }
            }
        }
    }
}
