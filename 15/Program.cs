using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCodeUtilities;
using IntCodeComputerNS;

namespace _13
{
    enum TileType { Unknown, Empty, Wall, OxygenSystem };
    enum Direction { North, East, South, West };

    class Tile
    {
        public TileType Type;
        public bool Explored = false;
        public bool Oxygen = false;

        public Tile(TileType type)
        {
            this.Type = type;

            if (type == TileType.OxygenSystem)
            {
                Oxygen = true;
            }
        }
    }

    class Program
    {
        static long InputMethod()
        {
            long input = 1;

            //input = Int64.Parse(Console.ReadLine());

            if (!Grid[LocationY - 1][LocationX].Explored)
                input = 1;
            else if (!Grid[LocationY][LocationX + 1].Explored)
                input = 4;
            else if (!Grid[LocationY + 1][LocationX].Explored)
                input = 2;
            else if (!Grid[LocationY][LocationX - 1].Explored)
                input = 3;
            else
            {
                if (DirectionStack.Count > 0)
                {
                    switch (DirectionStack[DirectionStack.Count - 1])
                    {
                        case Direction.North:
                            input = 2;
                            break;
                        case Direction.East:
                            input = 3;
                            break;
                        case Direction.South:
                            input = 1;
                            break;
                        case Direction.West:
                            input = 4;
                            break;
                    }
                }
                else
                {
                    PrintGrid();

                    Console.WriteLine("Maze Explored!");
                    //Console.ReadLine();

                    bool mapOxygenated = false;
                    int minutes = 0;
                    while (!mapOxygenated)
                    {
                        List<Tile> tilesToOxygenate = new List<Tile>();

                        mapOxygenated = true;
                        for (int y = 0; y < GridHeight; y++)
                        {
                            for (int x = 0; x < GridWidth; x++)
                            {
                                if (Grid[y][x].Type == TileType.Empty || Grid[y][x].Type == TileType.OxygenSystem)
                                {
                                    if (!Grid[y][x].Oxygen)
                                    {
                                        mapOxygenated = false;
                                    }
                                    else
                                    {
                                        tilesToOxygenate.Add(Grid[y - 1][x]);
                                        tilesToOxygenate.Add(Grid[y][x + 1]);
                                        tilesToOxygenate.Add(Grid[y + 1][x]);
                                        tilesToOxygenate.Add(Grid[y][x - 1]);
                                    }
                                }
                            }
                        }

                        foreach (Tile tile in tilesToOxygenate)
                        {
                            tile.Oxygen = true;
                        }

                        minutes++;

                        PrintGrid();
                        AoCUtilities.DebugReadLine();
                    }

                    minutes--;
                    Console.WriteLine("Took {0} minutes to fill the maze", minutes);
                    Console.ReadLine();
                }
            }

            //AoCUtilities.DebugWriteLine("Input: {0}", input);

            switch (input)
            {
                case 1:
                    // North
                    RequestedLocationX = LocationX;
                    RequestedLocationY = LocationY - 1;
                    RequestedDirection = Direction.North;
                    break;
                case 2:
                    // South
                    RequestedLocationX = LocationX;
                    RequestedLocationY = LocationY + 1;
                    RequestedDirection = Direction.South;
                    break;
                case 3:
                    // West
                    RequestedLocationX = LocationX - 1;
                    RequestedLocationY = LocationY;
                    RequestedDirection = Direction.West;
                    break;
                case 4:
                    // East
                    RequestedLocationX = LocationX + 1;
                    RequestedLocationY = LocationY;
                    RequestedDirection = Direction.East;
                    break;
            }

            return input;
        }

        static List<int> Outputs = new List<int>();

        static void OutputMethod(long output)
        {
            //AoCUtilities.DebugWriteLine("Output: {0}", output);

            switch (output)
            {
                case 0:
                    // wall
                    Grid[RequestedLocationY][RequestedLocationX] = new Tile(TileType.Wall);
                    break;
                case 1:
                    Grid[RequestedLocationY][RequestedLocationX] = new Tile(TileType.Empty);
                    LocationX = RequestedLocationX;
                    LocationY = RequestedLocationY;
                    break;
                case 2:
                    Grid[RequestedLocationY][RequestedLocationX] = new Tile(TileType.OxygenSystem);
                    LocationX = RequestedLocationX;
                    LocationY = RequestedLocationY;

                    Console.WriteLine("Oxygen System is {0} moves from origin", DirectionStack.Count + 1);
                    //Console.ReadLine();
                    break;
            }

            if (output == 1 || output == 2)
            {
                if (DirectionStack.Count != 0)
                {
                    Direction lastDirection = DirectionStack[DirectionStack.Count - 1];
                    if (
                        (lastDirection == Direction.North && RequestedDirection == Direction.South) ||
                        (lastDirection == Direction.South && RequestedDirection == Direction.North) ||
                        (lastDirection == Direction.East && RequestedDirection == Direction.West) ||
                        (lastDirection == Direction.West && RequestedDirection == Direction.East)
                        )
                    {
                        DirectionStack.RemoveAt(DirectionStack.Count - 1);
                    }
                    else
                    {
                        DirectionStack.Add(RequestedDirection);
                    }
                }
                else
                {
                    DirectionStack.Add(RequestedDirection);
                }
            }

            Grid[RequestedLocationY][RequestedLocationX].Explored = true;

            //PrintGrid();
        }

        static List<Direction> DirectionStack = new List<Direction>();

        const int GridWidth = 50;
        const int GridHeight = 50;
        static int LocationX = GridWidth / 2;
        static int LocationY = GridHeight / 2;
        static List<List<Tile>> Grid = new List<List<Tile>>();
        static Direction RequestedDirection;
        static int RequestedLocationX;
        static int RequestedLocationY;

        static void PrintGrid()
        {
#if DEBUG
            string print = "";

            for (int y = 0; y < GridHeight; y++)
            {
                for (int x = 0; x < GridWidth; x++)
                {
                    switch (Grid[y][x].Type)
                    {
                        case TileType.Unknown:
                            print += "▒▒";
                            break;
                        case TileType.Empty:
                            if (Grid[y][x].Oxygen)
                                print += "▓▓";
                            else
                                print += "  ";
                            break;
                        case TileType.Wall:
                            print += "██";
                            break;
                        case TileType.OxygenSystem:
                            print += "[]";
                            break;
                    }
                }
                print += "\n";
            }

            AoCUtilities.DebugClear();
            AoCUtilities.DebugWriteLine(print);
            //AoCUtilities.DebugReadLine();
#endif
        }

        static void Main(string[] args)
        {

            List<string> inputList = AoCUtilities.GetInput();
            string program = inputList[0];
            string[] data = program.Split(',');
            long[] memory = new long[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                memory[i] = Int64.Parse(data[i]);
            }

            for (int y = 0; y < GridHeight; y++)
            {
                Grid.Add(new List<Tile>());
                for (int x = 0; x < GridWidth; x++)
                {
                    Grid[y].Add(new Tile(TileType.Unknown));
                }
            }

            Grid[LocationY][LocationX] = new Tile(TileType.Empty);
            Grid[LocationY][LocationX].Explored = true;

            IntCodeComputer computer = new IntCodeComputer(InputMethod, OutputMethod);
            computer.Flash(memory);
            computer.Run();

            Console.ReadLine();
        }
    }
}
