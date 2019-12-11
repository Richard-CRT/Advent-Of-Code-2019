using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;
using IntCodeComputerNS;

namespace _11
{
    enum Direction { Up = 0, Right = 1, Down = 2, Left = 3 };
    enum Colour { Black = 0, White = 1 };

    class GridCell
    {
        public Colour Colour;
        private int numTimesPainted = 0;
        public int NumTimesPainted
        {
            get
            {
                return numTimesPainted;
            }
        }

        public GridCell(Colour _colour)
        {
            this.Colour = _colour;
        }

        public void Paint(Colour _colour)
        {
            this.Colour = _colour;
            this.numTimesPainted++;
        }
    }

    class Program
    {
        static long InputMethod()
        {
            long input = 0;

            input = (long)Grid[LocationY][LocationX].Colour;
            AoCUtilities.DebugWrite("Input: ");
            AoCUtilities.DebugWriteLine("{0}", input);
            return input;
        }

        static void OutputMethod(long output)
        {
            AoCUtilities.DebugWriteLine("Output: {0}", output);
            Outputs.Add(output);
            if (Outputs.Count() == 2)
            {
                Grid[LocationY][LocationX].Paint((Colour)Outputs[0]);

                int newDirectionInt = (int)CurrentDirection;
                if (Outputs[1] == 0)
                {
                    newDirectionInt = (int)CurrentDirection - 1;

                    if (newDirectionInt == -1)
                    {
                        newDirectionInt = 3;
                    }
                }
                else
                {
                    newDirectionInt = ((int)CurrentDirection + 1) % 4;
                }

                CurrentDirection = (Direction)newDirectionInt;

                Outputs.Clear();
                switch (CurrentDirection)
                {
                    case Direction.Up:
                        LocationY--;
                        break;
                    case Direction.Right:
                        LocationX++;
                        break;
                    case Direction.Down:
                        LocationY++;
                        break;
                    case Direction.Left:
                        LocationX--;
                        break;
                }

#if DEBUG
                PrintGrid();
#endif
            }
        }

        static List<long> Outputs = new List<long>();

        static List<List<GridCell>> Grid = new List<List<GridCell>>();

        const int GridSize = 100;
        static Direction CurrentDirection = Direction.Up;
        static int LocationX = GridSize / 2;
        static int LocationY = GridSize / 2;

        static void PrintGrid()
        {
            Console.Clear();
            for (int y = 0; y < GridSize; y++)
            {
                for (int x = 0; x < GridSize; x++)
                {
                    if (LocationX == x && LocationY == y)
                    {
                        switch (CurrentDirection)
                        {
                            case Direction.Up:
                                Console.Write("^");
                                break;
                            case Direction.Right:
                                Console.Write(">");
                                break;
                            case Direction.Down:
                                Console.Write("v");
                                break;
                            case Direction.Left:
                                Console.Write("<");
                                break;
                        }
                    }
                    else
                    {
                        switch (Grid[y][x].Colour)
                        {
                            case Colour.Black:
                                Console.Write(".");
                                break;
                            case Colour.White:
                                Console.Write("█");
                                break;
                        }
                    }
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }

        static void Main(string[] args)
        {

            for (int y = 0; y < GridSize; y++)
            {
                Grid.Add(new List<GridCell>());
                for (int x = 0; x < GridSize; x++)
                {
                    Grid[y].Add(new GridCell(Colour.Black));
                }
            }

            Grid[LocationY][LocationX].Colour = Colour.White;

            //PrintGrid();

            List<string> inputList = AoCUtilities.GetInput();
            string program = inputList[0];
            string[] data = program.Split(',');
            long[] memory = new long[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                memory[i] = Int64.Parse(data[i]);
            }

            IntCodeComputer computer = new IntCodeComputer(InputMethod, OutputMethod);
            computer.Flash(memory);
            computer.Run();

            int numPanelsPaintedAtLeastOnce = 0;

            for (int y = 0; y < GridSize; y++)
            {
                for (int x = 0; x < GridSize; x++)
                {
                    if (Grid[y][x].NumTimesPainted > 0)
                    {
                        //Console.WriteLine("{0} - {1},{2}", Grid[y][x].NumTimesPainted, x, y);
                        //Console.ReadLine();
                        numPanelsPaintedAtLeastOnce++;
                    }
                }
            }

            PrintGrid();

            Console.WriteLine("Number of panels painted at least once: {0}", numPanelsPaintedAtLeastOnce);
            Console.ReadLine();
        }
    }
}
