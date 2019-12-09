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

        public int EvaluateParameter(string[] memory)
        {
            int evaluation = 0;

            switch (this.Mode)
            {
                case ParameterMode.Immediate:
                    evaluation = this.Value;
                    break;
                case ParameterMode.Position:
                    evaluation = Int32.Parse(memory[this.Value]);
                    break;
            }

            return evaluation;
        }
    }

    public abstract class Instruction
    {
        public int Length;
        public List<Parameter> Parameters = new List<Parameter>();

        public abstract bool Execute(ref int instructionPointer, ref bool instructionPointerChanged, string[] memory);
    }

    public class AddInstruction : Instruction
    {
        public AddInstruction() : base()
        {
            this.Length = 4;
        }

        public override bool Execute(ref int instructionPointer, ref bool instructionPointerChanged, string[] memory)
        {
            if (this.Parameters[2].Mode == ParameterMode.Immediate)
                throw new ArgumentException("Parameters that an instruction writes to will never be in immediate mode.");
            int a = this.Parameters[0].EvaluateParameter(memory);
            int b = this.Parameters[1].EvaluateParameter(memory);
            int dest = this.Parameters[2].Value;
            memory[dest] = (a + b).ToString();
            return true;
        }
    }

    public class MultiplyInstruction : Instruction
    {
        public MultiplyInstruction() : base()
        {
            this.Length = 4;
        }

        public override bool Execute(ref int instructionPointer, ref bool instructionPointerChanged, string[] memory)
        {
            if (this.Parameters[2].Mode == ParameterMode.Immediate)
                throw new ArgumentException("Parameters that an instruction writes to will never be in immediate mode.");
            int a = this.Parameters[0].EvaluateParameter(memory);
            int b = this.Parameters[1].EvaluateParameter(memory);
            int dest = this.Parameters[2].Value;
            memory[dest] = (a * b).ToString();
            return true;
        }
    }

    public class InputInstruction : Instruction
    {
        public InputInstruction() : base()
        {
            this.Length = 2;
        }

        public override bool Execute(ref int instructionPointer, ref bool instructionPointerChanged, string[] memory)
        {
            if (this.Parameters[0].Mode == ParameterMode.Immediate)
                throw new ArgumentException("Parameters that an instruction writes to will never be in immediate mode.");
            Console.Write("Input: ");
            string input = Console.ReadLine();
            int dest = this.Parameters[0].Value;
            memory[dest] = input;
            return true;
        }
    }

    public class OutputInstruction : Instruction
    {
        public OutputInstruction() : base()
        {
            this.Length = 2;
        }

        public override bool Execute(ref int instructionPointer, ref bool instructionPointerChanged, string[] memory)
        {
            int a = this.Parameters[0].Value;
            Console.WriteLine("Output: {0}", memory[a]);
            return true;
        }
    }

    public class JumpIfTrueInstruction : Instruction
    {
        public JumpIfTrueInstruction() : base()
        {
            this.Length = 3;
        }

        public override bool Execute(ref int instructionPointer, ref bool instructionPointerChanged, string[] memory)
        {
            int a = this.Parameters[0].EvaluateParameter(memory);
            if (a != 0)
            {
                int dest = this.Parameters[1].EvaluateParameter(memory);
                instructionPointer = dest;
                instructionPointerChanged = true;
            }
            return true;
        }
    }

    public class JumpIfFalseInstruction : Instruction
    {
        public JumpIfFalseInstruction() : base()
        {
            this.Length = 3;
        }

        public override bool Execute(ref int instructionPointer, ref bool instructionPointerChanged, string[] memory)
        {
            int a = this.Parameters[0].EvaluateParameter(memory);
            if (a == 0)
            {
                int dest = this.Parameters[1].EvaluateParameter(memory);
                instructionPointer = dest;
                instructionPointerChanged = true;
            }
            return true;
        }
    }

    public class LessThanInstruction : Instruction
    {
        public LessThanInstruction() : base()
        {
            this.Length = 4;
            this.Parameters = new List<Parameter>();
        }

        public override bool Execute(ref int instructionPointer, ref bool instructionPointerChanged, string[] memory)
        {
            if (this.Parameters[2].Mode == ParameterMode.Immediate)
                throw new ArgumentException("Parameters that an instruction writes to will never be in immediate mode.");
            int a = this.Parameters[0].EvaluateParameter(memory);
            int b = this.Parameters[1].EvaluateParameter(memory);
            int dest = this.Parameters[2].Value;
            if (a < b)
                memory[dest] = "1";
            else
                memory[dest] = "0";
            return true;
        }
    }

    public class EqualsInstruction : Instruction
    {
        public EqualsInstruction() : base()
        {
            this.Length = 4;
        }

        public override bool Execute(ref int instructionPointer, ref bool instructionPointerChanged, string[] memory)
        {
            if (this.Parameters[2].Mode == ParameterMode.Immediate)
                throw new ArgumentException("Parameters that an instruction writes to will never be in immediate mode.");
            int a = this.Parameters[0].EvaluateParameter(memory);
            int b = this.Parameters[1].EvaluateParameter(memory);
            int dest = this.Parameters[2].Value;
            if (a == b)
                memory[dest] = "1";
            else
                memory[dest] = "0";
            return true;
        }
    }

    public class HaltInstruction : Instruction
    {
        public HaltInstruction() : base()
        {
            this.Length = 1;
        }

        public override bool Execute(ref int instructionPointer, ref bool instructionPointerChanged, string[] memory)
        {
            return false;
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();
            string program = inputList[0];
            string[] memory = program.Split(',');

            for (int instructionPointer = 0; instructionPointer < memory.Length;)
            {
                Instruction instruction;

                string complexOpcodeString = memory[instructionPointer].ToString().PadLeft(2, '0');
                string opcodeString = complexOpcodeString.Substring(complexOpcodeString.Length - 2, 2);

                switch (opcodeString)
                {
                    case "01":
                        instruction = new AddInstruction();
                        break;
                    case "02":
                        instruction = new MultiplyInstruction();
                        break;
                    case "03":
                        instruction = new InputInstruction();
                        break;
                    case "04":
                        instruction = new OutputInstruction();
                        break;
                    case "05":
                        instruction = new JumpIfTrueInstruction();
                        break;
                    case "06":
                        instruction = new JumpIfFalseInstruction();
                        break;
                    case "07":
                        instruction = new LessThanInstruction();
                        break;
                    case "08":
                        instruction = new EqualsInstruction();
                        break;
                    case "99":
                        instruction = new HaltInstruction();
                        break;
                    default:
                        throw new ArgumentException("Unknown opcode");
                }

                complexOpcodeString = complexOpcodeString.PadLeft(instruction.Length + 1, '0');

                for (int i = 1; i < instruction.Length; i++)
                {
                    Parameter parameter = new Parameter();
                    parameter.Value = Int32.Parse(memory[instructionPointer + i]);
                    switch (complexOpcodeString[instruction.Length - (1 + i)])
                    {
                        case '0':
                            parameter.Mode = ParameterMode.Position;
                            break;
                        case '1':
                            parameter.Mode = ParameterMode.Immediate;
                            break;
                    }
                    instruction.Parameters.Add(parameter);
                }

                // execute instruction
                bool instructionPointerChanged = false;
                if (!instruction.Execute(ref instructionPointer, ref instructionPointerChanged, memory))
                {
                    break;
                }

                if (!instructionPointerChanged)
                {
                    instructionPointer += instruction.Length;
                }
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
