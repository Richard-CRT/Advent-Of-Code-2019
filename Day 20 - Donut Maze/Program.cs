using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_20___Donut_Maze
{
    class Program
    {
        static void print(int width, int height, Tile[][] map)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (map[y][x].explored)
                        AoCUtilities.DebugWrite("*");
                    else
                        AoCUtilities.DebugWrite("{0}", (char)map[y][x].type);
                }
                AoCUtilities.DebugWriteLine();
            }
            AoCUtilities.DebugWriteLine();
        }

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();

            Dictionary<string, List<Tile>> portals = new Dictionary<string, List<Tile>>();

            int width = inputList[0].Length - 4;
            int height = inputList.Count - 4;

            int depth = 200;

            Tile[][][] map = new Tile[depth][][];
            for (int z = 0; z < depth; z++)
            {
                map[z] = new Tile[height][];
                for (int y = 0; y < height; y++)
                {
                    map[z][y] = new Tile[width];
                }
            }

            Tile startTile = null;
            Tile endTile = null;

            for (int line_i = 2; line_i < inputList.Count - 2; line_i++)
            {
                string line = inputList[line_i];
                for (int char_i = 2; char_i < line.Length - 2; char_i++)
                {
                    char character = line[char_i];

                    int y = line_i - 2;
                    int x = char_i - 2;
                    for (int level = 0; level < depth; level++)
                    {
                        Tile newTile;
                        if (character == (char)TileType.wall)
                            newTile = new Tile(level, x, y, TileType.wall);
                        else if (character == (char)TileType.open)
                            newTile = new Tile(level, x, y, TileType.open);
                        else
                            newTile = new Tile(level, x, y, TileType.unreachable);
                        map[level][y][x] = newTile;
                    }
                }
            }

            TileType type = TileType.wall;
            int doughnutThickness = 0;
            while (type != TileType.unreachable)
            {
                type = map[0][doughnutThickness][doughnutThickness].type;
                doughnutThickness++;
            }
            doughnutThickness--;


            for (int line_i = 2; line_i < inputList.Count - 2; line_i++)
            {
                string line = inputList[line_i];
                for (int char_i = 2; char_i < line.Length - 2; char_i++)
                {
                    char character = line[char_i];

                    int y = line_i - 2;
                    int x = char_i - 2;
                    bool outer = false;
                    Tile tile = map[0][y][x];

                    string label = "  ";
                    if (y == 0)
                    {
                        label = $"{inputList[line_i - 2][char_i]}{inputList[line_i - 1][char_i]}";
                        outer = true;
                    }
                    else if (y == height - 1)
                    {
                        label = $"{inputList[line_i + 1][char_i]}{inputList[line_i + 2][char_i]}";
                        outer = true;
                    }
                    else if (x == 0)
                    {
                        label = $"{inputList[line_i][char_i - 2]}{inputList[line_i][char_i - 1]}";
                        outer = true;
                    }
                    else if (x == width - 1)
                    {
                        label = $"{inputList[line_i][char_i + 1]}{inputList[line_i][char_i + 2]}";
                        outer = true;
                    }
                    else if (y == doughnutThickness - 1)
                        label = $"{inputList[line_i + 1][char_i]}{inputList[line_i + 2][char_i]}";
                    else if (y == height - 1 - doughnutThickness + 1)
                        label = $"{inputList[line_i - 2][char_i]}{inputList[line_i - 1][char_i]}";
                    else if (x == doughnutThickness - 1)
                        label = $"{inputList[line_i][char_i + 1]}{inputList[line_i][char_i + 2]}";
                    else if (x == width - 1 - doughnutThickness + 1)
                        label = $"{inputList[line_i][char_i - 2]}{inputList[line_i][char_i - 1]}";
                    if (label[0] >= 0x41 && label[0] <= 0x5A && label[1] >= 0x41 && label[1] <= 0x5A)
                    {
                        if (label == "AA")
                            startTile = tile;
                        else if (label == "ZZ")
                            endTile = tile;
                        else
                        {
                            if (portals.ContainsKey(label))
                            {
                                Tile outerPortalTile;
                                Tile innerPortalTile;
                                if (outer)
                                {
                                    outerPortalTile = tile;
                                    innerPortalTile = portals[label][0];
                                }
                                else
                                {
                                    outerPortalTile = portals[label][0];
                                    innerPortalTile = tile;
                                }

                                for (int z = 0; z < depth - 1; z++)
                                {
                                    map[z][innerPortalTile.y][innerPortalTile.x].portalTo = map[z + 1][outerPortalTile.y][outerPortalTile.x];
                                    if (z > 0)
                                        map[z][outerPortalTile.y][outerPortalTile.x].portalTo = map[z - 1][innerPortalTile.y][innerPortalTile.x];
                                }
                                portals[label].Add(tile);
                            }
                            else
                                portals[label] = new List<Tile> { tile };
                        }
                    }
                }
            }

            //print(width, height, map[0]);

            bool complete = false;

            List<Tile> tilesToExplore = new List<Tile> { startTile };
            int steps = -1;
            while (!complete)
            {
                List<Tile> newTilesToExplore = new List<Tile>();
                foreach (Tile tileToExplore in tilesToExplore)
                {
                    int level = tileToExplore.level;
                    int x = tileToExplore.x;
                    int y = tileToExplore.y;

                    if (x > 0 && map[level][y][x - 1].type == TileType.open && !map[level][y][x - 1].explored)
                        newTilesToExplore.Add(map[level][y][x - 1]);
                    if (x < width - 1 && map[level][y][x + 1].type == TileType.open && !map[level][y][x + 1].explored)
                        newTilesToExplore.Add(map[level][y][x + 1]);
                    if (y > 0 && map[level][y - 1][x].type == TileType.open && !map[level][y - 1][x].explored)
                        newTilesToExplore.Add(map[level][y - 1][x]);
                    if (y < height - 1 && map[level][y + 1][x].type == TileType.open && !map[level][y + 1][x].explored)
                        newTilesToExplore.Add(map[level][y + 1][x]);
                    if (tileToExplore.portalTo != null && !tileToExplore.portalTo.explored)
                        newTilesToExplore.Add(tileToExplore.portalTo);

                    tileToExplore.explored = true;

                    if (tileToExplore == endTile)
                        complete = true;
                }
                tilesToExplore = newTilesToExplore;
                steps++;
                //print(width, height,map[0]);
                AoCUtilities.DebugReadLine();

            }

            Console.WriteLine(steps);
            Console.ReadLine();
        }
    }

    public enum TileType { open = '.', wall = '#', unreachable = ' ' };

    class Tile
    {
        public int level;
        public int x;
        public int y;
        public TileType type;
        public Tile portalTo = null;
        public bool explored = false;

        public Tile(int level, int x, int y, TileType type)
        {
            this.level = level;
            this.x = x;
            this.y = y;
            this.type = type;
        }

        public override string ToString()
        {
            return $"({x},{y}) {type}";
        }
    }
}
