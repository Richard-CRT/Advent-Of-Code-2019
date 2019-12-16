using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _16
{
    class Program
    {
        static readonly int[] BasePattern = { 0, 1, 0, -1 };
        static int[] GetPattern(int length, int repeats)
        {
            int[] pattern = new int[length];

            for (int i = 0; i < length; i++)
            {
                pattern[i] = BasePattern[((i + 1) / repeats) % 4];
            }

            return pattern;
        }

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();
            int[] inputSignalShort = inputList[0].ToCharArray().Select(a => a - '0').ToArray();

            int[] inputSignal = new int[inputSignalShort.Length * 10000];

            for (int i = 0; i < 10000 * inputSignalShort.Length; i += inputSignalShort.Length)
            {
                for (int j = 0; j < inputSignalShort.Length; j++)
                {
                    inputSignal[i + j] = inputSignalShort[j];
                }
            }

            string offsetStr = "";
            for (int element = 0; element < 7; element++)
            {
                offsetStr += inputSignalShort[element];
            }
            int offset = Int32.Parse(offsetStr);

            for (int phase = 0; phase < 100; phase++)
            {
                //Console.WriteLine("Phase {0}", phase+1);

                // Offset is in second half, so will always be adding instead of possible 0 or subtracting
                // Therefore work backwards since: output[n] = input[n] + output[n+1]
                // Base case output[len-1] = input[len-1]

                int[] outputSignal = new int[inputSignal.Length];
                for (int element = inputSignal.Length - 1; element >= offset; element--)
                {
                    if (element == inputSignal.Length - 1)
                    {
                        outputSignal[element] = inputSignal[element];
                    }
                    else
                    {
                        outputSignal[element] = Math.Abs((inputSignal[element] + outputSignal[element+1]) % 10);
                    }
                }

                inputSignal = outputSignal;
            }

            Console.Write("The 8 digit message in the final output list is: ");
            //foreach (int outputElement in inputSignal)
            for (int i = offset; i < offset + Math.Min(8, inputSignal.Length); i++)
            {
                int outputElement = inputSignal[i];
                Console.Write(outputElement);
            }
            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
