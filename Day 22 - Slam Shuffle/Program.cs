using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_22___Slam_Shuffle
{
    class Program
    {
        static BigInteger RealMod(BigInteger a, BigInteger b)
        {
            BigInteger temp = a % b;
            while (temp < 0)
                temp += b;
            return temp;
        }

        static BigInteger ModInverse(BigInteger a, BigInteger n)
        {
            return BigInteger.ModPow(a, n - 2, n);
        }

        static BigInteger ModDivide(BigInteger a, BigInteger b, BigInteger n)
        {
            a = RealMod(a, n);
            BigInteger inv = ModInverse(b, n);
            return (inv == -1) ? -1 : RealMod(a * inv, n);
        }

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();

            ////////////////////////////////
            // Part 1
            ////////////////////////////////
            const long cardsP1 = 10007;

            BigInteger positionOf2019 = 2019;

            BigInteger a = 1;
            BigInteger b = 0;
            //for (int instruction_i = inputList.Count - 1; instruction_i >= 0; instruction_i--)
            for (int instruction_i = 0; instruction_i < inputList.Count; instruction_i++)
            {
                string instruction = inputList[instruction_i];
                if (instruction.Length >= 19 && instruction.Substring(0, 19) == "deal into new stack")
                {
                    // return (-pos - 1) % cards
                    a = RealMod(-a, cardsP1);
                    b = RealMod(-b - 1, cardsP1);
                }
                else if (instruction.Length >= 19 && instruction.Substring(0, 19) == "deal with increment")
                {
                    // return (pos * inc) % cards
                    long increment = long.Parse(instruction.Substring(20));

                    a = RealMod(a * increment, cardsP1);
                    b = RealMod(b * increment, cardsP1);
                }
                else if (instruction.Substring(0, 3) == "cut")
                {
                    // return (pos - cut) % cards
                    long cardsToCut = long.Parse(instruction.Substring(4));
                    b = RealMod(b - cardsToCut, cardsP1);
                }
            }

            positionOf2019 = RealMod((a * positionOf2019) + b, cardsP1);

            Console.WriteLine(positionOf2019);

            ////////////////////////////////
            // Part 2
            ////////////////////////////////

            const long cardsP2 = 119315717514047;
            const long numberOfShuffles = 101741582076661;
            const long endPosP2 = 2020;

            a = 1;
            b = 0;
            //for (int instruction_i = inputList.Count - 1; instruction_i >= 0; instruction_i--)
            for (int instruction_i = 0; instruction_i < inputList.Count; instruction_i++)
            {
                string instruction = inputList[instruction_i];
                if (instruction.Length >= 19 && instruction.Substring(0, 19) == "deal into new stack")
                {
                    // return (-pos - 1) % cards
                    a = RealMod(-a, cardsP2);
                    b = RealMod(-b - 1, cardsP2);
                }
                else if (instruction.Length >= 19 && instruction.Substring(0, 19) == "deal with increment")
                {
                    // return (pos * inc) % cards
                    long increment = long.Parse(instruction.Substring(20));

                    a = RealMod(a * increment, cardsP2);
                    b = RealMod(b * increment, cardsP2);
                }
                else if (instruction.Substring(0, 3) == "cut")
                {
                    // return (pos - cut) % cards
                    long cardsToCut = long.Parse(instruction.Substring(4));
                    b = RealMod(b - cardsToCut, cardsP2);
                }
            }

            BigInteger An = BigInteger.ModPow(a, numberOfShuffles, cardsP2);
            BigInteger Bn = RealMod(b * ModDivide(BigInteger.ModPow(a, numberOfShuffles, cardsP2) - 1, a - 1, cardsP2), cardsP2);
            BigInteger startPosP2 = RealMod(ModDivide(RealMod(endPosP2 - Bn, cardsP2), An, cardsP2), cardsP2);
            Console.WriteLine(startPosP2);
            Console.ReadLine();
        }
    }
}
