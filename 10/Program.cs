using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _10
{
    class Asteroid
    {
        public int X;
        public int Y;
        public List<Asteroid> VisibleAsteroids = new List<Asteroid>();

        public Asteroid(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public double AngleFromHereTo(Asteroid otherAsteroid)
        {
            int xDistance = otherAsteroid.X - this.X;
            int yDistance = otherAsteroid.Y - this.Y;
            int absXDistance = Math.Abs(xDistance);
            int absYDistance = Math.Abs(yDistance);

            double quadrantAngle = Math.Atan2(absXDistance, absYDistance);
            //AoCUtilities.DebugWriteLine("Quadrant Angle: {0}", quadrantAngle);
            double angle = 0;
            if (xDistance == 0 && yDistance == 0)
            {
                throw new ArgumentException("Same Asteroid as Trial Asteroid");
            }
            else if (xDistance == 0)
            {
                if (yDistance > 0)
                {
                    angle = Math.PI;
                }
                else if (yDistance < 0)
                {
                    angle = 0;
                }
            }
            else if (yDistance == 0)
            {
                if (xDistance > 0)
                {
                    angle = Math.PI / 2;
                }
                else if (xDistance < 0)
                {
                    angle = (3 * Math.PI) / 2;
                }
            }
            else if (xDistance > 0 && yDistance > 0)
            {
                angle = Math.PI - quadrantAngle;
            }
            else if (xDistance < 0 && yDistance > 0)
            {
                angle = Math.PI + quadrantAngle;
            }
            else if (xDistance > 0 && yDistance < 0)
            {
                angle = quadrantAngle;
            }
            else if (xDistance < 0 && yDistance < 0)
            {
                angle = (2 * Math.PI) - quadrantAngle;
            }
            else
            {
                throw new ArgumentException("No angles match");
            }

            return angle;
        }

        public void CalculateVisibleAsteroids(List<Asteroid> allAsteroids)
        {
            this.VisibleAsteroids.Clear();

            //AoCUtilities.DebugWriteLine("Trial Asteroid");
            List<Tuple<Asteroid, int, double>> otherAsteroids = new List<Tuple<Asteroid, int, double>>();
            foreach (Asteroid asteroid in allAsteroids)
            {
                if (this != asteroid)
                {
                    int xDistance = asteroid.X - this.X;
                    int yDistance = asteroid.Y - this.Y;
                    int absXDistance = Math.Abs(xDistance);
                    int absYDistance = Math.Abs(yDistance);
                    int manhattanDistance = absXDistance + absYDistance;

                    //AoCUtilities.DebugWriteLine("Angle: {0}", angle);

                    double angle = AngleFromHereTo(asteroid);

                    otherAsteroids.Add(new Tuple<Asteroid, int, double>(asteroid, manhattanDistance, angle));
                }
            }

            // compare all asteroids to make sure no gradients are the same
            for (int i = 0; i < otherAsteroids.Count(); i++)
            {
                Tuple<Asteroid, int, double> asteroid1 = otherAsteroids[i];

                Tuple<Asteroid, int, double> closestAsteroidWithEachGradient = asteroid1;
                for (int j = 0; j < otherAsteroids.Count(); j++)
                {
                    if (i != j)
                    {
                        Tuple<Asteroid, int, double> asteroid2 = otherAsteroids[j];

                        if (asteroid1.Item3 == asteroid2.Item3)
                        {
                            if (asteroid2.Item2 < closestAsteroidWithEachGradient.Item2)
                            {
                                closestAsteroidWithEachGradient = asteroid2;
                            }
                        }
                    }
                }

                if (!this.VisibleAsteroids.Contains(closestAsteroidWithEachGradient.Item1))
                    this.VisibleAsteroids.Add(closestAsteroidWithEachGradient.Item1);

            }
        }
    }

    class Program
    {
        static int Height;
        static int Width;
        static List<List<Asteroid>> Map = new List<List<Asteroid>>();
        static Asteroid StationAsteroid = null;

        static void PrintMap()
        {
            Console.WriteLine();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (Map[y][x] != null)
                    {
                        if (StationAsteroid != null && Map[y][x] == StationAsteroid)
                        {
                            Console.Write("■");
                        }
                        else
                        {
                            Console.Write("o");
                        }
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();

            Height = inputList.Count();
            Width = inputList[0].Count();

            List<Asteroid> allAsteroids = new List<Asteroid>();

            for (int y = 0; y < Height; y++)
            {
                List<Asteroid> row = new List<Asteroid>();
                for (int x = 0; x < Width; x++)
                {
                    if (inputList[y][x] == '#')
                    {
                        Asteroid newAsteroid = new Asteroid(x, y);
                        allAsteroids.Add(newAsteroid);
                        row.Add(newAsteroid);
                    }
                    else
                    {
                        row.Add(null);
                    }
                }
                Map.Add(row);
            }

            PrintMap();

            for (int x = 0; x < allAsteroids.Count; x++)
            {
                Asteroid trialAsteroid = allAsteroids[x];
                trialAsteroid.CalculateVisibleAsteroids(allAsteroids);
                AoCUtilities.DebugWriteLine("Asteroid {0} can see {1} asteroids", x, trialAsteroid.VisibleAsteroids.Count);
            }

            Asteroid bestAsteroid = null;
            foreach (Asteroid asteroid in allAsteroids)
            {
                if (bestAsteroid == null || asteroid.VisibleAsteroids.Count > bestAsteroid.VisibleAsteroids.Count)
                {
                    bestAsteroid = asteroid;
                }
            }

            StationAsteroid = bestAsteroid;
            Console.WriteLine("Best Asteroid at ({0},{1}) seeing {2} other Asteroids", bestAsteroid.X, bestAsteroid.Y, bestAsteroid.VisibleAsteroids.Count);

            PrintMap();

            Asteroid _200thAsteroid = null;
            int asteroidsDestroyed = 0;

            while (allAsteroids.Count > 1)
            {
                List<Tuple<Asteroid, double>> visibleAsteroidsWithGradients = new List<Tuple<Asteroid, double>>();
                foreach (Asteroid visibleAsteroid in StationAsteroid.VisibleAsteroids)
                {
                    double gradientToVisibleAsteroid = StationAsteroid.AngleFromHereTo(visibleAsteroid);
                    visibleAsteroidsWithGradients.Add(new Tuple<Asteroid, double>(visibleAsteroid, gradientToVisibleAsteroid));
                }
                visibleAsteroidsWithGradients.Sort((x, y) => x.Item2.CompareTo(y.Item2));

                int i = 0;
                while (StationAsteroid.VisibleAsteroids.Count > 0)
                {
                    // select next visible asteroid
                    Asteroid nextAsteroid = visibleAsteroidsWithGradients[i].Item1;
                    Map[nextAsteroid.Y][nextAsteroid.X] = null;
                    StationAsteroid.VisibleAsteroids.Remove(nextAsteroid);
                    allAsteroids.Remove(nextAsteroid);

                    asteroidsDestroyed++;
                    if (asteroidsDestroyed == 200)
                        _200thAsteroid = nextAsteroid;

                    //PrintMap();

                    i++;
                }

                StationAsteroid.CalculateVisibleAsteroids(allAsteroids);
            }

            if (_200thAsteroid != null)
            {
                Console.WriteLine("200th Asteroid destroyed at ({0},{1})", _200thAsteroid.X, _200thAsteroid.Y);
                Console.WriteLine("(X * 100) + Y = {0}", (_200thAsteroid.X * 100) + _200thAsteroid.Y);
            }
            Console.ReadLine();
        }
    }
}
