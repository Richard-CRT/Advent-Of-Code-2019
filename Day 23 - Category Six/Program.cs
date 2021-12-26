using AdventOfCodeUtilities;
using IntCodeComputerNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_23___Category_Six
{
    class Program
    {
        static long InputMethod(int computerId)
        {
            List<long> computerInputs = computers[computerId].Item2;

            long input = 1;

            AoCUtilities.DebugWrite($"Computer {computerId} asks for Input: ");

            if (computerInputs.Count > 0)
            {
                input = computerInputs[0];
                computerInputs.RemoveAt(0);
            }
            else
            {
                input = -1;
                //input = Int64.Parse(Console.ReadLine());
            }

            AoCUtilities.DebugWriteLine("{0}", input);
            //AoCUtilities.DebugReadLine();

            return input;
        }

        static int destinationComputerId = -1;
        static int state = 0;
        static bool firstNAT = true;

        static long natX = 0;
        static long natY = 0;
        static long lastDeliveredNatY = 0;
        static void OutputMethod(int computerId, long output)
        {
            AoCUtilities.DebugWriteLine($"Computer {computerId} Output: {output}");
            //AoCUtilities.DebugWrite("{0}", (char)output);

            if (state == 0)
            {
                destinationComputerId = (int)output;
                state++;
            }
            else if (state == 1)
            {
                if (destinationComputerId != 255)
                    computers[destinationComputerId].Item2.Add(output);
                else
                    natX = output;
                state++;
            }
            else if (state == 2)
            {
                if (destinationComputerId != 255)
                    computers[destinationComputerId].Item2.Add(output);
                else
                {
                    natY = output;
                    if (firstNAT)
                    {
                        firstNAT = false;
                        Console.WriteLine(output);
                    }
                }
                state = 0;
            }
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

            P12(memory);

            Console.ReadLine();
        }

        static Dictionary<int, (IntCodeComputer, List<long>)> computers = new Dictionary<int, (IntCodeComputer, List<long>)>();

        static void P12(long[] memory)
        {
            for (int i = 0; i < 50; i++)
            {
                int j = i;
                InputDelegate inputMethod = new InputDelegate(delegate () { return InputMethod(j); });
                OutputDelegate outputMethod = new OutputDelegate(delegate (long output) { OutputMethod(j, output); });
                IntCodeComputer computer = new IntCodeComputer(inputMethod, outputMethod);
                computers[j] = (computer, new List<long> { j });
                computer.Flash(memory);
            }

            bool firstRun = true;
            bool done = false;
            while (!done)
            {
                for (int i = 0; i < 50; i++)
                {
                    computers[i].Item1.RunUntilInput();
                }

                bool allInputBuffersEmpty = true;
                for (int i = 0; i < 50; i++)
                {
                    if (computers[i].Item2.Count > 0)
                    {
                        allInputBuffersEmpty = false;
                        break;
                    }
                }
                if (allInputBuffersEmpty)
                {
                    if (!firstRun)
                    {
                        computers[0].Item2.Add(natX);
                        computers[0].Item2.Add(natY);

                        if (natY == lastDeliveredNatY)
                        {
                            Console.WriteLine(natY);
                            done = true;
                        }

                        lastDeliveredNatY = natY;
                    }
                    firstRun = false;
                }
            }
        }
    }
}
