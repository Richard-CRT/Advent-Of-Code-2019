using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _5
{
    public enum Opcode { Add, Multiply, Halt };
    public enum ParameterMode { Position, Immediate };
    public class Parameter
    {
        public ParameterMode Mode;
        public int Value;
    }

    public abstract class Instruction
    {
        public int Length;
        public Opcode Opcode;
        public List<Parameter> Parameters;

        public Instruction(Opcode _opcode, List<Parameter> _parameters)
        {
            Opcode = _opcode;
            Parameters = _parameters;
        }

        public abstract void Execute(int[] memory);
    }

    public class AddInstruction : Instruction
    {

        public AddInstruction(Opcode _opcode, List<Parameter> _parameters) : base(_opcode, _parameters)
        {
            this.Length = 4;
        }

        public override void Execute(int[] memory)
        {
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();
            string program = inputList[0];
            string[] dataString = program.Split(',');
            int[] data = new int[dataString.Length];
            for (int i = 0; i < dataString.Length; i++)
            {
                data[i] = Int32.Parse(dataString[i]);
            }

            int opcode = -1;
            int instructionPointer = 0;
            while (opcode != 99)
            {
                opcode = data[instructionPointer];
                int input1Pos;
                int input2Pos;
                int outputPos;
                switch (opcode)
                {
                    case 1: // Add
                        {
                            input1Pos = data[instructionPointer + 1];
                            input2Pos = data[instructionPointer + 2];
                            outputPos = data[instructionPointer + 3];

                            data[outputPos] = data[input1Pos] + data[input2Pos];

                            instructionPointer += 4;
                        }
                        break;
                    case 2: // Multiply
                        {
                            input1Pos = data[instructionPointer + 1];
                            input2Pos = data[instructionPointer + 2];
                            outputPos = data[instructionPointer + 3];

                            data[outputPos] = data[input1Pos] * data[input2Pos];

                            instructionPointer += 4;
                        }
                        break;
                    case 99:
                        {
                            instructionPointer += 1;
                        }
                        break;
                }
            }
        }
    }
}
