using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _12
{
    class Moon
    {
        public int ID;

        public int X;
        public int Y;
        public int Z;

        public int Xvel = 0;
        public int Yvel = 0;
        public int Zvel = 0;

        public int PotentialEnergy;
        public int KineticEnergy;
        public int TotalEnergy;

        public Moon(int id, string startingPosition)
        {
            this.ID = id;

            string[] split1 = startingPosition.Substring(1, startingPosition.Length - 2).Split(',');
            string[] split2;

            split2 = split1[0].Split('=');
            this.X = Int32.Parse(split2[1]);

            split2 = split1[1].Split('=');
            this.Y = Int32.Parse(split2[1]);

            split2 = split1[2].Split('=');
            this.Z = Int32.Parse(split2[1]);
        }

        public void ApplyGravity(List<Moon> Moons)
        {
            for (int i = ID + 1; i < Moons.Count; i++)
            {
                Moon otherMoon = Moons[i];

                if (this != otherMoon)
                {
                    if (otherMoon.X > this.X)
                    {
                        this.Xvel++;
                        otherMoon.Xvel--;
                    }
                    else if (otherMoon.X < this.X)
                    {
                        this.Xvel--;
                        otherMoon.Xvel++;
                    }

                    if (otherMoon.Y > this.Y)
                    {
                        this.Yvel++;
                        otherMoon.Yvel--;
                    }
                    else if (otherMoon.Y < this.Y)
                    {
                        this.Yvel--;
                        otherMoon.Yvel++;
                    }

                    if (otherMoon.Z > this.Z)
                    {
                        this.Zvel++;
                        otherMoon.Zvel--;
                    }
                    else if (otherMoon.Z < this.Z)
                    {
                        this.Zvel--;
                        otherMoon.Zvel++;
                    }
                }
                else
                {
                    throw new Exception();
                }
            }

            this.X += this.Xvel;
            this.Y += this.Yvel;
            this.Z += this.Zvel;

            this.PotentialEnergy = Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
            this.KineticEnergy = Math.Abs(Xvel) + Math.Abs(Yvel) + Math.Abs(Zvel);
            this.TotalEnergy = PotentialEnergy * KineticEnergy;
        }

        public void PrintState()
        {
            AoCUtilities.DebugWriteLine("pos=<x={0}, y={1}, z={2}>, vel=<x={3}, y={4}, z={5}> pe={6} ke={7} te={8}",
                this.X,
                this.Y,
                this.Z,
                this.Xvel,
                this.Yvel,
                this.Zvel,
                this.PotentialEnergy,
                this.KineticEnergy,
                this.TotalEnergy);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();

            List<Moon> Moons = new List<Moon>();
            for (int i = 0; i < inputList.Count; i++)
            {
                Moons.Add(new Moon(i, inputList[i]));
            }

            foreach (Moon moon in Moons)
            {
                moon.PrintState();
            }
            AoCUtilities.DebugReadLine();

            int totalEnergyOfAllMoons = 0;

            for (int i = 0; i < 1000; i++)
            {
                foreach (Moon moon in Moons)
                {
                    moon.ApplyGravity(Moons);
                    moon.PrintState();
                }

                totalEnergyOfAllMoons = 0;
                foreach (Moon moon in Moons)
                {
                    totalEnergyOfAllMoons += moon.TotalEnergy;
                }

                AoCUtilities.DebugWriteLine("Total Energy of all Moons: {0}", totalEnergyOfAllMoons);

                AoCUtilities.DebugReadLine();
            }

            Console.WriteLine("Total Energy of all Moons: {0}", totalEnergyOfAllMoons);
            Console.ReadLine();
        }
    }
}
