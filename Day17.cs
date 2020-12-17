namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class PocketDimension : Dictionary<(int x, int y, int z), bool> { }

    public static class Day17
    {
        public static void Solve()
        {
            var starterRegionData = File.ReadAllLines("Day17.data");
            var initialPocketDimension = ParseStarterRegion(starterRegionData);
            ProcessBootCycles(ref initialPocketDimension, starterRegionData.Length, starterRegionData[0].Length);
            var numberOfActiveCubes = initialPocketDimension.Count(pos => pos.Value == true);
            Console.WriteLine($"Number of active cubes after boot: {numberOfActiveCubes}");
        }

        private static PocketDimension ParseStarterRegion(string[] starterRegionData)
        {
            var pocketDimension = new PocketDimension();
            for (var y = 0; y < starterRegionData.Length; y++)
            {
                for (var x = 0; x < starterRegionData[y].Length; x++)
                {
                    pocketDimension[(x, y, 0)] = starterRegionData[y][x] == '#' ? true : false;
                }
            }

            return pocketDimension;
        }

        /**
         * <param name="srh">Starter region height</param>
         * <param name="srw">Starter region width</param>
         */
        private static void ProcessBootCycles(ref PocketDimension currentState, int srw, int srh)
        {
            PocketDimension tmpStateRef;
            var nextState = new PocketDimension();
            for (var i = 1; i < 7; i++) // expand space by 1 for each cycle
            {
                var positions = Get3DIterator(
                    startX: 0 - i,
                    endX:   srw + i,
                    startY: 0 - i,
                    endY:   srh + i,
                    startZ: 0 - i,
                    endZ:   i + 1);

                foreach (var pos in positions)
                {
                    nextState[pos] = CalculateNextState(currentState, pos);
                }

                tmpStateRef = i % 2 == 1 ? currentState : nextState;
                if (i % 2 == 1)
                {
                    tmpStateRef = currentState;
                    currentState = nextState;
                    nextState = tmpStateRef;
                }
                else
                {
                    tmpStateRef = nextState;
                    nextState = currentState;
                    currentState = tmpStateRef;
                }
            }
        }

        private static bool CalculateNextState(PocketDimension currentState, (int x, int y, int z) cubePos)
        {
            var numberOfActiveNeighbours = 0;

            foreach (var pos in Get3DIterator(cubePos.x - 1, cubePos.x + 2, cubePos.y - 1, cubePos.y + 2, cubePos.z - 1, cubePos.z + 2))
            {
                if (pos.x != cubePos.x || pos.y != cubePos.y || pos.z != cubePos.z)
                {
                    if (currentState.TryGetValue((pos.x, pos.y, pos.z), out var s) && s == true)
                    {
                        numberOfActiveNeighbours += 1;
                    }
                }
            }

            if (currentState.TryGetValue(cubePos, out var state) && state == true)
            {
                return numberOfActiveNeighbours == 2 || numberOfActiveNeighbours == 3;
            }
            else
            {
                return numberOfActiveNeighbours == 3;
            }
        }

        private static IEnumerable<(int x, int y, int z)> Get3DIterator(int startX, int endX, int startY, int endY, int startZ, int endZ)
        {
            for (var x = startX; x < endX ; x++)
            {
                for (var y = startY; y < endY; y++)
                {
                    for (var z = startZ; z < endZ; z++)
                    {
                        yield return (x, y, z);
                    }
                }
            }
        }
    }
}
