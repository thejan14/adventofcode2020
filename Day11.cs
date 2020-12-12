namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    public enum State
    {
        Floor,
        Empty,
        Occupied
    }

    public static class Day11
    {
        public static void Solve()
        {
            var data = File.ReadAllLines("Day11.data");
            var stateMap = ParseLayout(data);
            Console.WriteLine($"Final occupied seats: {GetFinalStateOccupiedSeats(stateMap)}");
        }


        private static State[,] ParseLayout(string[] data)
        {
            var stateMap = new State[data.Length, data[0].Length];
            for (var row = 0; row < data.Length; row++)
            {
                for (var column = 0; column < data[row].Length; column++)
                {
                    // initially there are no occupied seats
                    stateMap[row, column] = data[row][column] switch
                    {
                        'L' => State.Empty,
                        _ => State.Floor
                    };
                }
            }

            return stateMap;
        }

        private static int GetFinalStateOccupiedSeats(State[,] stateMap)
        {
            var states = new SortedSet<int>();
            var nextState = stateMap.CountOccupied();
            while (!states.Contains(nextState))
            {
                states.Add(nextState);
                stateMap = CalculateNextState(stateMap);
                nextState = stateMap.CountOccupied();
            }

            return nextState;
        }

        private static State[,] CalculateNextState(State[,] stateMap)
        {
            var newStateMap = new State[stateMap.GetLength(0), stateMap.GetLength(1)];
            for (var row = 0; row < stateMap.GetLength(0); row++)
            {
                for (var column = 0; column < stateMap.GetLength(1); column++)
                {
                    var adjacent = stateMap.GetAdjacentStates(row, column);
                    newStateMap[row, column] = stateMap[row, column] switch
                    {
                        State.Empty => adjacent.Count(s => s == State.Occupied) == 0 ? State.Occupied : State.Empty,
                        State.Occupied => adjacent.Count(s => s == State.Occupied) > 3 ? State.Empty : State.Occupied,
                        _ => State.Floor
                    };
                }
            }

            return newStateMap;
        }

        private static IEnumerable<State> GetAdjacentStates(this State[,] stateMap, int row, int column)
        {
            for (var i = Math.Max(0, row - 1); i < Math.Min(stateMap.GetLength(0), row + 2); i++)
            {
                for (var j = Math.Max(0, column - 1); j < Math.Min(stateMap.GetLength(1), column + 2); j++)
                {
                    if (i != row || j != column)
                    {
                        yield return stateMap[i, j];
                    }
                }
            }
        }

        private static int CountOccupied(this State[,] stateMap)
        {
            var count = 0;
            for (var row = 0; row < stateMap.GetLength(0); row++)
            {
                for (var column = 0; column < stateMap.GetLength(1); column++)
                {
                    if (stateMap[row, column] == State.Occupied)
                    {
                        count += 1;
                    }
                }
            }

            return count;
        }
    }
}
