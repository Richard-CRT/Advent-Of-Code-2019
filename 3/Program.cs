using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _3
{
    enum Direction { Up, Right, Down, Left };
    class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();
            List<Tuple<Direction, int>> Wire1 = PathStringToList(inputList[0]);
            List<Tuple<Direction, int>> Wire2 = PathStringToList(inputList[1]);

            List<Tuple<int, int, int>> Wire1Coordinates = GetCoordinatesList(Wire1);
            List<Tuple<int, int, int>> Wire2Coordinates = GetCoordinatesList(Wire2);

            List<Tuple<int, int, int>> CrossedCoords = new List<Tuple<int, int, int>>();

            for (int i = 0; i < Wire1Coordinates.Count; i++)
            {
                Tuple<int, int, int> coord1 = Wire1Coordinates[i];

                for (int j = 0; j < Wire2Coordinates.Count; j++)
                {
                    Tuple<int, int, int> coord2 = Wire2Coordinates[j];

                    if (coord1.Item1 == coord2.Item1 && coord1.Item2 == coord2.Item2)
                    {
                        CrossedCoords.Add(new Tuple<int, int, int>(coord1.Item1, coord1.Item2, coord1.Item3 + coord2.Item3));
                    }
                }
            }

            int lowestCount = Int32.MaxValue;

            foreach (Tuple<int, int, int> crossedCoord in CrossedCoords)
            {
                //int distance = Math.Abs(crossedCoord.Item1) + Math.Abs(crossedCoord.Item2);
                if (crossedCoord.Item3 < lowestCount)
                {
                    lowestCount = crossedCoord.Item3;
                }
            }

            Console.WriteLine(lowestCount);
            Console.ReadLine();
        }

        static List<Tuple<int, int, int>> GetCoordinatesList(List<Tuple<Direction, int>> WirePath)
        {
            List<Tuple<int, int, int>> coords = new List<Tuple<int, int, int>>();
            int x = 0;
            int y = 0;
            int steps = 0;
            foreach (Tuple<Direction, int> wirePart in WirePath)
            {
                switch (wirePart.Item1)
                {
                    case Direction.Up:
                        {
                            for (int i = 0; i < wirePart.Item2; i++)
                            {
                                y++;
                                steps++;
                                coords.Add(new Tuple<int, int, int>(x, y, steps));
                            }
                        }
                        break;
                    case Direction.Right:
                        {
                            for (int i = 0; i < wirePart.Item2; i++)
                            {
                                x++;
                                steps++;
                                coords.Add(new Tuple<int, int, int>(x, y, steps));
                            }
                        }
                        break;
                    case Direction.Down:
                        {
                            for (int i = 0; i < wirePart.Item2; i++)
                            {
                                y--;
                                steps++;
                                coords.Add(new Tuple<int, int, int>(x, y, steps));
                            }
                        }
                        break;
                    case Direction.Left:
                        {
                            for (int i = 0; i < wirePart.Item2; i++)
                            {
                                x--;
                                steps++;
                                coords.Add(new Tuple<int, int, int>(x, y, steps));
                            }
                        }
                        break;
                }
            }

            return coords;
        }

        static List<Tuple<Direction, int>> PathStringToList(string path)
        {
            string[] pathSplit = path.Split(',');

            List<Tuple<Direction, int>> wirePath = new List<Tuple<Direction, int>>();

            foreach (string pathPart in pathSplit)
            {
                Direction dir = Direction.Up;
                int distance;

                switch (pathPart[0])
                {
                    case 'U':
                        {
                            dir = Direction.Up;
                        }
                        break;
                    case 'R':
                        {
                            dir = Direction.Right;
                        }
                        break;
                    case 'D':
                        {
                            dir = Direction.Down;
                        }
                        break;
                    case 'L':
                        {
                            dir = Direction.Left;
                        }
                        break;
                }
                distance = Int32.Parse(pathPart.Substring(1, pathPart.Length - 1));

                wirePath.Add(new Tuple<Direction, int>(dir, distance));
            }

            return wirePath;
        }
    }
}
