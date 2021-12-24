using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _12
{
    class AxisState
    {
        public int Position;

        public int Velocity;

        public AxisState()
        {

        }

        public AxisState(int position, int velocity)
        {
            this.Position = position;
            this.Velocity = velocity;
        }

        public AxisState(AxisState copyState) : this(copyState.Position, copyState.Velocity)
        {
        }

        public override bool Equals(object obj)
        {
            AxisState otherState = obj as AxisState;

            if (this.Position == otherState.Position &&
                this.Velocity == otherState.Velocity)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    class Moon
    {
        public int ID;

        public AxisState XState = new AxisState();
        public AxisState YState = new AxisState();
        public AxisState ZState = new AxisState();

        public int PotentialEnergy;
        public int KineticEnergy;
        public int TotalEnergy;

        public Moon(int id, string startingPosition)
        {
            this.ID = id;

            string[] split1 = startingPosition.Substring(1, startingPosition.Length - 2).Split(',');
            string[] split2;

            split2 = split1[0].Split('=');
            this.XState.Position = Int32.Parse(split2[1]);

            split2 = split1[1].Split('=');
            this.YState.Position = Int32.Parse(split2[1]);

            split2 = split1[2].Split('=');
            this.ZState.Position = Int32.Parse(split2[1]);
        }

        public void ApplyGravity(List<Moon> Moons)
        {
            for (int i = ID + 1; i < Moons.Count; i++)
            {
                Moon otherMoon = Moons[i];

                if (this != otherMoon)
                {
                    if (otherMoon.XState.Position > this.XState.Position)
                    {
                        this.XState.Velocity++;
                        otherMoon.XState.Velocity--;
                    }
                    else if (otherMoon.XState.Position < this.XState.Position)
                    {
                        this.XState.Velocity--;
                        otherMoon.XState.Velocity++;
                    }

                    if (otherMoon.YState.Position > this.YState.Position)
                    {
                        this.YState.Velocity++;
                        otherMoon.YState.Velocity--;
                    }
                    else if (otherMoon.YState.Position < this.YState.Position)
                    {
                        this.YState.Velocity--;
                        otherMoon.YState.Velocity++;
                    }

                    if (otherMoon.ZState.Position > this.ZState.Position)
                    {
                        this.ZState.Velocity++;
                        otherMoon.ZState.Velocity--;
                    }
                    else if (otherMoon.ZState.Position < this.ZState.Position)
                    {
                        this.ZState.Velocity--;
                        otherMoon.ZState.Velocity++;
                    }
                }
                else
                {
                    throw new Exception();
                }
            }

            this.XState.Position += this.XState.Velocity;
            this.YState.Position += this.YState.Velocity;
            this.ZState.Position += this.ZState.Velocity;

            this.PotentialEnergy = Math.Abs(this.XState.Position) + Math.Abs(this.YState.Position) + Math.Abs(this.ZState.Position);
            this.KineticEnergy = Math.Abs(this.XState.Velocity) + Math.Abs(this.YState.Velocity) + Math.Abs(this.ZState.Velocity);
            this.TotalEnergy = PotentialEnergy * KineticEnergy;
        }

        public void PrintState()
        {
            AoCUtilities.DebugWriteLine("pos=<x={0}, y={1}, z={2}>, vel=<x={3}, y={4}, z={5}> pe={6} ke={7} te={8}",
                this.XState.Position,
                this.YState.Position,
                this.ZState.Position,
                this.XState.Velocity,
                this.YState.Velocity,
                this.YState.Velocity,
                this.PotentialEnergy,
                this.KineticEnergy,
                this.TotalEnergy);
        }
    }

    class Program
    {
        static Int64 gcf(Int64 a, Int64 b)
        {
            while (b != 0)
            {
                Int64 temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        static Int64 lcm(Int64 a, Int64 b)
        {
            return (a / gcf(a, b)) * b;
        }

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();

            List<Moon> Moons = new List<Moon>();
            List<AxisState> startXStates = new List<AxisState>();
            List<AxisState> startYStates = new List<AxisState>();
            List<AxisState> startZStates = new List<AxisState>();

            for (int i = 0; i < inputList.Count; i++)
            {
                Moons.Add(new Moon(i, inputList[i]));
                startXStates.Add(new AxisState(Moons[i].XState));
                startYStates.Add(new AxisState(Moons[i].YState));
                startZStates.Add(new AxisState(Moons[i].ZState));
            }
            int xRepeats = -1;
            int yRepeats = -1;
            int zRepeats = -1;

            int totalEnergyOfAllMoons = 0;

            int stepIndex = 0;
            while (xRepeats == -1 || yRepeats == -1 || zRepeats == -1)
            {
                bool xMatches = true;
                bool yMatches = true;
                bool zMatches = true;

                for (int moonIndex = 0; moonIndex < Moons.Count; moonIndex++)
                {
                    Moon moon = Moons[moonIndex];

                    if (!moon.XState.Equals(startXStates[moonIndex]))
                    {
                        xMatches = false;
                    }
                    if (!moon.YState.Equals(startYStates[moonIndex]))
                    {
                        yMatches = false;
                    }
                    if (!moon.ZState.Equals(startZStates[moonIndex]))
                    {
                        zMatches = false;
                    }
                }

                if (xMatches)
                {
                    AoCUtilities.DebugWriteLine("Step {0}: all moons are in their start X state", stepIndex);
                    if (xRepeats == -1 && stepIndex != 0)
                    {
                        xRepeats = stepIndex;
                    }
                }
                if (yMatches)
                {
                    AoCUtilities.DebugWriteLine("Step {0}: all moons are in their start Y state", stepIndex);
                    if (yRepeats == -1 && stepIndex != 0)
                    {
                        yRepeats = stepIndex;
                    }
                }
                if (zMatches)
                {
                    AoCUtilities.DebugWriteLine("Step {0}: all moons are in their start Z state", stepIndex);
                    if (zRepeats == -1 && stepIndex != 0)
                    {
                        zRepeats = stepIndex;
                    }
                }

                totalEnergyOfAllMoons = 0;

                for (int moonIndex = 0; moonIndex < Moons.Count; moonIndex++)
                {
                    Moon moon = Moons[moonIndex];

                    moon.PrintState();
                    totalEnergyOfAllMoons += moon.TotalEnergy;

                    moon.ApplyGravity(Moons);
                }


                //AoCUtilities.DebugWriteLine("Total Energy of all Moons: {0}", totalEnergyOfAllMoons);

                AoCUtilities.DebugReadLine();

                stepIndex++;
            }

            Console.WriteLine("Total Energy of all Moons is {0} after {1} steps", totalEnergyOfAllMoons, stepIndex);
            Console.WriteLine("X Axis Repeats Every {0} Steps", xRepeats);
            Console.WriteLine("Y Axis Repeats Every {0} Steps", yRepeats);
            Console.WriteLine("Z Axis Repeats Every {0} Steps", zRepeats);

            Int64 lcmXY = lcm(xRepeats, yRepeats);
            Int64 earliestStepWithAllRepeat = lcm(lcmXY, zRepeats);
            Console.WriteLine("Earliest Step With All Axes Repeated: {0}", earliestStepWithAllRepeat);
            Console.ReadLine();
        }
    }
}
