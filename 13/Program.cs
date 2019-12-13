using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;
using IntCodeComputerNS;

namespace _13
{
    enum Tile { Empty = 0, Wall = 1, Block = 2, HorizontalPaddle = 3, Ball = 4 };

    class Program
    {
        static long InputMethod()
        {
            int ballX = -1;
            int paddleX = -1;
            for (int y = 0; y < GridHeight; y++)
            {
                for (int x = 0; x < GridWidth; x++)
                {
                    if (Grid[y][x] == Tile.Ball)
                    {
                        ballX = x;
                    }
                    else if (Grid[y][x] == Tile.HorizontalPaddle)
                    {
                        paddleX = x;
                    }

                    if (ballX != -1 && paddleX != -1)
                    {
                        break;
                    }
                }

                if (ballX != -1 && paddleX != -1)
                {
                    break;
                }
            }

            long input = 0;

            if (ballX > paddleX)
            {
                input = 1;
            }
            else if (ballX < paddleX)
            {
                input = -1;
            }

            //AoCUtilities.DebugWrite("Input: {0}", input);
            //long input = Int64.Parse(Console.ReadLine());

            return input;
        }

        static List<int> Outputs = new List<int>();

        static void OutputMethod(long output)
        {
            //AoCUtilities.DebugWriteLine("Output: {0}", output);
            Outputs.Add((int)output);

            if (Outputs.Count() == 3)
            {
                int x = Outputs[0];
                int y = Outputs[1];

                if (x == -1 && y == 0)
                {
                    Score = Outputs[2];
                }
                else
                {
                    Tile tileType = (Tile)Outputs[2];
                    Grid[y][x] = tileType;
                }
                Outputs.Clear();

                PrintGrid();
            }
        }

        const int GridWidth = 37;
        const int GridHeight = 22;
        static List<List<Tile>> Grid = new List<List<Tile>>();
        static int Score = 0;

        static void PrintGrid()
        {

#if DEBUG
            string print = "";
            print += "Score: " + Score + "\n";

            for (int y = 0; y < GridHeight; y++)
            {
                Grid.Add(new List<Tile>());
                for (int x = 0; x < GridWidth; x++)
                {
                    switch (Grid[y][x])
                    {
                        case Tile.Empty:
                            print += "  ";
                            break;
                        case Tile.Wall:
                            print += "██";
                            break;
                        case Tile.Block:
                            print += "▒▒";
                            break;
                        case Tile.HorizontalPaddle:
                            print += "▀▀";
                            break;
                        case Tile.Ball:
                            print += "▓▓";
                            break;
                    }
                }
                print += "\n";
            }
            
            AoCUtilities.DebugClear();
            AoCUtilities.DebugWriteLine(print);
#endif

            //AoCUtilities.DebugReadLine();
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
            memory[0] = 2;

            for (int y = 0; y < GridHeight; y++)
            {
                Grid.Add(new List<Tile>());
                for (int x = 0; x < GridWidth; x++)
                {
                    Grid[y].Add(Tile.Empty);
                }
            }

            IntCodeComputer computer = new IntCodeComputer(InputMethod, OutputMethod);
            computer.Flash(memory);
            computer.Run();

            int howManyBlockTiles = 0;
            for (int y = 0; y < GridHeight; y++)
            {
                Grid.Add(new List<Tile>());
                for (int x = 0; x < GridWidth; x++)
                {
                    if (Grid[y][x] == Tile.Block)
                    {
                        howManyBlockTiles++;
                    }
                }
            }

            Console.WriteLine("When the game exits there are {0} block tiles", howManyBlockTiles);
            Console.WriteLine("When the game exits the score is {0}", Score);
            Console.ReadLine();
        }
    }
}
