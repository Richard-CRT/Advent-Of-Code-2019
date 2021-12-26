using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_24___Planet_of_Discord
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();

            GridState state = new GridState(inputList);

            HashSet<Int64> knownBiodiversities = new HashSet<Int64>();
            while (!knownBiodiversities.Contains(state.biodiversity))
            {
                knownBiodiversities.Add(state.biodiversity);

                state = state.Tick();
                //state.Print();
            }

            Console.WriteLine(state.biodiversity);
            Console.ReadLine();

            RecursiveGridState recursiveGridState = new RecursiveGridState(inputList);
            //recursiveGridState.Print();

            for (int i = 0; i < 200;i++)
            {
                recursiveGridState = recursiveGridState.Tick();
                //recursiveGridState.Print();
            }

            Console.WriteLine(recursiveGridState.bugCount);
            Console.ReadLine();
        }
    }

    public enum TileType { bug = '#', space = '.' };

    public class GridState
    {
        public TileType[][] grid;

        public long _biodiversity = -1;
        public long biodiversity
        {
            get
            {
                if (_biodiversity == -1)
                    _biodiversity = CalculateBiodiversity();
                return _biodiversity;
            }
        }

        public long _bugCount = -1;
        public long bugCount
        {
            get
            {
                if (_bugCount == -1)
                    _bugCount = CountBugs();
                return _bugCount;
            }
        }

        public GridState(List<string> inputLines)
        {
            grid = new TileType[5][];
            for (int y = 0; y < 5; y++)
            {
                grid[y] = new TileType[5];
                for (int x = 0; x < 5; x++)
                {
                    grid[y][x] = (TileType)inputLines[y][x];
                }
            }
        }

        public GridState(TileType[][] grid)
        {
            this.grid = grid;
        }

        public GridState Tick(bool P2 = false, GridState insetState = null, GridState enclosingState = null)
        {
            TileType[][] newGrid = new TileType[5][];
            for (int y = 0; y < 5; y++)
            {
                newGrid[y] = new TileType[5];
                for (int x = 0; x < 5; x++)
                {
                    if (!P2 || y != 2 || x != 2) // don't do adj bugs for middle
                    {
                        int adjacentBugs = 0;

                        if ((!P2 || y != 3 || x != 2) && y > 0 && grid[y - 1][x] == TileType.bug)
                            adjacentBugs++;
                        if ((!P2 || y != 1 || x != 2) && y < grid.Length - 1 && grid[y + 1][x] == TileType.bug)
                            adjacentBugs++;
                        if ((!P2 || y != 2 || x != 3) && x > 0 && grid[y][x - 1] == TileType.bug)
                            adjacentBugs++;
                        if ((!P2 || y != 2 || x != 1) && x < grid[0].Length - 1 && grid[y][x + 1] == TileType.bug)
                            adjacentBugs++;

                        if (P2)
                        {
                            // Checking enclosing state
                            if (enclosingState != null)
                            {
                                if (y == 0 && enclosingState.grid[1][2] == TileType.bug)
                                    adjacentBugs++;
                                else if (y == grid.Length - 1 && enclosingState.grid[3][2] == TileType.bug)
                                    adjacentBugs++;
                                if (x == 0 && enclosingState.grid[2][1] == TileType.bug)
                                    adjacentBugs++;
                                else if (x == grid[0].Length - 1 && enclosingState.grid[2][3] == TileType.bug)
                                    adjacentBugs++;
                            }

                            // Checking inset state
                            if (insetState != null)
                            {
                                if (y == 1 && x == 2)
                                {
                                    for (int i = 0; i < 5; i++)
                                        if (insetState.grid[0][i] == TileType.bug)
                                            adjacentBugs++;
                                }
                                else if (y == 3 && x == 2)
                                {
                                    for (int i = 0; i < 5; i++)
                                        if (insetState.grid[4][i] == TileType.bug)
                                            adjacentBugs++;
                                }
                                if (x == 1 && y == 2)
                                {
                                    for (int i = 0; i < 5; i++)
                                        if (insetState.grid[i][0] == TileType.bug)
                                            adjacentBugs++;
                                }
                                else if (x == 3 && y == 2)
                                {
                                    for (int i = 0; i < 5; i++)
                                        if (insetState.grid[i][4] == TileType.bug)
                                            adjacentBugs++;
                                }
                            }
                        }

                        switch (grid[y][x])
                        {
                            case TileType.bug:
                                if (adjacentBugs == 1)
                                    newGrid[y][x] = TileType.bug;
                                else
                                    newGrid[y][x] = TileType.space;
                                break;
                            case TileType.space:
                                if (adjacentBugs == 1 || adjacentBugs == 2)
                                    newGrid[y][x] = TileType.bug;
                                else
                                    newGrid[y][x] = TileType.space;
                                break;
                        }
                    }
                    else
                    {
                        newGrid[y][x] = TileType.space;
                    }
                }
            }
            return new GridState(newGrid);
        }

        public void Print()
        {
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    Console.Write((char)grid[y][x]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private long CalculateBiodiversity()
        {
            long biodiversity = 0;
            int powerOf2 = 0;
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    if (grid[y][x] == TileType.bug)
                        biodiversity = biodiversity | ((long)1 << powerOf2);
                    powerOf2 += 1;
                }
            }
            return biodiversity;
        }

        private long CountBugs()
        {
            long bugCount = 0;
            for (int y = 0; y < 5;y++)
            {
                for (int x =0;x<5;x++)
                {
                    if (grid[y][x] == TileType.bug)
                        bugCount++;
                }
            }
            return bugCount;
        }
    }

    public class RecursiveGridState
    {
        public Dictionary<int, GridState> gridLevels;

        public long _bugCount = -1;
        public long bugCount
        {
            get
            {
                if (_bugCount == -1)
                    _bugCount = CountBugs();
                return _bugCount;
            }
        }

        public RecursiveGridState(List<string> inputLines)
        {
            gridLevels = new Dictionary<int, GridState>();
            gridLevels[0] = new GridState(inputLines);
        }

        public RecursiveGridState(Dictionary<int, GridState> gridLevels)
        {
            this.gridLevels = gridLevels;
        }

        public RecursiveGridState Tick()
        {
            int minLevel = int.MaxValue;
            int maxLevel = int.MinValue;
            foreach (var kVP in gridLevels)
            {
                if (kVP.Key < minLevel)
                    minLevel = kVP.Key;
                if (kVP.Key > maxLevel)
                    maxLevel = kVP.Key;
            }

            TileType[][] newMaxLevelGrid = new TileType[5][];
            TileType[][] newMinLevelGrid = new TileType[5][];
            for (int y = 0; y < 5; y++)
            {
                newMaxLevelGrid[y] = new TileType[5];
                newMinLevelGrid[y] = new TileType[5];
                for (int x = 0; x < 5; x++)
                {
                    newMaxLevelGrid[y][x] = TileType.space;
                    newMinLevelGrid[y][x] = TileType.space;
                }
            }

            gridLevels[maxLevel + 1] = new GridState(newMaxLevelGrid);
            gridLevels[minLevel - 1] = new GridState(newMinLevelGrid);

            Dictionary<int, GridState> newGridLevels = new Dictionary<int, GridState>();

            for (int level = minLevel - 1; level <= maxLevel + 1; level++)
            {
                GridState gridState = gridLevels[level];

                if (level == minLevel - 1)
                    newGridLevels[level] = gridState.Tick(true, gridLevels[level + 1], null);
                else if (level == maxLevel + 1)
                    newGridLevels[level] = gridState.Tick(true, null, gridLevels[level - 1]);
                else
                    newGridLevels[level] = gridState.Tick(true, gridLevels[level + 1], gridLevels[level - 1]);
            }
            return new RecursiveGridState(newGridLevels);
        }

        public void Print()
        {
            int minLevel = int.MaxValue;
            int maxLevel = int.MinValue;
            foreach (var kVP in gridLevels)
            {
                if (kVP.Key < minLevel)
                    minLevel = kVP.Key;
                if (kVP.Key > maxLevel)
                    maxLevel = kVP.Key;
            }

            for (int i = minLevel; i <= maxLevel; i++)
            {
                var gridLevel = gridLevels[i];
                Console.WriteLine($"Level {i}");
                gridLevel.Print();
                Console.WriteLine();
            }
            Console.WriteLine("==========================");
        }

        private long CountBugs()
        {
            long bugCount = 0;
            foreach (var kVP in gridLevels)
            {
                bugCount += kVP.Value.bugCount;
            }
            return bugCount;
        }
    }
}
