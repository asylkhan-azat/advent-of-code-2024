using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public sealed partial class Problem3 : IProblem
{
    private readonly string _path;
    private readonly TextWriter _writer;

    public Problem3(string path, TextWriter writer)
    {
        _path = path;
        _writer = writer;
    }

    public void Solve(int partNumber)
    {
        switch (partNumber)
        {
            case 1:
                SolvePartOne();
                break;
            
            case 2:
                SolvePartTwo();
                break;
        }
    }

    private void SolvePartTwo()
    {
        var machine = new Machine(ignoreSwitches: false);

        foreach (var instruction in ReadInstructionsFromFile(_path))
        {
            instruction.InvokeOn(machine);
        }

        _writer.WriteLine(machine.Total);
    }
    
    private void SolvePartOne()
    {
        var machine = new Machine(ignoreSwitches: true);

        foreach (var instruction in ReadInstructionsFromFile(_path))
        {
            instruction.InvokeOn(machine);
        }

        _writer.WriteLine(machine.Total);
    }

    [GeneratedRegex(@"((mul)\((\d+),(\d+)\))|(((do)|(don't))\(\))")]
    private static partial Regex InstructionPattern { get; }

    private static IEnumerable<IInstruction> ReadInstructionsFromFile(string path)
    {
        var input = File.ReadAllText(path);

        foreach (Match match in InstructionPattern.Matches(input))
        {
            if (match.Groups[2].ValueSpan is "mul")
            {
                yield return new Mul(
                    int.Parse(match.Groups[3].ValueSpan),
                    int.Parse(match.Groups[4].ValueSpan));
            }
            else switch (match.Groups[6].ValueSpan)
            {
                case "do":
                    yield return Switch.Enable;
                    break;
                case "don't":
                    yield return Switch.Disable;
                    break;
            }
        }
    }

    private sealed class Machine
    {
        private readonly bool _ignoreSwitches;
        
        public Machine(bool ignoreSwitches)
        {
            _ignoreSwitches = ignoreSwitches;
        }
        
        public int Total { get; private set; }

        private bool _disabled;

        public void VisitMul(Mul mul)
        {
            if (_disabled) return;
            Total += mul.LhsOperand * mul.RhsOperand;
        }

        public void VisitSwitch(Switch @switch)
        {
            if (_ignoreSwitches) return;
            _disabled = @switch.Disabled;
        }
    }

    private interface IInstruction
    {
        void InvokeOn(Machine machine);
    }

    private sealed record Switch(bool Disabled) : IInstruction
    {
        public static readonly Switch Enable = new(Disabled: false);
        public static readonly Switch Disable = new(Disabled: true);
        
        public void InvokeOn(Machine machine)
        {
            machine.VisitSwitch(this);
        }
    }

    private sealed record Mul(int LhsOperand, int RhsOperand) : IInstruction
    {
        public void InvokeOn(Machine machine)
        {
            machine.VisitMul(this);
        }
    }
}