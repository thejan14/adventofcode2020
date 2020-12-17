namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class PocketDimension3D : Dictionary<(int x, int y, int z), bool> { }

    public class PocketDimension4D : Dictionary<(int x, int y, int z, int w), bool> { }

    public static class Day17
    {
        public static void Solve()
        {
            var starterRegionData = File.ReadAllLines("Day17.data");

            var initialPocketDimension3D = ParseStarterRegionTo3D(starterRegionData);
            ProcessBootCycles(ref initialPocketDimension3D, starterRegionData.Length, starterRegionData[0].Length);
            var numberOfActiveCubes3D = initialPocketDimension3D.Count(pos => pos.Value == true);
            Console.WriteLine($"(1) Number of active cubes after 3D boot: {numberOfActiveCubes3D}");

            var initialPocketDimension4D = ParseStarterRegionTo4D(starterRegionData);
            ProcessBootCycles(ref initialPocketDimension4D, starterRegionData.Length, starterRegionData[0].Length);
            var numberOfActiveCubes4D = initialPocketDimension4D.Count(pos => pos.Value == true);
            Console.WriteLine($"(1) Number of active cubes after 4D boot: {numberOfActiveCubes4D}");
        }

        private static PocketDimension3D ParseStarterRegionTo3D(string[] starterRegionData)
        {
            var pocketDimension = new PocketDimension3D();
            for (var y = 0; y < starterRegionData.Length; y++)
            {
                for (var x = 0; x < starterRegionData[y].Length; x++)
                {
                    pocketDimension[(x, y, 0)] = starterRegionData[y][x] == '#' ? true : false;
                }
            }

            return pocketDimension;
        }

        private static PocketDimension4D ParseStarterRegionTo4D(string[] starterRegionData)
        {
            var pocketDimension = new PocketDimension4D();
            for (var y = 0; y < starterRegionData.Length; y++)
            {
                for (var x = 0; x < starterRegionData[y].Length; x++)
                {
                    pocketDimension[(x, y, 0, 0)] = starterRegionData[y][x] == '#' ? true : false;
                }
            }

            return pocketDimension;
        }

        /**
         * <param name="srh">Starter region height</param>
         * <param name="srw">Starter region width</param>
         */
        private static void ProcessBootCycles(ref PocketDimension3D currentState, int srw, int srh)
        {
            PocketDimension3D tmpStateRef;
            var nextState = new PocketDimension3D();
            for (var i = 1; i < 7; i++) // expand space by 1 for each cycle
            {
                var positions = Get3DIterator(
                    startX: 0 - i,
                    endX: srw + i,
                    startY: 0 - i,
                    endY: srh + i,
                    startZ: 0 - i,
                    endZ: i + 1);

                foreach (var pos in positions)
                {
                    nextState[pos] = CalculateNextState(currentState, pos);
                }

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

        /**
         * <param name="srh">Starter region height</param>
         * <param name="srw">Starter region width</param>
         */
        private static void ProcessBootCycles(ref PocketDimension4D currentState, int srw, int srh)
        {
            PocketDimension4D tmpStateRef;
            var nextState = new PocketDimension4D();
            for (var i = 1; i < 7; i++) // expand space by 1 for each cycle
            {
                var positions = Get4DIterator(
                    startX: 0 - i,
                    endX: srw + i,
                    startY: 0 - i,
                    endY: srh + i,
                    startZ: 0 - i,
                    endZ: i + 1,
                    startW: 0 - i,
                    endW: i + 1);

                foreach (var pos in positions)
                {
                    nextState[pos] = CalculateNextState(currentState, pos);
                }

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

        private static bool CalculateNextState(PocketDimension3D currentState, (int x, int y, int z) cubePos)
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

        private static bool CalculateNextState(PocketDimension4D currentState, (int x, int y, int z, int w) cubePos)
        {
            var numberOfActiveNeighbours = 0;

            foreach (var pos in Get4DIterator(cubePos.x - 1, cubePos.x + 2, cubePos.y - 1, cubePos.y + 2, cubePos.z - 1, cubePos.z + 2, cubePos.w - 1, cubePos.w + 2))
            {
                if (pos.x != cubePos.x || pos.y != cubePos.y || pos.z != cubePos.z || pos.w != cubePos.w)
                {
                    if (currentState.TryGetValue((pos.x, pos.y, pos.z, pos.w), out var s) && s == true)
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
            for (var x = startX; x < endX; x++)
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

        private static IEnumerable<(int x, int y, int z, int w)> Get4DIterator(int startX, int endX, int startY, int endY, int startZ, int endZ, int startW, int endW)
        {
            for (var x = startX; x < endX; x++)
            {
                for (var y = startY; y < endY; y++)
                {
                    for (var z = startZ; z < endZ; z++)
                    {
                        for (var w = startW; w < endW; w++)
                        {
                            yield return (x, y, z, w);
                        }
                    }
                }
            }
        }
    }
}
