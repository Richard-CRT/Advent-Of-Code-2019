using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _2
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();
            string program = inputList[0];
            string[] dataString = program.Split(',');
            int[] data = new int[dataString.Length];
            for (int i = 0; i < dataString.Length; i++)
            {
                data[i] = Int32.Parse(dataString[i]);
            }

            int noun = -1;
            int verb = 0;

            const int maxNounVerb = 99;

            int[] dataWorking = new int[data.Length];
            dataWorking[0] = 0;

            while (dataWorking[0] != 19690720)
            {
                if (noun == maxNounVerb)
                {
                    noun = 0;
                    if (verb == maxNounVerb)
                    {
                        break;
                    }
                    else
                    {
                        verb++;
                    }
                }
                else
                {
                    noun++;
                }

                for (int i = 0; i < data.Length; i++)
                {
                    dataWorking[i] = data[i];
                }

                dataWorking[1] = noun;
                dataWorking[2] = verb;

                int opcode = 0;
                int instructionPointer = 0;
                while (opcode != 99)
                {
                    opcode = dataWorking[instructionPointer];
                    int input1Pos;
                    int input2Pos;
                    int outputPos;
                    switch (opcode)
                    {
                        case 1: // Add
                            {
                                input1Pos = dataWorking[instructionPointer + 1];
                                input2Pos = dataWorking[instructionPointer + 2];
                                outputPos = dataWorking[instructionPointer + 3];

                                dataWorking[outputPos] = dataWorking[input1Pos] + dataWorking[input2Pos];

                                instructionPointer += 4;
                            }
                            break;
                        case 2: // Multiply
                            {
                                input1Pos = dataWorking[instructionPointer + 1];
                                input2Pos = dataWorking[instructionPointer + 2];
                                outputPos = dataWorking[instructionPointer + 3];

                                dataWorking[outputPos] = dataWorking[input1Pos] * dataWorking[input2Pos];

                                instructionPointer += 4;
                            }
                            break;
                        case 99:
                            {
                                instructionPointer += 1;
                            }
                            break;
                    }
                }
                AoCUtilities.DebugWriteLine("{0}, {1}, = {2}", noun, verb, dataWorking[0]);
            }

            if (verb == maxNounVerb && noun == maxNounVerb)
            {
                Console.WriteLine("Answer not found");
            }
            else
            {
                Console.WriteLine("Answer: {0}", (100 * noun) + verb);
            }

            Console.ReadLine();
        }
    }
}
