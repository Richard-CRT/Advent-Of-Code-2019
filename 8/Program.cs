using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _8
{


    class Program
    {
        static void Main(string[] args)
        {
            const int PictureWidth = 25;
            const int PictureHeight = 6;
            const int DigitsInLayer = PictureWidth * PictureHeight;

            List<string> inputList = AoCUtilities.GetInput();
            string pictureData = inputList[0];

            int layerCount = pictureData.Length / DigitsInLayer;

            List<List<int>> layers = new List<List<int>>();

            int fewestZeros = Int32.MaxValue;
            int layerNumWithFewestZeros = 0;

            for (int layerNum = 0; layerNum < layerCount; layerNum++)
            {
                List<int> layer = new List<int>();
                int zeroCount = 0;
                for (int digitNum = 0; digitNum < DigitsInLayer; digitNum++)
                {
                    int totalIndex = (layerNum * DigitsInLayer) + digitNum;
                    int digit = Int32.Parse(pictureData[totalIndex].ToString());
                    layer.Add(digit);

                    if (digit == 0)
                        zeroCount++;
                }
                layers.Add(layer);

                if (zeroCount < fewestZeros)
                {
                    fewestZeros = zeroCount;
                    layerNumWithFewestZeros = layerNum;
                }
            }

            AoCUtilities.DebugWriteLine("Layer with fewest zeros is: {0}", layerNumWithFewestZeros);

            int oneCount = 0;
            int twoCount = 0;
            for (int i = 0; i < DigitsInLayer; i++)
            {
                int digit = layers[layerNumWithFewestZeros][i];

                if (digit == 1)
                    oneCount++;
                else if (digit == 2)
                    twoCount++;
            }

            int onesByTwos = oneCount * twoCount;
            Console.WriteLine("Number of Ones multiplied by number of Twos on layer with fewest Zeros is: {0}", onesByTwos);
            Console.ReadLine();

            List<int> finalImage = new List<int>();
            for (int pixelNum = 0; pixelNum < DigitsInLayer; pixelNum ++)
            {
                int currentPixel = 2;
                for (int layerNum = 0; layerNum < layerCount; layerNum ++)
                {
                    switch (layers[layerNum][pixelNum])
                    {
                        case 2:
                            continue;
                        case 1:
                        case 0:
                            currentPixel = layers[layerNum][pixelNum];
                            break;
                    }
                    if (currentPixel != 2)
                    {
                        break;
                    }
                }
                finalImage.Add(currentPixel);
            }

            Console.Clear();

            for (int y = 0; y < PictureHeight; y++)
            {
                for (int x = 0; x < PictureWidth; x++)
                {
                    switch (finalImage[(y*PictureWidth)+x])
                    {
                        case 1:
                            Console.Write("█");
                            break;
                        case 0:
                            Console.Write(" ");
                            break;
                        default:
                            throw new ArgumentException();
                    }
                }
                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}
