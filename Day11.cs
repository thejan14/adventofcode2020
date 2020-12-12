namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class Day11
    {
        private enum State
        {
            Empty,
            Occupied,
            Floor
        }

        private delegate IEnumerable<State> GetAdjacentSeats(State[,] stateMap, int row, int column);

        public static void Solve()
        {
            var data = File.ReadAllLines("Day11.data");
            var stateMap = ParseLayout(data);
            Console.WriteLine($"(1) Final occupied seats: {GetFinalStateOccupiedSeats(stateMap.Clone() as State[,], 3, GetDirectlyAdjacentSeats)}");
            Console.WriteLine($"(2) Final occupied seats (updated rules): {GetFinalStateOccupiedSeats(stateMap.Clone() as State[,], 4, GetFirstSeatsInSight)}");
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

        private static int GetFinalStateOccupiedSeats(State[,] stateMap, int toleranceThreshold, GetAdjacentSeats adjacentSeatsFunc)
        {
            var countStates = new SortedSet<int>();
            var nextCountState = stateMap.Cast<State>().Count(s => s == State.Occupied);
            var nextStateMap = new State[stateMap.GetLength(0), stateMap.GetLength(1)];

            // calculate stateMap's new state in nextStateMap and nextStatesMap new state in stateMap in alternating order
            // (avoids creating lots of new 2d arrays)
            var i = 0;
            while (!countStates.Contains(nextCountState))
            {
                i += 1;
                countStates.Add(nextCountState);
                if (i % 2 == 1)
                {
                    CalculateNextState(stateMap, nextStateMap, toleranceThreshold, adjacentSeatsFunc);
                    nextCountState = nextStateMap.Cast<State>().Count(s => s == State.Occupied);
                }
                else
                {
                    CalculateNextState(nextStateMap, stateMap, toleranceThreshold, adjacentSeatsFunc);
                    nextCountState = stateMap.Cast<State>().Count(s => s == State.Occupied);
                }
            }

            return nextCountState;
        }

        private static void CalculateNextState(State[,] stateMap, State[,] newStateMap, int toleranceThreshold, GetAdjacentSeats adjacentSeatsFunc)
        {
            for (var row = 0; row < stateMap.GetLength(0); row++)
            {
                for (var column = 0; column < stateMap.GetLength(1); column++)
                {
                    var adjacent = adjacentSeatsFunc(stateMap, row, column);
                    newStateMap[row, column] = stateMap[row, column] switch
                    {
                        State.Empty => adjacent.Count(s => s == State.Occupied) == 0 ? State.Occupied : State.Empty,
                        State.Occupied => adjacent.Count(s => s == State.Occupied) > toleranceThreshold ? State.Empty : State.Occupied,
                        _ => State.Floor
                    };
                }
            }
        }

        private static IEnumerable<State> GetDirectlyAdjacentSeats(this State[,] stateMap, int row, int column)
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

        private static IEnumerable<State> GetFirstSeatsInSight(this State[,] stateMap, int row, int column)
        {
            yield return GetFirstSeatUp(stateMap, row, column);
            yield return GetFirstSeatUpRight(stateMap, row, column);
            yield return GetFirstSeatRight(stateMap, row, column);
            yield return GetFirstSeatDownRight(stateMap, row, column);
            yield return GetFirstSeatDown(stateMap, row, column);
            yield return GetFirstSeatDownLeft(stateMap, row, column);
            yield return GetFirstSeatLeft(stateMap, row, column);
            yield return GetFirstSeatUpLeft(stateMap, row, column);
        }

        private static State GetFirstSeatUp(this State[,] stateMap, int row, int column)
        {
            for (var i = row - 1; i >= 0; i--)
            {
                if (stateMap[i, column] != State.Floor)
                {
                    return stateMap[i, column];
                }
            }

            return State.Floor;
        }

        private static State GetFirstSeatUpRight(this State[,] stateMap, int row, int column)
        {
            var i = row - 1;
            var j = column + 1;
            while (i >= 0 && j < stateMap.GetLength(1))
            {
                if (stateMap[i, j] != State.Floor)
                {
                    return stateMap[i, j];
                }
                else
                {
                    i -= 1;
                    j += 1;
                }
            }

            return State.Floor;
        }

        private static State GetFirstSeatRight(this State[,] stateMap, int row, int column)
        {
            for (var j = column + 1; j < stateMap.GetLength(1); j++)
            {
                if (stateMap[row, j] != State.Floor)
                {
                    return stateMap[row, j];
                }
            }

            return State.Floor;
        }

        private static State GetFirstSeatDownRight(this State[,] stateMap, int row, int column)
        {
            var i = row + 1;
            var j = column + 1;
            while (i < stateMap.GetLength(0) && j < stateMap.GetLength(1))
            {
                if (stateMap[i, j] != State.Floor)
                {
                    return stateMap[i, j];
                }
                else
                {
                    i += 1;
                    j += 1;
                }
            }

            return State.Floor;
        }

        private static State GetFirstSeatDown(this State[,] stateMap, int row, int column)
        {
            for (var i = row + 1; i < stateMap.GetLength(0); i++)
            {
                if (stateMap[i, column] != State.Floor)
                {
                    return stateMap[i, column];
                }
            }

            return State.Floor;
        }

        private static State GetFirstSeatDownLeft(this State[,] stateMap, int row, int column)
        {
            var i = row + 1;
            var j = column - 1;
            while (i < stateMap.GetLength(0) && j >= 0)
            {
                if (stateMap[i, j] != State.Floor)
                {
                    return stateMap[i, j];
                }
                else
                {
                    i += 1;
                    j -= 1;
                }
            }

            return State.Floor;
        }

        private static State GetFirstSeatLeft(this State[,] stateMap, int row, int column)
        {
            for (var j = column - 1; j >= 0; j--)
            {
                if (stateMap[row, j] != State.Floor)
                {
                    return stateMap[row, j];
                }
            }

            return State.Floor;
        }

        private static State GetFirstSeatUpLeft(this State[,] stateMap, int row, int column)
        {
            var i = row - 1;
            var j = column - 1;
            while (i >= 0 && j >= 0)
            {
                if (stateMap[i, j] != State.Floor)
                {
                    return stateMap[i, j];
                }
                else
                {
                    i -= 1;
                    j -= 1;
                }
            }

            return State.Floor;
        }
    }
}
