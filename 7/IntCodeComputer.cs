using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntCodeComputerNS
{
    public delegate int InputDelegate();
    public delegate void OutputDelegate(int output);

    class IntCodeComputer
    {
        public enum Opcode { Add, Multiply, Halt };
        public enum ParameterMode { Position, Immediate };
        public class Parameter
        {
            public ParameterMode Mode;
            public int Value;

            public int EvaluateParameter(int[] memory)
            {
                int evaluation = 0;

                switch (this.Mode)
                {
                    case ParameterMode.Immediate:
                        evaluation = this.Value;
                        break;
                    case ParameterMode.Position:
                        evaluation = memory[this.Value];
                        break;
                }

                return evaluation;
            }
        }

        public abstract class Instruction
        {
            public int Length;
            public List<Parameter> Parameters = new List<Parameter>();

            public abstract bool Execute(ref int instructionPointer, ref bool instructionPointerChanged, int[] memory);
        }

        public class AddInstruction : Instruction
        {
            public AddInstruction() : base()
            {
                this.Length = 4;
            }

            public override bool Execute(ref int instructionPointer, ref bool instructionPointerChanged, int[] memory)
            {
                if (this.Parameters[2].Mode == ParameterMode.Immediate)
                    throw new ArgumentException("Parameters that an instruction writes to will never be in immediate mode.");
                int a = this.Parameters[0].EvaluateParameter(memory);
                int b = this.Parameters[1].EvaluateParameter(memory);
                int dest = this.Parameters[2].Value;
                memory[dest] = a + b;
                return true;
            }
        }

        public class MultiplyInstruction : Instruction
        {
            public MultiplyInstruction() : base()
            {
                this.Length = 4;
            }

            public override bool Execute(ref int instructionPointer, ref bool instructionPointerChanged, int[] memory)
            {
                if (this.Parameters[2].Mode == ParameterMode.Immediate)
                    throw new ArgumentException("Parameters that an instruction writes to will never be in immediate mode.");
                int a = this.Parameters[0].EvaluateParameter(memory);
                int b = this.Parameters[1].EvaluateParameter(memory);
                int dest = this.Parameters[2].Value;
                memory[dest] = a * b;
                return true;
            }
        }

        public class InputInstruction : Instruction
        {
            private InputDelegate InputCallback;

            public InputInstruction(InputDelegate inputCallback) : base()
            {
                this.Length = 2;
                this.InputCallback = inputCallback;
            }

            public override bool Execute(ref int instructionPointer, ref bool instructionPointerChanged, int[] memory)
            {
                if (this.Parameters[0].Mode == ParameterMode.Immediate)
                    throw new ArgumentException("Parameters that an instruction writes to will never be in immediate mode.");
                int input = InputCallback();
                int dest = this.Parameters[0].Value;
                memory[dest] = input;
                return true;
            }
        }

        public class OutputInstruction : Instruction
        {
            private OutputDelegate OutputCallback;

            public OutputInstruction(OutputDelegate outputCallback) : base()
            {
                this.Length = 2;
                this.OutputCallback = outputCallback;
            }

            public override bool Execute(ref int instructionPointer, ref bool instructionPointerChanged, int[] memory)
            {
                int a = this.Parameters[0].Value;
                OutputCallback(memory[a]);
                return true;
            }
        }

        public class JumpIfTrueInstruction : Instruction
        {
            public JumpIfTrueInstruction() : base()
            {
                this.Length = 3;
            }

            public override bool Execute(ref int instructionPointer, ref bool instructionPointerChanged, int[] memory)
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

            public override bool Execute(ref int instructionPointer, ref bool instructionPointerChanged, int[] memory)
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

            public override bool Execute(ref int instructionPointer, ref bool instructionPointerChanged, int[] memory)
            {
                if (this.Parameters[2].Mode == ParameterMode.Immediate)
                    throw new ArgumentException("Parameters that an instruction writes to will never be in immediate mode.");
                int a = this.Parameters[0].EvaluateParameter(memory);
                int b = this.Parameters[1].EvaluateParameter(memory);
                int dest = this.Parameters[2].Value;
                if (a < b)
                    memory[dest] = 1;
                else
                    memory[dest] = 0;
                return true;
            }
        }

        public class EqualsInstruction : Instruction
        {
            public EqualsInstruction() : base()
            {
                this.Length = 4;
            }

            public override bool Execute(ref int instructionPointer, ref bool instructionPointerChanged, int[] memory)
            {
                if (this.Parameters[2].Mode == ParameterMode.Immediate)
                    throw new ArgumentException("Parameters that an instruction writes to will never be in immediate mode.");
                int a = this.Parameters[0].EvaluateParameter(memory);
                int b = this.Parameters[1].EvaluateParameter(memory);
                int dest = this.Parameters[2].Value;
                if (a == b)
                    memory[dest] = 1;
                else
                    memory[dest] = 0;
                return true;
            }
        }

        public class HaltInstruction : Instruction
        {
            public HaltInstruction() : base()
            {
                this.Length = 1;
            }

            public override bool Execute(ref int instructionPointer, ref bool instructionPointerChanged, int[] memory)
            {
                return false;
            }
        }

        private int[] Memory;
        private InputDelegate InputCallback;
        private OutputDelegate OutputCallback;
        private int InstructionPointer = 0;

        public IntCodeComputer(InputDelegate inputCallback, OutputDelegate outputCallback)
        {
            this.Memory = new int[] { 99 };
            this.InputCallback = inputCallback;
            this.OutputCallback = outputCallback;
        }

        public void Flash(int[] memory)
        {
            this.InstructionPointer = 0;
            this.Memory = new int[memory.Length];
            Array.Copy(memory, 0, this.Memory, 0, memory.Length);
        }

        public Instruction FetchInstruction()
        {
            Instruction instruction;

            string complexOpcodeString = Memory[InstructionPointer].ToString().PadLeft(2, '0');
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
                    instruction = new InputInstruction(InputCallback);
                    break;
                case "04":
                    instruction = new OutputInstruction(OutputCallback);
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
                parameter.Value = Memory[InstructionPointer + i];
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

            return instruction;
        }

        public bool ExecuteInstruction(Instruction instruction)
        {
            // execute instruction
            bool instructionPointerChanged = false;
            if (!instructionPointerChanged)
            {
                InstructionPointer += instruction.Length;
            }

            return instruction.Execute(ref InstructionPointer, ref instructionPointerChanged, Memory);
        }

        public void Run()
        {
            for (InstructionPointer = 0; InstructionPointer < Memory.Length;)
            {
                Instruction instruction = FetchInstruction();
                if (!ExecuteInstruction(instruction))
                {
                    break;
                }
            }
        }

        public bool RunUntilInput()
        {
            Instruction instruction = FetchInstruction();
            bool firstInstruction = true;
            while (firstInstruction || !(instruction is InputInstruction))
            {
                firstInstruction = false;

                if (!ExecuteInstruction(instruction))
                {
                    return false;
                }
                instruction = FetchInstruction();
            }

            return true;
        }
    }
}
