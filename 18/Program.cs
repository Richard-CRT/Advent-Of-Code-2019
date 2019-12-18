using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _18
{
    enum GridCellType { Open, Wall };

    class KeyDoorPair
    {
        public Key Key = null;
        public Door Door = null;
    }

    class Door
    {
        public char Character;
        public GridCell GridCell = null;
        public Key Key = null;
        public bool Locked = true;

        public Door(GridCell gridCell, char character)
        {
            this.GridCell = gridCell;
            this.Character = character;
        }

    }

    class Key
    {
        public char Character;
        public GridCell GridCell = null;
        public Door Door = null;

        public Key(GridCell gridCell, char character)
        {
            this.GridCell = gridCell;
            this.Character = character;
        }
    }

    class Existence
    {
        public Key LocationKey = null;
        public List<Key> CollectedKeys = new List<Key>();

        public Existence(Key locationKey)
        {
            this.LocationKey = locationKey;
        }
    }

    class GridCell
    {
        public int X;
        public int Y;
        public GridCellType Type;
        public Key Key = null;
        public Door Door = null;
        public bool YouHere = false;

        public GridCell(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    class Program
    {
        static List<List<GridCell>> Grid = new List<List<GridCell>>();

        static Dictionary<char, KeyDoorPair> KeysAndDoors = new Dictionary<char, KeyDoorPair>();
        static List<Key> Keys = new List<Key>();
        static List<Door> Doors = new List<Door>();

        static Existence StartExistence;

        static void PrintGrid()
        {
            for (int y = 0; y < Grid.Count; y++)
            {
                for (int x = 0; x < Grid[y].Count(); x++)
                {
                    GridCell gridCell = Grid[y][x];
                    switch (gridCell.Type)
                    {
                        case GridCellType.Open:
                            if (gridCell.YouHere)
                            {
                                AoCUtilities.DebugWrite("@");
                            }
                            else if (gridCell.Key != null)
                            {
                                AoCUtilities.DebugWrite("{0}", (char)((int)gridCell.Key.Character + 32));
                            }
                            else if (gridCell.Door != null && gridCell.Door.Locked)
                            {
                                AoCUtilities.DebugWrite("{0}", gridCell.Door.Character);
                            }
                            else
                            {
                                AoCUtilities.DebugWrite(".");
                            }
                            break;
                        case GridCellType.Wall:
                            AoCUtilities.DebugWrite("▓");
                            break;
                    }
                }
                AoCUtilities.DebugWriteLine();
            }
        }

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();

            for (int y = 0; y < inputList.Count; y++)
            {
                Grid.Add(new List<GridCell>());
                for (int x = 0; x < inputList[y].Count(); x++)
                {
                    GridCell gridCell = new GridCell(x, y);

                    Grid[y].Add(gridCell);

                    char inputChar = inputList[y][x];
                    if (inputChar == '#')
                    {
                        gridCell.Type = GridCellType.Wall;
                    }
                    else
                    {
                        gridCell.Type = GridCellType.Open;

                        if (inputChar >= 'a' && inputChar <= 'z')
                        {
                            char upperChar = (char)((int)inputChar - 32);
                            // key
                            Key newKey = new Key(gridCell, upperChar);
                            gridCell.Key = newKey;

                            Keys.Add(newKey);

                            if (!KeysAndDoors.ContainsKey(upperChar))
                            {
                                KeysAndDoors[upperChar] = new KeyDoorPair();
                            }
                            KeysAndDoors[upperChar].Key = newKey;
                            if (KeysAndDoors[upperChar].Door != null)
                            {
                                KeysAndDoors[upperChar].Door.Key = newKey;
                                newKey.Door = KeysAndDoors[upperChar].Door;
                            }
                        }
                        else if (inputChar >= 'A' && inputChar <= 'Z')
                        {
                            // door
                            Door newDoor = new Door(gridCell, inputChar);
                            gridCell.Door = newDoor;

                            Doors.Add(newDoor);

                            if (!KeysAndDoors.ContainsKey(inputChar))
                            {
                                KeysAndDoors[inputChar] = new KeyDoorPair();
                            }
                            KeysAndDoors[inputChar].Door = newDoor;
                            if (KeysAndDoors[inputChar].Key != null)
                            {
                                KeysAndDoors[inputChar].Key.Door = newDoor;
                                newDoor.Key = KeysAndDoors[inputChar].Key;
                            }
                        }
                        else if (inputChar == '@')
                        {
                            gridCell.YouHere = true;
                            StartExistence = new Existence(new Key(gridCell, '@'));
                        }
                    }
                }
            }

            // add start existence to new existences

            List<Existence> existences = new List<Existence>();
            existences.Add(StartExistence);

            List<Existence> existencesToScanFrom = new List<Existence>();
            existencesToScanFrom.Add(StartExistence);

            while (existencesToScanFrom.Count > 0)
            {
                List<Existence> existencesToScanFromNext = new List<Existence>();

                foreach (Existence existence in existencesToScanFrom)
                {
                    // breadth first from each new existence (with relevant doors unlocked), find available keys and create new existences with distance costs

                    List<GridCell> gridCellsExplored = new List<GridCell>();
                    List<GridCell> gridCellsToExplore = new List<GridCell>();

                    gridCellsToExplore.Add(existence.LocationKey.GridCell);

                    while (gridCellsToExplore.Count > 0)
                    {
                        List<GridCell> gridCellsToExploreNext = new List<GridCell>();
                        foreach (GridCell gridCell in gridCellsToExplore)
                        {
                            if (!gridCellsExplored.Contains(gridCell))
                            {
                                if (gridCell.Key != null && !existence.CollectedKeys.Contains(gridCell.Key))
                                {
                                    // this cell has a new available Key
                                    Existence newExistence = new Existence(gridCell.Key);

                                    List<Key> newCollectedKeys = new List<Key>();
                                    newCollectedKeys.AddRange(existence.CollectedKeys);
                                    newCollectedKeys.Add(gridCell.Key);
                                    newExistence.CollectedKeys.AddRange(newCollectedKeys);

                                    Existence matchingExistence = null;
                                    foreach (Existence oldExistence in existences)
                                    {
                                        if (oldExistence.LocationKey == newExistence.LocationKey && oldExistence.CollectedKeys.Count == newExistence.CollectedKeys.Count)
                                        {
                                            bool matches = true;
                                            foreach (Key collectedKey in newExistence.CollectedKeys)
                                            {
                                                if (!oldExistence.CollectedKeys.Contains(collectedKey))
                                                {
                                                    matches = false;
                                                    break;
                                                }
                                            }
                                            if (matches)
                                            {
                                                matchingExistence = oldExistence;
                                            }
                                        }
                                    }

                                    if (matchingExistence == null)
                                    {
                                        existencesToScanFromNext.Add(newExistence);
                                        existences.Add(newExistence);
                                    }
                                }
                                else if ((gridCell.Door != null && !existence.CollectedKeys.Contains(gridCell.Door.Key)) || gridCell.Type == GridCellType.Wall)
                                {
                                    // is locked door or wall
                                }
                                else
                                {
                                    if (gridCell.X > 0)
                                        gridCellsToExploreNext.Add(Grid[gridCell.Y][gridCell.X - 1]);
                                    if (gridCell.X < Grid[gridCell.Y].Count - 1)
                                        gridCellsToExploreNext.Add(Grid[gridCell.Y][gridCell.X + 1]);
                                    if (gridCell.Y > 0)
                                        gridCellsToExploreNext.Add(Grid[gridCell.Y - 1][gridCell.X]);
                                    if (gridCell.Y < Grid.Count - 1)
                                        gridCellsToExploreNext.Add(Grid[gridCell.Y + 1][gridCell.X]);
                                }

                                gridCellsExplored.Add(gridCell);
                            }
                        }
                        gridCellsToExplore = gridCellsToExploreNext;
                    }
                }
                existencesToScanFrom = existencesToScanFromNext;
            }

            // for all existences with all keys found get path length from start (dijkstra?) and find the shortest one

            PrintGrid();
            Console.WriteLine("test");
            Console.ReadLine();
        }
    }
}
