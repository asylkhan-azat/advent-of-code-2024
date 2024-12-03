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
        }
    }

    private void SolvePartOne()
    {
        var total = 0;

        foreach (var operation in MulOperation.FromFile(_path))
        {
            total += operation.LhsOperand * operation.RhsOperand;
        }
        
        _writer.WriteLine(total);
    }
    
    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    private static partial Regex MulPattern { get; }

    private readonly record struct MulOperation(int LhsOperand, int RhsOperand)
    {
        public static IEnumerable<MulOperation> FromFile(string path)
        {
            var input = File.ReadAllText(path);

            foreach (Match match in MulPattern.Matches(input))
            {
                yield return new MulOperation(
                    int.Parse(match.Groups[1].ValueSpan),
                    int.Parse(match.Groups[2].ValueSpan));
            }
        }
    }
}