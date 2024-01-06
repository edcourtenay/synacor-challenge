namespace SynacorChallenge;

public class VirtualMachine
{
    private readonly Memory<ushort> _memory;
    private readonly Memory<ushort> _registers;

    private ushort ProgramCounter { get; set; }
    private Stack<ushort> Stack { get; } = new();
    private bool _halted;

    public VirtualMachine(Memory<ushort> memory, Memory<ushort> registers)
    {
        _memory = memory;
        _registers = registers;
    }

    public void Execute()
    {
        while (!_halted)
        {
            ExecuteInstruction(Value());
        }
    }

    private ushort Value()
    {
        var value = _memory.Span[ProgramCounter++];

        return value switch
        {
            <= 0x7FFF => value,
            <= 0x8007 => _registers.Span[value - 0x8000],
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    private ushort Register()
    {
        var value = _memory.Span[ProgramCounter++];

        return value switch
        {
            >= 0x8000 and <= 0x8007 => (ushort)(value - 0x8000),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void ExecuteInstruction(ushort instruction)
    {
        switch (instruction)
        {
            case Constants.HaltInstruction:
                Halt();
                return;
            case Constants.SetInstruction:
                Set(Register(), Value());
                return;
            case Constants.PushInstruction:
                Push(Value());
                return;
            case Constants.PopInstruction:
                Pop(Register());
                return;
            case Constants.EqInstruction:
                Calculate(Register(), Value(), Value(), Equals);
                return;
            case Constants.GtInstruction:
                Calculate(Register(), Value(), Value(), GreaterThan);
                return;
            case Constants.JmpInstruction:
                Jump(Value());
                return;
            case Constants.JtInstruction:
                JumpIf(Value(), Value(), value => value != 0);
                return;
            case Constants.JfInstruction:
                JumpIf(Value(), Value(), value => value == 0);
                return;
            case Constants.AddInstruction:
                Calculate(Register(), Value(), Value(), Add);
                return;
            case Constants.MultInstruction:
                Calculate(Register(), Value(), Value(), Multiply);
                return;
            case Constants.ModInstruction:
                Calculate(Register(), Value(), Value(), Mod);
                return;
            case Constants.AndInstruction:
                Calculate(Register(), Value(), Value(), And);
                return;
            case Constants.OrInstruction:
                Calculate(Register(), Value(), Value(), Or);
                return;
            case Constants.NotInstruction:
                Calculate(Register(), Value(), 0x7FFF, Xor);
                return;
            case Constants.RmemInstruction:
                ReadMemory(Register(), Value());
                return;
            case Constants.WmemInstruction:
                WriteMemory(Value(), Value());
                return;
            case Constants.CallInstruction:
                Call(Value());
                return;
            case Constants.RetInstruction:
                Return();
                return;
            case Constants.OutInstruction:
                Out(Value());
                return;
            case Constants.InInstruction:
                In(Register());
                return;
            case Constants.NoopInstruction:
                // Do nothing
                return;
            default:
                throw new ArgumentOutOfRangeException(nameof(instruction), instruction, null);
        }
    }

    private static ushort Equals(ushort a, ushort b)
        => a == b ? (ushort)1 : (ushort)0;

    private static ushort GreaterThan(ushort a, ushort b)
        => a > b ? (ushort)1 : (ushort)0;

    private static ushort Add(ushort a, ushort b)
        => (ushort)(a + b);

    private static ushort Multiply(ushort a, ushort b)
        => (ushort)(a * b);

    private static ushort Mod(ushort a, ushort b)
        => (ushort)(a % b);

    private static ushort And(ushort a, ushort b)
        => (ushort)(a & b);

    private static ushort Or(ushort a, ushort b)
        => (ushort)(a | b);

    private static ushort Xor(ushort a, ushort b)
        => (ushort)(a ^ b);

    private void Calculate(ushort register, ushort value1, ushort value2, Func<ushort, ushort, ushort> func)
    {
        _registers.Span[register] = (ushort)(func(value1, value2) % 0x8000);
    }

    private void Halt()
    {
        _halted = true;
    }

    private void Set(ushort register, ushort value)
    {
        _registers.Span[register] = value;
    }

    private void Push(ushort value)
    {
        Stack.Push(value);
    }

    private void Pop(ushort register)
    {
        _registers.Span[register] = Stack.Pop();
    }

    private void Jump(ushort address)
    {
        ProgramCounter = address;
    }

    private void JumpIf(ushort value, ushort address, Func<ushort, bool> func)
    {
        if (func(value))
        {
            ProgramCounter = address;
        }
    }

    private void Call(ushort address)
    {
        Stack.Push(ProgramCounter);
        ProgramCounter = address;
    }

    private void ReadMemory(ushort register, ushort value)
    {
        _registers.Span[register] = _memory.Span[value];
    }

    private void WriteMemory(ushort address, ushort value)
    {
        _memory.Span[address] = value;
    }

    private void Return()
    {
        ProgramCounter = Stack.Pop();
    }

    private static void Out(ushort value)
    {
        Console.Write((char)value);
    }

    private void In(ushort register)
    {
        while (true)
        {
            var ch = Console.Read();
            if (ch is 13 or -1)
            {
                continue;
            }

            _registers.Span[register] = (ushort)ch;
            break;
        }
    }
}