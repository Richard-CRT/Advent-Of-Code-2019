using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;
using IntCodeComputerNS;

namespace _9
{
    class Program
    {

        static long InputMethod()
        {
            int input = 0;

            if (Inputs.Count == 0)
            {
                Console.Write("Input: ");
                input = Int32.Parse(Console.ReadLine());
            }
            else
            {
                input = Inputs[0];
                Inputs.RemoveAt(0);
                AoCUtilities.DebugWrite("Input: ");
                AoCUtilities.DebugWriteLine("{0}", input);
            }
            return input;
        }

        static void OutputMethod(long output)
        {
            Console.WriteLine("Output: {0}", output);
        }

        static List<int> Inputs = new List<int>();

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
            computer.Flash(memory);
            computer.Run();

            Console.ReadLine();
        }
    }
}
