using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_18___Many_Worlds_Interpretation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            Node startNode = null;
            List<List<Node>> map = new List<List<Node>>();
            Dictionary<char, Node> keyNodes = new Dictionary<char, Node>();
            for (int y = 0; y < inputList.Count; y++)
            {
                map.Add(new List<Node>());
                for (int x = 0; x < inputList[y].Count(); x++)
                {
                    Node newNode = new Node(inputList[y][x], x, y);
                    if (inputList[y][x] == '@')
                    {
                        startNode = newNode;
                    }
                    if (newNode.Key != '0')
                        keyNodes[(char)(inputList[y][x] - 32)] = newNode;
                    map[y].Add(newNode);
                }
            }

            for (int y = 0; y < map.Count; y++)
            {
                for (int x = 0; x < map[y].Count; x++)
                {
                    map[y][x].ExploredDistance = -1;
                }
            }
            startNode.FindDistanceToKeys(map);

            foreach (var kVP in keyNodes)
            {
                Node node = kVP.Value;
                for (int y = 0; y < map.Count; y++)
                {
                    for (int x = 0; x < map[y].Count; x++)
                    {
                        map[y][x].ExploredDistance = -1;
                    }
                }

                node.FindDistanceToKeys(map);
            }

            P1(startNode, keyNodes);

            int midX = startNode.X;
            int midY = startNode.Y;
            map[midY][midX].Wall = true;
            map[midY - 1][midX].Wall = true;
            map[midY + 1][midX].Wall = true;
            map[midY][midX - 1].Wall = true;
            map[midY][midX + 1].Wall = true;

            List<Node> startNodes = new List<Node> { map[midY - 1][midX - 1], map[midY - 1][midX + 1], map[midY + 1][midX - 1], map[midY + 1][midX + 1] };

            for (int y = 0; y < map.Count; y++)
            {
                for (int x = 0; x < map[y].Count; x++)
                {
                    map[y][x].DistancesAndRequiredDoorsForKey = new Dictionary<char, (List<char>, int)>();
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int y = 0; y < map.Count; y++)
                {
                    for (int x = 0; x < map[y].Count; x++)
                    {
                        map[y][x].ExploredDistance = -1;
                    }
                }
                startNodes[i].FindDistanceToKeys(map);
            }

            foreach (var kVP in keyNodes)
            {
                Node node = kVP.Value;
                for (int y = 0; y < map.Count; y++)
                {
                    for (int x = 0; x < map[y].Count; x++)
                    {
                        map[y][x].ExploredDistance = -1;
                    }
                }

                node.FindDistanceToKeys(map);
            }

            P2(startNodes, keyNodes);
        }

        static void P1(Node startNode, Dictionary<char, Node> keyNodes)
        {
            State.cache = new Dictionary<string, State>();

            State startState = new State();
            startState.CurrentNodes = new List<Node> { startNode };
            State.cache[startState.UniqueStringRep()] = startState;

            startState.GenerateAdjacentStates(keyNodes);

            string fullAlphabetOfKeys = "";
            for (char a = 'A'; a < 'A' + keyNodes.Count; a++)
                fullAlphabetOfKeys += a;

            Dijsktra(startState, fullAlphabetOfKeys);

            List<int> minDistances = new List<int>();
            foreach (var kVP in State.cache)
            {
                string cacheKey = kVP.Key;
                if (cacheKey.Length > fullAlphabetOfKeys.Length && cacheKey.Substring(cacheKey.Length - fullAlphabetOfKeys.Length) == fullAlphabetOfKeys)
                    minDistances.Add(kVP.Value.DijsktraTotalDistance);
            }
            if (minDistances.Count == 0)
                throw new Exception();
            Console.WriteLine(minDistances.Min());
            Console.ReadLine();
        }

        static void P2(List<Node> startNodes, Dictionary<char, Node> keyNodes)
        {
            State.cache = new Dictionary<string, State>();

            State startState = new State();
            startState.CurrentNodes = startNodes;
            State.cache[startState.UniqueStringRep()] = startState;

            startState.GenerateAdjacentStates(keyNodes);

            string fullAlphabetOfKeys = "";
            for (char a = 'A'; a < 'A' + keyNodes.Count; a++)
                fullAlphabetOfKeys += a;

            Dijsktra(startState, fullAlphabetOfKeys);

            List<int> minDistances = new List<int>();
            foreach (var kVP in State.cache)
            {
                string cacheKey = kVP.Key;
                if (cacheKey.Length > fullAlphabetOfKeys.Length && cacheKey.Substring(cacheKey.Length - fullAlphabetOfKeys.Length) == fullAlphabetOfKeys)
                    minDistances.Add(kVP.Value.DijsktraTotalDistance);
            }
            if (minDistances.Count == 0)
                throw new Exception();
            Console.WriteLine(minDistances.Min());
            Console.ReadLine();
        }

        static void Dijsktra(State startState, string fullAlphabetOfKeys)
        {
            startState.DijsktraTotalDistance = 0;
            List<State> inProgressNodes = new List<State>() { startState };

            while (inProgressNodes.Count > 0)
            {
                int minTotalDistanceStateI = -1;
                for (int i = 0; i < inProgressNodes.Count; i++)
                {
                    State state = inProgressNodes[i];
                    if (minTotalDistanceStateI == -1 || state.DijsktraTotalDistance < inProgressNodes[minTotalDistanceStateI].DijsktraTotalDistance)
                        minTotalDistanceStateI = i;
                }

                if (minTotalDistanceStateI == -1)
                    throw new Exception();

                State minTotalDistanceState = inProgressNodes[minTotalDistanceStateI];
                inProgressNodes.RemoveAt(minTotalDistanceStateI);
                minTotalDistanceState.DijsktraInProgress = false;
                minTotalDistanceState.DijsktraExplored = true;

                string cacheKey = minTotalDistanceState.UniqueStringRep();
                if (cacheKey.Length > fullAlphabetOfKeys.Length && cacheKey.Substring(cacheKey.Length - fullAlphabetOfKeys.Length) == fullAlphabetOfKeys)
                    break;

                foreach (var (energyToReachStateAdjacent, stateAdjacentToMinTotalDistanceState) in minTotalDistanceState.AdjacentStates)
                {
                    if (!stateAdjacentToMinTotalDistanceState.DijsktraExplored)
                    {
                        if (!stateAdjacentToMinTotalDistanceState.DijsktraInProgress)
                        {
                            inProgressNodes.Add(stateAdjacentToMinTotalDistanceState);
                            stateAdjacentToMinTotalDistanceState.DijsktraInProgress = true;
                        }

                        int trialDistance = minTotalDistanceState.DijsktraTotalDistance + energyToReachStateAdjacent;

                        if (trialDistance < stateAdjacentToMinTotalDistanceState.DijsktraTotalDistance)
                            stateAdjacentToMinTotalDistanceState.DijsktraTotalDistance = trialDistance;
                    }
                }
            }
        }
    }

    public class State
    {
        public static Dictionary<string, State> cache = new Dictionary<string, State>();
        public static int cacheHits = 0;
        public static State cacheCheck(State state)
        {
            string cacheKey = state.UniqueStringRep();
            if (cache.ContainsKey(cacheKey))
            {
                state = cache[cacheKey];
                //Console.WriteLine($"CACHE HIT FOR {cacheKey}");
                cacheHits++;
            }
            else
                cache[cacheKey] = state;
            return state;
        }

        public bool AdjacentStatesGenerated = false;
        public List<(int, State)> AdjacentStates = new List<(int, State)>();

        public bool DijsktraExplored = false;
        public bool DijsktraInProgress = false;
        public int DijsktraTotalDistance = int.MaxValue;

        public List<Node> CurrentNodes;
        public List<char> CollectedKeys = new List<char>();

        public State()
        {

        }

        public State(State otherState)
        {
            CurrentNodes = new List<Node>(otherState.CurrentNodes);
            CollectedKeys = new List<char>(otherState.CollectedKeys);
        }
        public void GenerateAdjacentStates(Dictionary<char, Node> keyNodes)
        {
            for (int currentNodeIndex = 0; currentNodeIndex < CurrentNodes.Count; currentNodeIndex++)
            {
                Node currentNode = CurrentNodes[currentNodeIndex];

                foreach (var kVP in currentNode.DistancesAndRequiredDoorsForKey)
                {
                    char key = kVP.Key;
                    if (!CollectedKeys.Contains(key))
                    {
                        // we haven't yet collected this key
                        // do we have the keys needed to get to this key
                        var (requiredKeys, distanceToKey) = kVP.Value;
                        bool gotAllRequiredKeys = true;
                        foreach (char requiredKey in requiredKeys)
                        {
                            if (!CollectedKeys.Contains(requiredKey))
                            {
                                gotAllRequiredKeys = false;
                                break;
                            }
                        }

                        if (gotAllRequiredKeys)
                        {
                            // collecting this key is an adjacent move
                            State newState = new State(this);
                            newState.CurrentNodes[currentNodeIndex] = keyNodes[key];
                            newState.CollectedKeys.Add(key);
                            newState.CollectedKeys.Sort();
                            newState = cacheCheck(newState);
                            AdjacentStates.Add((distanceToKey, newState));
                        }
                    }
                }
            }

            //this.Print();

            AdjacentStatesGenerated = true;
            foreach (var (e, s) in AdjacentStates)
            {
                if (!s.AdjacentStatesGenerated)
                    s.GenerateAdjacentStates(keyNodes);
            }
        }

        public void Print()
        {
            Console.WriteLine($"Positions:");
            foreach (Node currentNode in CurrentNodes)
            {
                Console.WriteLine($"{currentNode.X},{ currentNode.Y}");
            }
            Console.WriteLine($"Collected Keys: {string.Join("", CollectedKeys)}");
            Console.WriteLine($"Adjacent States: {AdjacentStates.Count}");
            //Console.ReadLine();
        }

        public string UniqueStringRep()
        {
            string strRep = "";
            foreach (Node currentNode in CurrentNodes)
            {
                strRep += $"{currentNode.X},{ currentNode.Y}_";
            }
            strRep += $"{string.Join("", CollectedKeys)}";
            return strRep;
        }
    }

    public class Node
    {
        public int ExploredDistance = -1;

        public int X;
        public int Y;

        public bool Wall = false;
        public char Key = '0';
        public char Door = '0';

        public Dictionary<char, (List<char>, int)> DistancesAndRequiredDoorsForKey = new Dictionary<char, (List<char>, int)>();

        public Node(char c, int x, int y)
        {
            X = x;
            Y = y;

            if (c == '#')
                Wall = true;
            else if (c >= 'a' && c <= 'z')
                Key = (char)(c - 32);
            else if (c >= 'A' && c <= 'Z')
                Door = c;
        }

        public void FindDistanceToKeys(List<List<Node>> map)
        {
            Explore(map, DistancesAndRequiredDoorsForKey);
        }

        public void Explore(List<List<Node>> map, Dictionary<char, (List<char>, int)> distancesAndRequiredDoorsForKey, List<char> doorsOnWay = null, int distance = 0)
        {
            if (doorsOnWay == null)
                doorsOnWay = new List<char>();
            this.ExploredDistance = distance;

            if (Key != '0' && distance != 0)
            {
                distancesAndRequiredDoorsForKey[this.Key] = (doorsOnWay, distance);
            }

            if (X > 0)
            {
                Node trialNode = map[Y][X - 1];
                if ((trialNode.ExploredDistance == -1 || trialNode.ExploredDistance > distance + 1) && !trialNode.Wall)
                {
                    List<char> doorsOnWayClone = new List<char>(doorsOnWay);
                    if (Door != '0')
                        doorsOnWayClone.Add(this.Door);
                    trialNode.Explore(map, distancesAndRequiredDoorsForKey, doorsOnWayClone, distance + 1);
                }
            }
            if (X < map[Y].Count - 1)
            {
                Node trialNode = map[Y][X + 1];
                if ((trialNode.ExploredDistance == -1 || trialNode.ExploredDistance > distance + 1) && !trialNode.Wall)
                {
                    List<char> doorsOnWayClone = new List<char>(doorsOnWay);
                    if (Door != '0')
                        doorsOnWayClone.Add(this.Door);
                    trialNode.Explore(map, distancesAndRequiredDoorsForKey, doorsOnWayClone, distance + 1);
                }
            }
            if (Y > 0)
            {
                Node trialNode = map[Y - 1][X];
                if ((trialNode.ExploredDistance == -1 || trialNode.ExploredDistance > distance + 1) && !trialNode.Wall)
                {
                    List<char> doorsOnWayClone = new List<char>(doorsOnWay);
                    if (Door != '0')
                        doorsOnWayClone.Add(this.Door);
                    trialNode.Explore(map, distancesAndRequiredDoorsForKey, doorsOnWayClone, distance + 1);
                }
            }
            if (Y < map.Count - 1)
            {
                Node trialNode = map[Y + 1][X];
                if ((trialNode.ExploredDistance == -1 || trialNode.ExploredDistance > distance + 1) && !trialNode.Wall)
                {
                    List<char> doorsOnWayClone = new List<char>(doorsOnWay);
                    if (Door != '0')
                        doorsOnWayClone.Add(this.Door);
                    trialNode.Explore(map, distancesAndRequiredDoorsForKey, doorsOnWayClone, distance + 1);
                }
            }
        }

        public override string ToString()
        {
            if (Key != '0')
                return $"Key {Key}";
            else if (Door != '0')
                return $"Door {Door}";
            else if (Wall)
                return $"Wall";
            else
                return $"Space";
        }
    }
}
