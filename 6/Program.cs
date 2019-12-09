using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _6
{
    class SpaceObject
    {
        public string Name;
        public SpaceObject Orbitee;
        public int IndirectOrbits
        {
            get
            {
                if (Name == "COM")
                    return -1;
                else
                    return Orbitee.IndirectOrbits + 1;
            }
        }
        public List<SpaceObject> Orbiters = new List<SpaceObject>();

        public SpaceObject(string _name)
        {
            this.Name = _name;
        }
    }

    class Program
    {
        static Dictionary<string, SpaceObject> SpaceObjects = new Dictionary<string, SpaceObject>();

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();

            int directOrbits = 0;

            foreach (string orbitPair in inputList)
            {
                string[] objectsStrings = orbitPair.Split(')');

                string orbiteeName = objectsStrings[0];
                string orbiterName = objectsStrings[1];
                directOrbits++;

                SpaceObject orbitee;
                SpaceObject orbiter;

                if (!SpaceObjects.TryGetValue(orbiteeName, out orbitee))
                {
                    orbitee = new SpaceObject(orbiteeName);
                    SpaceObjects[orbiteeName] = orbitee;
                }
                if (!SpaceObjects.TryGetValue(orbiterName, out orbiter))
                {
                    orbiter = new SpaceObject(orbiterName);
                    SpaceObjects[orbiterName] = orbiter;
                }

                orbiter.Orbitee = orbitee;
                orbitee.Orbiters.Add(orbiter);
            }

            int indirectOrbits = 1; // COM has -1 indirect orbits

            foreach (KeyValuePair<string, SpaceObject> entry in SpaceObjects)
            {
                indirectOrbits += entry.Value.IndirectOrbits;
            }

            Console.WriteLine("Direct Orbits: {0}", directOrbits);
            Console.WriteLine("Indirect Orbits: {0}", indirectOrbits);
            int totalOrbits = directOrbits + indirectOrbits;
            Console.WriteLine("Total Orbits: {0}", totalOrbits);

            // PART 2

            SpaceObject commonObject = null;
            SpaceObject potentialCommonObject = SpaceObjects["YOU"];
            int youDistance = -1;
            int santaDistance = -1;
            for (int i = 0; i < SpaceObjects["YOU"].IndirectOrbits + 1; i++)
            {
                commonObject = null;
                potentialCommonObject = potentialCommonObject.Orbitee;
                youDistance++;

                SpaceObject testCommonObject = SpaceObjects["SAN"];
                santaDistance = -1;
                for (int j = 0; j < SpaceObjects["SAN"].IndirectOrbits + 1; j++)
                {
                    testCommonObject = testCommonObject.Orbitee;
                    santaDistance++;

                    if (potentialCommonObject == testCommonObject)
                    {
                        commonObject = potentialCommonObject;
                        break;
                    }
                }

                if (commonObject != null)
                {
                    break;
                }
            }

            if (commonObject != null)
            {
                Console.WriteLine("Hops to common node from YOU: {0}", youDistance);
                Console.WriteLine("Hops to SAN from common node: {0}", santaDistance);
                int totalHops = youDistance + santaDistance;
                Console.WriteLine("Total hops: {0}", totalHops);
                Console.ReadLine();
            }
            else
            {
                throw new ArgumentException("No common object found, at least COM should be common");
            }
        }
    }
}
