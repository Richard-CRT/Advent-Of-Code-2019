using AdventOfCodeUtilities;
using IntCodeComputerNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_19___Tractor_Beam
{
    class Program
    {
        static List<int> Inputs = new List<int>();

        static List<int> Outputs = new List<int>();
        static int LastOutput = 0;

        static long InputMethod()
        {
            long input = 1;

            AoCUtilities.DebugWrite("Input: ");

            if (Inputs.Count > 0)
            {
                input = Inputs[0];
                Inputs.RemoveAt(0);
            }
            else
            {
                input = Int64.Parse(Console.ReadLine());
            }

            AoCUtilities.DebugWriteLine("{0}", input);
            //AoCUtilities.DebugReadLine();

            return input;
        }


        static void OutputMethod(long output)
        {
            AoCUtilities.DebugWriteLine("Output: {0}", output);
            //AoCUtilities.DebugWrite("{0}", (char)output);

            LastOutput = (int)output;
            Outputs.Add((int)output);
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

            IntCodeComputer computer = new IntCodeComputer(InputMethod, OutputMethod);

            //P1(computer, memory);
            P2(computer, memory);

            Console.ReadLine();
        }

        static void PrintOutputs(int rowLength)
        {
            for (int row = 0; row < Outputs.Count / rowLength; row++)
            {
                for (int column = 0; column < rowLength; column++)
                {
                    if (Outputs[(row * rowLength) + column] == 1)
                        Console.Write("#");
                    else
                        Console.Write(".");
                }
                Console.WriteLine();
            }
        }

        static void TryPosition(IntCodeComputer computer, long[] memory, int x, int y)
        {
            Inputs.Add(x);
            Inputs.Add(y);
            computer.Flash(memory);
            computer.Run();
        }

        static void P1(IntCodeComputer computer, long[] memory)
        {
            for (int y = 0; y < 50; y++)
                for (int x = 0; x < 50; x++)
                    TryPosition(computer, memory, x, y);

            PrintOutputs(50);

            int numberOfBeamEffectedPositions = 0;
            foreach (int output in Outputs)
            {
                if (output == 1)
                    numberOfBeamEffectedPositions++;
            }

            Console.WriteLine(numberOfBeamEffectedPositions);
            Console.ReadLine();
        }

        static void P2(IntCodeComputer computer, long[] memory)
        {
            Outputs = new List<int>();

            int x = 100;
            int y = -1;
            bool foundSpace = false;

            while (!foundSpace)
            {
                while (LastOutput != 1)
                {
                    y++;
                    TryPosition(computer, memory, x, y);
                }
                TryPosition(computer, memory, x - 99, y + 99);
                if (LastOutput == 1)
                {
                    foundSpace = true;
                }
                else
                {
                    x++;
                    TryPosition(computer, memory, x, y);
                }
            }

            int result = ((x - 99) * 10000) + y;
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
