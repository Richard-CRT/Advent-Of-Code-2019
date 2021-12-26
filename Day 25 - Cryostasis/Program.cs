using AdventOfCodeUtilities;
using IntCodeComputerNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_25___Cryostasis
{
    class Program
    {
        static List<int> Inputs = new List<int>();

        static long InputMethod()
        {
            long input = 1;

            if (Inputs.Count > 0)
            {
                input = Inputs[0];
                Inputs.RemoveAt(0);
            }
            else
            {
                AoCUtilities.DebugWrite("Input: ");

                string inputStr = Console.ReadLine();
                input = inputStr[0];
                for (int i = 1; i < inputStr.Length;i++)
                {
                    Inputs.Add(inputStr[i]);
                }
                Inputs.Add('\n');
            }

            //AoCUtilities.DebugWriteLine("{0}", input);
            //AoCUtilities.DebugReadLine();

            return input;
        }

        static List<int> Outputs = new List<int>();
        static char LastOutput;

        static void OutputMethod(long output)
        {
            //AoCUtilities.DebugWriteLine("Output: {0}", output);
            AoCUtilities.DebugWrite("{0}", (char)output);

            LastOutput = (char)output;
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

            // south, east, east, east, east, east, south, west, west
            // passed pressure floor by holding "klein bottle, mutex, mug, hypercube"

            IntCodeComputer computer = new IntCodeComputer(InputMethod, OutputMethod);

            P1(computer, memory);

            Console.ReadLine();
        }

        static void P1(IntCodeComputer computer, long[] memory)
        {
            computer.Flash(memory);
            computer.Run();
            Console.ReadLine();
        }
    }
}
