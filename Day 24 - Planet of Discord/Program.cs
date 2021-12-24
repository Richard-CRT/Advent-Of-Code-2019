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
            state.Print();

            List<long> knownBiodiversities = new List<long>() {  };

            while(!knownBiodiversities.Contains(state.biodiversity))
            {
                knownBiodiversities.Add(state.biodiversity);

                state = state.Tick();
                state.Print();
            }

            Console.WriteLine(state.biodiversity);
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

        public GridState Tick()
        {
            TileType[][] newGrid = new TileType[5][];
            for (int y = 0; y < 5; y++)
            {
                newGrid[y] = new TileType[5];
                for (int x = 0; x < 5; x++)
                {
                    int adjacentBugs = 0;
                    if (y > 0 && grid[y - 1][x] == TileType.bug)
                        adjacentBugs++;
                    if (y < grid.Length - 1 && grid[y + 1][x] == TileType.bug)
                        adjacentBugs++;
                    if (x > 0 && grid[y][x - 1] == TileType.bug)
                        adjacentBugs++;
                    if (x < grid[0].Length - 1 && grid[y][x + 1] == TileType.bug)
                        adjacentBugs++;

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

    }

    public class RecursiveGridState
    {
        public Dictionary<int,GridState> gridLevels;

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
            /*
            TileType[][] newGrid = new TileType[5][];
            for (int y = 0; y < 5; y++)
            {
                newGrid[y] = new TileType[5];
                for (int x = 0; x < 5; x++)
                {
                    int adjacentBugs = 0;
                    if (y > 0 && grid[y - 1][x] == TileType.bug)
                        adjacentBugs++;
                    if (y < grid.Length - 1 && grid[y + 1][x] == TileType.bug)
                        adjacentBugs++;
                    if (x > 0 && grid[y][x - 1] == TileType.bug)
                        adjacentBugs++;
                    if (x < grid[0].Length - 1 && grid[y][x + 1] == TileType.bug)
                        adjacentBugs++;

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
            }
            return new RecursiveGridState(newGrid);
            */
            return null;
        }

        public void Print()
        {
            foreach (var levelKV in gridLevels)
            {
                Console.WriteLine($"Level {levelKV.Key}");
                levelKV.Value.Print();
                Console.WriteLine();
            }
        }
    }
}
