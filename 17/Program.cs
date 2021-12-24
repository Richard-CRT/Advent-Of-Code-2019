using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCodeUtilities;
using IntCodeComputerNS;

namespace _13
{
    enum TileType { Space, Scaffold };
    enum RobotDirection { None, Left, Up, Right, Down, Tumble };

    class MovementAction
    {
        public RobotDirection Direction;
        public RobotDirection Turn;
        public int Quantity;

        public MovementAction()
        {

        }

        public MovementAction(RobotDirection turn, int quantity)
        {
            this.Turn = turn;
            this.Quantity = quantity;
        }

        public override bool Equals(object obj)
        {
            MovementAction otherMovementAction = obj as MovementAction;
            return this.Turn == otherMovementAction.Turn && this.Quantity == otherMovementAction.Quantity;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    class Tile
    {
        public TileType Type;
        public bool Intersection = false;
        public RobotDirection RobotDirection = RobotDirection.None;

        public Tile(TileType type)
        {
            this.Type = type;
        }
        public Tile(TileType type, RobotDirection robotDirection)
        {
            this.Type = type;
            this.RobotDirection = robotDirection;
        }
    }

    class Program
    {
        static List<int> Inputs = new List<int>();

        static long InputMethod()
        {
            long input = 1;

            AoCUtilities.DebugWrite("Input: ");

            if (Inputs.Count > 0)
            {
                input = Inputs[0];
                Inputs.RemoveAt(0);
            }
            else
            {
                input = Int64.Parse(Console.ReadLine());
            }

            AoCUtilities.DebugWriteLine("{0}", input);
            //AoCUtilities.DebugReadLine();

            return input;
        }

        static char LastOutput = ' ';

        static string MovementActionsToString(List<MovementAction> movementActions)
        {
            string movementActionString = "";

            for (int i = 0; i < movementActions.Count; i++)
            {
                MovementAction movementAction = movementActions[i];

                switch (movementAction.Turn)
                {
                    case RobotDirection.Right:
                        movementActionString += "R,";
                        break;
                    case RobotDirection.Left:
                        movementActionString += "L,";
                        break;
                }

                movementActionString += movementAction.Quantity.ToString();

                if (i != movementActions.Count - 1)
                {
                    movementActionString += ",";
                }
            }

            return movementActionString;
        }

        static List<MovementAction> RemoveMovementActionSubListFrom(List<MovementAction> movementActions, List<MovementAction> subMovementActions)
        {
            List<MovementAction> movementActionsWithSubRemoved = new List<MovementAction>(movementActions);

            for (int i = 0; i < movementActionsWithSubRemoved.Count - subMovementActions.Count + 1;)
            {
                bool rangeMatch = true;
                for (int j = 0; j < subMovementActions.Count; j++)
                {
                    if (!movementActionsWithSubRemoved[i + j].Equals(subMovementActions[j]))
                    {
                        rangeMatch = false;
                        break;
                    }
                }
                if (rangeMatch)
                {
                    movementActionsWithSubRemoved.RemoveRange(i, subMovementActions.Count);
                }
                else
                {
                    i++;
                }
            }

            return movementActionsWithSubRemoved;
        }

        static List<char> MovementActionListToSubRoutine(List<MovementAction> allMovementActions, List<MovementAction> aMovementActions, List<MovementAction> bMovementActions, List<MovementAction> cMovementActions)
        {
            List<char> subRoutineOrder = new List<char>();

            for (int i = 0; i < allMovementActions.Count;)
            {
                bool aRangeMatch = true;
                bool bRangeMatch = true;
                bool cRangeMatch = true;
                for (int j = 0; j < aMovementActions.Count; j++)
                {
                    if (i + j < allMovementActions.Count && !allMovementActions[i + j].Equals(aMovementActions[j]))
                    {
                        aRangeMatch = false;
                        break;
                    }
                }
                if (aRangeMatch)
                {
                    subRoutineOrder.Add('A');
                    allMovementActions.RemoveRange(i, aMovementActions.Count);
                }
                else
                {
                    for (int j = 0; j < bMovementActions.Count; j++)
                    {
                        if (i + j < allMovementActions.Count && !allMovementActions[i + j].Equals(bMovementActions[j]))
                        {
                            bRangeMatch = false;
                            break;
                        }
                    }
                    if (bRangeMatch)
                    {
                        subRoutineOrder.Add('B');
                        allMovementActions.RemoveRange(i, bMovementActions.Count);
                    }
                    else
                    {
                        for (int j = 0; j < cMovementActions.Count; j++)
                        {
                            if (i + j < allMovementActions.Count && !allMovementActions[i + j].Equals(cMovementActions[j]))
                            {
                                cRangeMatch = false;
                                break;
                            }
                        }
                        if (cRangeMatch)
                        {
                            subRoutineOrder.Add('C');
                            allMovementActions.RemoveRange(i, cMovementActions.Count);
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }
            }

            return subRoutineOrder;
        }

        static void CameraComplete()
        {
            int robotX = 0;
            int robotY = 0;
            RobotDirection robotDirection = RobotDirection.Up;

            for (int y = 0; y < Grid.Count(); y++)
            {
                for (int x = 0; x < Grid[y].Count; x++)
                {
                    int adjacentScaffolds = 0;
                    if (Grid[y][x].Type == TileType.Scaffold)
                    {
                        if (y > 0)
                        {
                            if (Grid[y - 1][x].Type == TileType.Scaffold)
                                adjacentScaffolds++;
                        }
                        if (y < Grid.Count - 1)
                        {
                            if (Grid[y + 1][x].Type == TileType.Scaffold)
                                adjacentScaffolds++;
                        }
                        if (x > 0)
                        {
                            if (Grid[y][x - 1].Type == TileType.Scaffold)
                                adjacentScaffolds++;
                        }
                        if (x < Grid[y].Count - 1)
                        {
                            if (Grid[y][x + 1].Type == TileType.Scaffold)
                                adjacentScaffolds++;
                        }

                        if (adjacentScaffolds > 2)
                        {
                            Grid[y][x].Intersection = true;
                            int alignmentParameter = y * x;
                            SumAlignmentParameters += alignmentParameter;
                        }
                    }

                    if (Grid[y][x].RobotDirection != RobotDirection.None)
                    {
                        robotX = x;
                        robotY = y;
                        robotDirection = Grid[y][x].RobotDirection;
                    }
                }
            }

            Console.WriteLine("Sum of the Alignment Parameters: {0}", SumAlignmentParameters);
            //Console.ReadLine();

            List<MovementAction> movementActions = new List<MovementAction>();

            while (true)
            {
                MovementAction movementAction = new MovementAction();
                if (robotDirection != RobotDirection.Down && robotY > 0 && Grid[robotY - 1][robotX].Type == TileType.Scaffold)
                {
                    if (robotDirection == RobotDirection.Left)
                        movementAction.Turn = RobotDirection.Right;
                    else
                        movementAction.Turn = RobotDirection.Left;

                    robotDirection = RobotDirection.Up;

                    movementAction.Direction = robotDirection;
                    for (; robotY > 0 && Grid[robotY - 1][robotX].Type == TileType.Scaffold; robotY--)
                    {
                        movementAction.Quantity++;
                    }

                    movementActions.Add(movementAction);
                }
                else if (robotDirection != RobotDirection.Left && robotX < Grid[robotY].Count - 1 && Grid[robotY][robotX + 1].Type == TileType.Scaffold)
                {
                    if (robotDirection == RobotDirection.Up)
                        movementAction.Turn = RobotDirection.Right;
                    else
                        movementAction.Turn = RobotDirection.Left;

                    robotDirection = RobotDirection.Right;

                    movementAction.Direction = robotDirection;
                    for (; robotX < Grid[robotY].Count - 1 && Grid[robotY][robotX + 1].Type == TileType.Scaffold; robotX++)
                    {
                        movementAction.Quantity++;
                    }

                    movementActions.Add(movementAction);
                }
                else if (robotDirection != RobotDirection.Up && robotY < Grid.Count - 1 && Grid[robotY + 1][robotX].Type == TileType.Scaffold)
                {
                    if (robotDirection == RobotDirection.Right)
                        movementAction.Turn = RobotDirection.Right;
                    else
                        movementAction.Turn = RobotDirection.Left;

                    robotDirection = RobotDirection.Down;

                    movementAction.Direction = robotDirection;
                    for (; robotY < Grid.Count - 1 && Grid[robotY + 1][robotX].Type == TileType.Scaffold; robotY++)
                    {
                        movementAction.Quantity++;
                    }

                    movementActions.Add(movementAction);
                }
                else if (robotDirection != RobotDirection.Right && robotX > 0 && Grid[robotY][robotX - 1].Type == TileType.Scaffold)
                {
                    if (robotDirection == RobotDirection.Down)
                        movementAction.Turn = RobotDirection.Right;
                    else
                        movementAction.Turn = RobotDirection.Left;

                    robotDirection = RobotDirection.Left;

                    movementAction.Direction = robotDirection;
                    for (; robotX > 0 && Grid[robotY][robotX - 1].Type == TileType.Scaffold; robotX--)
                    {
                        movementAction.Quantity++;
                    }

                    movementActions.Add(movementAction);
                }
                else
                {
                    // end of the path
                    break;
                }
            }

            AoCUtilities.DebugWrite("Movement Actions: ");
            AoCUtilities.DebugWriteLine(MovementActionsToString(movementActions));

            // R,8,L,10,L,12,R,4,R,8,L,12,R,4,R,4,R,8,L,10,L,12,R,4,R,8,L,10,R,8,R,8,L,10,L,12,R,4,R,8,L,12,R,4,R,4,R,8,L,10,R,8,R,8,L,12,R,4,R,4,R,8,L,10,R,8,R,8,L,12,R,4,R,4

            /*
            // Sample
            // R,8,R,8,R,4,R,4,R,8,L,6,L,2,R,4,R,4,R,8,R,8,R,8,L,6,L,2
            // A: R,8,R,8,R,4,R,4,R,8
            // B: L,6,L,2,R,4,R,4,R,8
            // C: R,8,R,8,L,6,L,2
            // A,B,C
            movementActions.Clear();
            movementActions.Add(new MovementAction(RobotDirection.Right, 8));
            movementActions.Add(new MovementAction(RobotDirection.Right, 8));
            movementActions.Add(new MovementAction(RobotDirection.Right, 4));
            movementActions.Add(new MovementAction(RobotDirection.Right, 4));
            movementActions.Add(new MovementAction(RobotDirection.Right, 8));
            movementActions.Add(new MovementAction(RobotDirection.Left, 6));
            movementActions.Add(new MovementAction(RobotDirection.Left, 2));
            movementActions.Add(new MovementAction(RobotDirection.Right, 4));
            movementActions.Add(new MovementAction(RobotDirection.Right, 4));
            movementActions.Add(new MovementAction(RobotDirection.Right, 8));
            movementActions.Add(new MovementAction(RobotDirection.Right, 8));
            movementActions.Add(new MovementAction(RobotDirection.Right, 8));
            movementActions.Add(new MovementAction(RobotDirection.Left, 6));
            movementActions.Add(new MovementAction(RobotDirection.Left, 2));
            */

            bool done = false;

            for (int ALength = movementActions.Count; ALength > 0; ALength--)
            {
                aMovementActions = movementActions.GetRange(0, ALength);
                if (MovementActionsToString(aMovementActions).Length <= 20)
                {
                    List<MovementAction> movementActionsWithARemoved = RemoveMovementActionSubListFrom(movementActions, aMovementActions);

                    for (int BLength = movementActionsWithARemoved.Count; BLength > 0; BLength--)
                    {
                        bMovementActions = movementActionsWithARemoved.GetRange(0, BLength);
                        if (MovementActionsToString(bMovementActions).Length <= 20)
                        {
                            List<MovementAction> aMovementActionsWithBRemoved = RemoveMovementActionSubListFrom(movementActionsWithARemoved, bMovementActions);

                            for (int CLength = aMovementActionsWithBRemoved.Count; CLength > 0; CLength--)
                            {
                                cMovementActions = aMovementActionsWithBRemoved.GetRange(0, CLength);
                                if (MovementActionsToString(cMovementActions).Length <= 20)
                                {
                                    List<MovementAction> bMovementActionsWithCRemoved = RemoveMovementActionSubListFrom(aMovementActionsWithBRemoved, cMovementActions);

                                    if (bMovementActionsWithCRemoved.Count == 0)
                                    {
                                        done = true;
                                        break;
                                    }
                                }
                            }

                            if (done)
                                break;
                        }
                    }

                    if (done)
                        break;
                }
            }

            if (done == true)
            {
                AoCUtilities.DebugWriteLine("A: {0}", MovementActionsToString(aMovementActions));
                AoCUtilities.DebugWriteLine("B: {0}", MovementActionsToString(bMovementActions));
                AoCUtilities.DebugWriteLine("C: {0}", MovementActionsToString(cMovementActions));
                //AoCUtilities.DebugReadLine();

                List<char> subRoutineOrder = null;
                subRoutineOrder = MovementActionListToSubRoutine(movementActions, aMovementActions, bMovementActions, cMovementActions);

                for (int i = 0; i < subRoutineOrder.Count; i++)
                {
                    Inputs.Add(subRoutineOrder[i]);
                    AoCUtilities.DebugWrite("{0}", subRoutineOrder[i]);
                    if (i != subRoutineOrder.Count - 1)
                    {
                        Inputs.Add(',');
                        AoCUtilities.DebugWrite(",");
                    }
                    else
                    {
                        Inputs.Add('\n');
                        AoCUtilities.DebugWrite("\n");
                    }
                }

                string aString = MovementActionsToString(aMovementActions);
                for (int i = 0; i < aString.Length; i++)
                {
                    Inputs.Add(aString[i]);
                    AoCUtilities.DebugWrite("{0}",aString[i]);
                }
                Inputs.Add('\n');
                AoCUtilities.DebugWrite("\n");

                string bString = MovementActionsToString(bMovementActions);
                for (int i = 0; i < bString.Length; i++)
                {
                    Inputs.Add(bString[i]);
                    AoCUtilities.DebugWrite("{0}", bString[i]);
                }
                Inputs.Add('\n');
                AoCUtilities.DebugWrite("\n");

                string cString = MovementActionsToString(cMovementActions);
                for (int i = 0; i < cString.Length; i++)
                {
                    Inputs.Add(cString[i]);
                    AoCUtilities.DebugWrite("{0}", cString[i]);
                }
                Inputs.Add('\n');
                Inputs.Add('n');
                Inputs.Add('\n');
                AoCUtilities.DebugWrite("\n");
            }
            else
            {
                throw new Exception();
            }
        }

        static void OutputMethod(long output)
        {
            //AoCUtilities.DebugWriteLine("Output: {0}", output);
            //AoCUtilities.DebugWrite("{0}", (char)output);

            if (status == 0)
            {
                switch (output)
                {
                    case '\n':
                        if (LastOutput == '\n')
                        {
                            status = 1;
                            CameraComplete();
                        }
                        else
                        {
                            Grid.Add(NewGridLine);
                            NewGridLine = new List<Tile>();
                            PrintGrid();
                        }
                        break;
                    case '.':
                        NewGridLine.Add(new Tile(TileType.Space));
                        break;
                    case '#':
                        NewGridLine.Add(new Tile(TileType.Scaffold));
                        break;
                    case '<':
                        NewGridLine.Add(new Tile(TileType.Scaffold, RobotDirection.Left));
                        break;
                    case '^':
                        NewGridLine.Add(new Tile(TileType.Scaffold, RobotDirection.Up));
                        break;
                    case '>':
                        NewGridLine.Add(new Tile(TileType.Scaffold, RobotDirection.Right));
                        break;
                    case 'v':
                        NewGridLine.Add(new Tile(TileType.Scaffold, RobotDirection.Down));
                        break;
                    case 'X':
                        NewGridLine.Add(new Tile(TileType.Scaffold, RobotDirection.Tumble));
                        break;
                    default:
                        throw new NotImplementedException();
                }

            }
            else
            {

                if (status < 3)
                {
                    AoCUtilities.DebugWrite("{0}", (char)output);
                }
                else if (status == 3)
                {
                    Console.Write("{0}", output);
                }

                if (LastOutput == '\n' && output == '\n')
                {
                    status++;
                    if (status == 3)
                    {
                        Console.Write("Dust Collected: ");
                    }
                }
            }

            LastOutput = (char)output;
        }

        static List<List<Tile>> Grid = new List<List<Tile>>();
        static List<Tile> NewGridLine = new List<Tile>();
        static int SumAlignmentParameters = 0;

        static List<MovementAction> aMovementActions = null;
        static List<MovementAction> bMovementActions = null;
        static List<MovementAction> cMovementActions = null;
        static int status = 0;

        static void PrintGrid()
        {
#if DEBUG
            string print = "";

            for (int y = 0; y < Grid.Count(); y++)
            {
                for (int x = 0; x < Grid[y].Count; x++)
                {
                    switch (Grid[y][x].Type)
                    {
                        case TileType.Space:
                            print += "▒▒";
                            break;
                        case TileType.Scaffold:
                            switch (Grid[y][x].RobotDirection)
                            {
                                case RobotDirection.None:
                                    print += "██";
                                    break;
                                case RobotDirection.Left:
                                    print += "<<";
                                    break;
                                case RobotDirection.Up:
                                    print += "^^";
                                    break;
                                case RobotDirection.Right:
                                    print += ">>";
                                    break;
                                case RobotDirection.Down:
                                    print += "vv";
                                    break;
                                case RobotDirection.Tumble:
                                    print += "XX";
                                    break;
                            }
                            break;
                    }
                }
                print += "\n";
            }

            AoCUtilities.DebugClear();
            AoCUtilities.DebugWriteLine(print);
            //AoCUtilities.DebugReadLine();
#endif
        }

        static void Main(string[] args)
        {

            List<string> inputList = AoCUtilities.GetInput();
            string program = inputList[0];
            string[] data = program.Split(',');
            long[] memory = new long[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                memory[i] = Int64.Parse(data[i]);
            }

            IntCodeComputer computer = new IntCodeComputer(InputMethod, OutputMethod);
            memory[0] = 2;
            computer.Flash(memory);
            computer.Run();

            Console.ReadLine();
        }
    }
}
