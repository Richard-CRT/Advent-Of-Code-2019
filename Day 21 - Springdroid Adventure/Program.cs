using AdventOfCodeUtilities;
using IntCodeComputerNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_21___Springdroid_Adventure
{
    class Program
    {
        static List<int> Inputs = new List<int>();

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

        static List<int> Outputs = new List<int>();
        static char LastOutput;

        static void OutputMethod(long output)
        {
            //AoCUtilities.DebugWriteLine("Output: {0}", output);
            if (output <= 255)
                AoCUtilities.DebugWrite("{0}", (char)output);
            else
                Console.WriteLine(output);

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

            IntCodeComputer computer = new IntCodeComputer(InputMethod, OutputMethod);

            P1(computer, memory);
            P2(computer, memory);

            Console.ReadLine();
        }

        static void AddInstruction(string instruction)
        {
            foreach (char c in instruction)
                Inputs.Add(c);
            Inputs.Add('\n');
        }

        static void P1(IntCodeComputer computer, long[] memory)
        {
            AddInstruction("OR B J");
            AddInstruction("AND C J");
            AddInstruction("NOT J J");
            AddInstruction("AND D J"); // D must be ground
            AddInstruction("NOT A T");
            AddInstruction("OR T J");
            AddInstruction("WALK");
            computer.Flash(memory);
            computer.Run();
            Console.ReadLine();
        }

        static void P2(IntCodeComputer computer, long[] memory)
        {
            AddInstruction("OR B J");
            AddInstruction("AND C J");
            AddInstruction("NOT J J");
            AddInstruction("AND D J"); // D must be ground
            AddInstruction("AND H J"); // H must be ground
            AddInstruction("NOT A T");
            AddInstruction("OR T J");

            AddInstruction("RUN");
            computer.Flash(memory);
            computer.Run();
            Console.ReadLine();
        }
    }
}
