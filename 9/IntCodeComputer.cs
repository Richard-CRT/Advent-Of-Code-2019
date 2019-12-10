using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntCodeComputerNS
{
    public delegate long InputDelegate();
    public delegate void OutputDelegate(long output);

    class IntCodeComputer
    {
        public enum Opcode { Add, Multiply, Halt };
        public enum ParameterMode { Position, Relative, Immediate };
        public class Parameter
        {
            private ParameterMode Mode;
            private long Value;

            public Parameter(ParameterMode parameterMode, long value)
            {
                this.Mode = parameterMode;
                this.Value = value;
            }

            public long EvaluateParameter(State currentState)
            {
                long evaluation = 0;

                switch (this.Mode)
                {
                    case ParameterMode.Immediate:
                        evaluation = this.Value;
                        break;
                    case ParameterMode.Position:
                        evaluation = currentState.Memory[this.Value];
                        break;
                    case ParameterMode.Relative:
                        evaluation = currentState.Memory[this.Value + currentState.RelativeBase];
                        break;
                }

                return evaluation;
            }

            public void WriteParameter(State currentState, long data)
            {
                switch (this.Mode)
                {
                    case ParameterMode.Position:
                        currentState.Memory[this.Value] = data;
                        break;
                    case ParameterMode.Relative:
                        currentState.Memory[this.Value + currentState.RelativeBase] = data;
                        break;
                    case ParameterMode.Immediate:
                        throw new ArgumentException("Parameters that an instruction writes to will never be in immediate mode.");
                }
            }
        }

        public abstract class Instruction
        {
            public int Length;
            public List<Parameter> Parameters = new List<Parameter>();

            public abstract bool Execute(ref bool instructionPointerChanged, State currentState);
        }

        public class AddInstruction : Instruction
        {
            public AddInstruction() : base()
            {
                this.Length = 4;
            }

            public override bool Execute(ref bool instructionPointerChanged, State currentState)
            {
                long a = this.Parameters[0].EvaluateParameter(currentState);
                long b = this.Parameters[1].EvaluateParameter(currentState);
                this.Parameters[2].WriteParameter(currentState, a + b);
                return true;
            }
        }

        public class MultiplyInstruction : Instruction
        {
            public MultiplyInstruction() : base()
            {
                this.Length = 4;
            }

            public override bool Execute(ref bool instructionPointerChanged, State currentState)
            {
                long a = this.Parameters[0].EvaluateParameter(currentState);
                long b = this.Parameters[1].EvaluateParameter(currentState);
                this.Parameters[2].WriteParameter(currentState, a * b);
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

            public override bool Execute(ref bool instructionPointerChanged, State currentState)
            {
                long input = InputCallback();
                this.Parameters[0].WriteParameter(currentState, input);
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

            public override bool Execute(ref bool instructionPointerChanged, State currentState)
            {
                long a = this.Parameters[0].EvaluateParameter(currentState);
                OutputCallback(a);
                return true;
            }
        }

        public class JumpIfTrueInstruction : Instruction
        {
            public JumpIfTrueInstruction() : base()
            {
                this.Length = 3;
            }

            public override bool Execute(ref bool instructionPointerChanged, State currentState)
            {
                long a = this.Parameters[0].EvaluateParameter(currentState);
                if (a != 0)
                {
                    long dest = this.Parameters[1].EvaluateParameter(currentState);
                    currentState.InstructionPointer = dest;
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

            public override bool Execute(ref bool instructionPointerChanged, State currentState)
            {
                long a = this.Parameters[0].EvaluateParameter(currentState);
                if (a == 0)
                {
                    long dest = this.Parameters[1].EvaluateParameter(currentState);
                    currentState.InstructionPointer = dest;
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

            public override bool Execute(ref bool instructionPointerChanged, State currentState)
            {
                long a = this.Parameters[0].EvaluateParameter(currentState);
                long b = this.Parameters[1].EvaluateParameter(currentState);
                if (a < b)
                    this.Parameters[2].WriteParameter(currentState, 1);
                else
                    this.Parameters[2].WriteParameter(currentState, 0);
                return true;
            }
        }

        public class EqualsInstruction : Instruction
        {
            public EqualsInstruction() : base()
            {
                this.Length = 4;
            }

            public override bool Execute(ref bool instructionPointerChanged, State currentState)
            {
                long a = this.Parameters[0].EvaluateParameter(currentState);
                long b = this.Parameters[1].EvaluateParameter(currentState);
                if (a == b)
                    this.Parameters[2].WriteParameter(currentState, 1);
                else
                    this.Parameters[2].WriteParameter(currentState, 0);
                return true;
            }
        }
        public class AdjustRelativeBaseInstruction : Instruction
        {
            public AdjustRelativeBaseInstruction() : base()
            {
                this.Length = 2;
            }

            public override bool Execute(ref bool instructionPointerChanged, State currentState)
            {
                long a = this.Parameters[0].EvaluateParameter(currentState);
                currentState.RelativeBase += a;
                return true;
            }
        }

        public class HaltInstruction : Instruction
        {
            public HaltInstruction() : base()
            {
                this.Length = 1;
            }

            public override bool Execute(ref bool instructionPointerChanged, State currentState)
            {
                return false;
            }
        }

        public class State
        {
            public long[] Memory;
            public long InstructionPointer = 0;
            public long RelativeBase = 0;
        }

        private State CurrentState;
        private InputDelegate InputCallback;
        private OutputDelegate OutputCallback;

        public IntCodeComputer(InputDelegate inputCallback, OutputDelegate outputCallback)
        {
            CurrentState = new State();

            this.CurrentState.Memory = new long[] { 99 };
            this.InputCallback = inputCallback;
            this.OutputCallback = outputCallback;
        }

        public void Reset()
        {
            this.CurrentState.InstructionPointer = 0;
            this.CurrentState.RelativeBase = 0;
        }

        public void Flash(long[] memory)
        {
            this.CurrentState.Memory = new long[Int16.MaxValue];
            Array.Clear(this.CurrentState.Memory, 0, Int16.MaxValue);
            Array.Copy(memory, 0, this.CurrentState.Memory, 0, memory.Length);
            Reset();
        }

        public Instruction FetchInstruction()
        {
            Instruction instruction;

            string complexOpcodeString = CurrentState.Memory[CurrentState.InstructionPointer].ToString().PadLeft(2, '0');
            int opcode = Int32.Parse(complexOpcodeString.Substring(complexOpcodeString.Length - 2, 2));

            switch (opcode)
            {
                case 1:
                    instruction = new AddInstruction();
                    break;
                case 2:
                    instruction = new MultiplyInstruction();
                    break;
                case 3:
                    instruction = new InputInstruction(InputCallback);
                    break;
                case 4:
                    instruction = new OutputInstruction(OutputCallback);
                    break;
                case 5:
                    instruction = new JumpIfTrueInstruction();
                    break;
                case 6:
                    instruction = new JumpIfFalseInstruction();
                    break;
                case 7:
                    instruction = new LessThanInstruction();
                    break;
                case 8:
                    instruction = new EqualsInstruction();
                    break;
                case 9:
                    instruction = new AdjustRelativeBaseInstruction();
                    break;
                case 99:
                    instruction = new HaltInstruction();
                    break;
                default:
                    throw new ArgumentException("Unknown opcode");
            }

            complexOpcodeString = complexOpcodeString.PadLeft(instruction.Length + 1, '0');

            for (int i = 1; i < instruction.Length; i++)
            {
                long value = CurrentState.Memory[CurrentState.InstructionPointer + i];
                ParameterMode mode;

                switch (complexOpcodeString[instruction.Length - (1 + i)])
                {
                    case '0':
                        mode = ParameterMode.Position;
                        break;
                    case '1':
                        mode = ParameterMode.Immediate;
                        break;
                    case '2':
                        mode = ParameterMode.Relative;
                        break;
                    default:
                        throw new ArgumentException("None of the parameter modes match");
                }

                Parameter parameter = new Parameter(mode, value);
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
                CurrentState.InstructionPointer += instruction.Length;
            }

            return instruction.Execute(ref instructionPointerChanged, CurrentState);
        }

        public void Run()
        {
            for (CurrentState.InstructionPointer = 0; CurrentState.InstructionPointer < CurrentState.Memory.Length;)
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
