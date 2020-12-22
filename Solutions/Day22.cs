namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Day22 : Solution
    {
        public override void Solve(string dataPath)
        {
            var data = File.ReadAllText(dataPath);
            var decks = data.Split("\n\n");
            var player1Deck = new Queue<int>(decks[0].Split('\n').Skip(1).Select(s => int.Parse(s)));
            var player2Deck = new Queue<int>(decks[1].Split('\n').Skip(1).Select(s => int.Parse(s)));

            Console.WriteLine($"Winning player's score: {CalculateWinnerScore(player1Deck, player2Deck)}");
        }

        private static long CalculateWinnerScore(Queue<int> deck1, Queue<int> deck2)
        {
            while (deck1.Count != 0 && deck2.Count != 0)
            {
                if (deck1.Peek() < deck2.Peek())
                {
                    deck2.Enqueue(deck2.Dequeue()); // winners card first
                    deck2.Enqueue(deck1.Dequeue());
                }
                else
                {
                    deck1.Enqueue(deck1.Dequeue()); // winners card first
                    deck1.Enqueue(deck2.Dequeue());
                }
            }

            var score = 0L;
            var winningDeck = deck1.Count == 0 ? deck2 : deck1;
            for (var i = winningDeck.Count; i > 0; i--)
            {
                score += winningDeck.Dequeue() * i;
            }

            return score;
        }
    }
}
