using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _4
{
    class Program
    {
        static void Part1()
        {
            List<string> inputList = AoCUtilities.GetInput();
            string[] inputSplit = inputList[0].Split('-');

            int startInt = Int32.Parse(inputSplit[0]);
            int endInt = Int32.Parse(inputSplit[1]);

            List<int> possiblePasswords = new List<int>();

            for (int i = startInt; i < endInt; i++)
            {
                string stringi = i.ToString();

                bool doubleDigit = false;
                bool increasing = true;

                int lastDigit = -1;
                foreach (char character in stringi)
                {
                    int digit = Int32.Parse(character.ToString());
                    if (digit > lastDigit)
                    {
                        if (digit == lastDigit)
                        {
                            doubleDigit = true;
                        }
                    }
                    else
                    {
                        increasing = false;
                        break;
                    }

                    lastDigit = digit;
                }

                if (increasing && doubleDigit)
                {
                    possiblePasswords.Add(i);
                }
            }

            Console.WriteLine("{0}", possiblePasswords.Count);
            Console.ReadLine();
        }
        static void Part2()
        {
            List<string> inputList = AoCUtilities.GetInput();
            string[] inputSplit = inputList[0].Split('-');

            int startInt = Int32.Parse(inputSplit[0]);
            int endInt = Int32.Parse(inputSplit[1]);

            List<int> possiblePasswords = new List<int>();

            for (int i = startInt; i < endInt; i++)
            {
                string stringi = i.ToString();

                bool doubleDigit = false;
                bool increasing = true;

                int lastDigit = -1;

                int howManyCurrentDigit = 0;

                foreach (char character in stringi)
                {
                    int digit = Int32.Parse(character.ToString());

                    if (digit >= lastDigit)
                    {
                        if (digit == lastDigit)
                        {
                            howManyCurrentDigit++;
                        }
                        else
                        {
                            if (howManyCurrentDigit == 2)
                            {
                                doubleDigit = true;
                            }
                            howManyCurrentDigit = 1;
                        }
                    }
                    else
                    {
                        increasing = false;
                        break;
                    }

                    lastDigit = digit;
                }

                if (increasing && (doubleDigit || howManyCurrentDigit == 2))
                {
                    possiblePasswords.Add(i);
                }
            }

            Console.WriteLine("{0}", possiblePasswords.Count);
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            Part1();
            Part2();
        }
    }
}
