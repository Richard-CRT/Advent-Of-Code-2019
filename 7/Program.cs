using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;
using IntCodeComputerNS;

namespace _7
{
    class Program
    {
        static List<int> AInputs = new List<int>();
        static List<int> BInputs = new List<int>();
        static List<int> CInputs = new List<int>();
        static List<int> DInputs = new List<int>();
        static List<int> EInputs = new List<int>();

        static int AInputMethod()
        {
            int input = 0;

            if (AInputs.Count == 0)
            {
                Console.Write("A Input: ");
                input = Int32.Parse(Console.ReadLine());
            }
            else
            {
                input = AInputs[0];
                AInputs.RemoveAt(0);
                AoCUtilities.DebugWrite("A Input: ");
                AoCUtilities.DebugWriteLine("{0}", input);
            }
            return input;
        }

        static int BInputMethod()
        {
            int input = 0;

            if (BInputs.Count == 0)
            {
                Console.Write("B Input: ");
                input = Int32.Parse(Console.ReadLine());
            }
            else
            {
                input = BInputs[0];
                BInputs.RemoveAt(0);
                AoCUtilities.DebugWrite("B Input: ");
                AoCUtilities.DebugWriteLine("{0}", input);
            }
            return input;
        }

        static int CInputMethod()
        {
            int input = 0;

            if (CInputs.Count == 0)
            {
                Console.Write("C Input: ");
                input = Int32.Parse(Console.ReadLine());
            }
            else
            {
                input = CInputs[0];
                CInputs.RemoveAt(0);
                AoCUtilities.DebugWrite("C Input: ");
                AoCUtilities.DebugWriteLine("{0}", input);
            }
            return input;
        }

        static int DInputMethod()
        {
            int input = 0;

            if (DInputs.Count == 0)
            {
                Console.Write("D Input: ");
                input = Int32.Parse(Console.ReadLine());
            }
            else
            {
                input = DInputs[0];
                DInputs.RemoveAt(0);
                AoCUtilities.DebugWrite("D Input: ");
                AoCUtilities.DebugWriteLine("{0}", input);
            }
            return input;
        }

        static int EInputMethod()
        {
            int input = 0;

            if (EInputs.Count == 0)
            {
                Console.Write("E Input: ");
                input = Int32.Parse(Console.ReadLine());
            }
            else
            {
                input = EInputs[0];
                EInputs.RemoveAt(0);
                AoCUtilities.DebugWrite("E Input: ");
                AoCUtilities.DebugWriteLine("{0}", input);
            }
            return input;
        }

        static void AOutputMethod(int output)
        {
            AoCUtilities.DebugWriteLine("A Output: {0}", output);
            BInputs.Add(output);
        }

        static void BOutputMethod(int output)
        {
            AoCUtilities.DebugWriteLine("B Output: {0}", output);
            CInputs.Add(output);
        }

        static void COutputMethod(int output)
        {
            AoCUtilities.DebugWriteLine("C Output: {0}", output);
            DInputs.Add(output);
        }

        static void DOutputMethod(int output)
        {
            AoCUtilities.DebugWriteLine("D Output: {0}", output);
            EInputs.Add(output);
        }

        static void EOutputMethod(int output)
        {
            AoCUtilities.DebugWriteLine("E Output: {0}", output);
            AInputs.Add(output);
        }

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();
            string program = inputList[0];
            string[] data = program.Split(',');
            int[] memory = new int[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                memory[i] = Int32.Parse(data[i]);
            }

            IntCodeComputer amplifier1 = new IntCodeComputer(AInputMethod, AOutputMethod);
            IntCodeComputer amplifier2 = new IntCodeComputer(BInputMethod, BOutputMethod);
            IntCodeComputer amplifier3 = new IntCodeComputer(CInputMethod, COutputMethod);
            IntCodeComputer amplifier4 = new IntCodeComputer(DInputMethod, DOutputMethod);
            IntCodeComputer amplifier5 = new IntCodeComputer(EInputMethod, EOutputMethod);

            int highestThrusterSignal = 0;

            for (int a = 5; a <= 9; a++)
            {
                for (int b = 5; b <= 9; b++)
                {
                    if (a == b)
                    {
                        continue;
                    }

                    for (int c = 5; c <= 9; c++)
                    {
                        if (c == b || c == a)
                        {
                            continue;
                        }

                        for (int d = 5; d <= 9; d++)
                        {
                            if (d == c || d == b || d == a)
                            {
                                continue;
                            }

                            for (int e = 5; e <= 9; e++)
                            {
                                if (e == d || e == c || e == b || e == a)
                                {
                                    continue;
                                }

                                AInputs.Clear();
                                BInputs.Clear();
                                CInputs.Clear();
                                DInputs.Clear();
                                EInputs.Clear();

                                amplifier1.Flash(memory);
                                amplifier2.Flash(memory);
                                amplifier3.Flash(memory);
                                amplifier4.Flash(memory);
                                amplifier5.Flash(memory);

                                AInputs.Add(a);
                                BInputs.Add(b);
                                CInputs.Add(c);
                                DInputs.Add(d);
                                EInputs.Add(e);

                                // Input Phase
                                amplifier1.RunUntilInput();
                                amplifier2.RunUntilInput();
                                amplifier3.RunUntilInput();
                                amplifier4.RunUntilInput();
                                amplifier5.RunUntilInput();

                                AInputs.Add(0);
                                bool lastAmplifierFinished = false;
                                while (!lastAmplifierFinished)
                                {
                                    amplifier1.RunUntilInput();
                                    amplifier2.RunUntilInput();
                                    amplifier3.RunUntilInput();
                                    amplifier4.RunUntilInput();
                                    lastAmplifierFinished = !amplifier5.RunUntilInput();
                                }

                                int thrusterSignal = AInputs[0];
                                AoCUtilities.DebugWriteLine("Thruster Signal: {0}", thrusterSignal);

                                if (thrusterSignal > highestThrusterSignal)
                                {
                                    highestThrusterSignal = thrusterSignal;
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Maximum Thruster Signal: {0}", highestThrusterSignal);
            Console.ReadLine();

            /*
            Inputs.Add(4);
            Inputs.Add(3);
            Inputs.Add(2);
            Inputs.Add(1);
            Inputs.Add(0);
            */
        }
    }
}
