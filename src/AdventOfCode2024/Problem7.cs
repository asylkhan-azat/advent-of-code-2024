using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2024;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public sealed class Problem7 : IProblem
{
    private readonly string _path;
    private readonly TextWriter _writer;

    public Problem7(string path, TextWriter writer)
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

    private void SolvePartOne()
    {
        var total = 0L;

        foreach (var line in Line.FromFile(_path))
        {
            if (line.IsSolvableByUsing(OperatorsPartOne))
            {
                total += line.TestValue;
            }
        }

        _writer.WriteLine(total);
    }
    
    private void SolvePartTwo()
    {
        var total = 0L;

        foreach (var line in Line.FromFile(_path))
        {
            if (line.IsSolvableByUsing(OperatorsPartTwo))
            {
                total += line.TestValue;
            }
        }

        _writer.WriteLine(total);
    }

    private static readonly Func<long, long, long>[] OperatorsPartOne =
    [
        static (a, b) => a + b,
        static (a, b) => a * b
    ];

    private static readonly Func<long, long, long>[] OperatorsPartTwo =
    [
        ..OperatorsPartOne,
        static (a, b) => (long)Math.Pow(10, (long)(Math.Log10(b) + 1)) * a + b
    ];

    private readonly record struct Line(long TestValue, long[] Numbers)
    {
        public bool IsSolvableByUsing(Func<long, long, long>[] operators)
        {
            return IsSolvableByUsing(Numbers[0], 1, operators);
        }

        private bool IsSolvableByUsing(
            long currentValue,
            int index,
            Func<long, long, long>[] operators)
        {
            if (index == Numbers.Length)
            {
                return currentValue == TestValue;
            }

            foreach (var @operator in operators)
            {
                var value = @operator(currentValue, Numbers[index]);

                if (IsSolvableByUsing(value, index + 1, operators))
                {
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<Line> FromFile(string path)
        {
            foreach (var line in File.ReadLines(path))
            {
                yield return Parse(line);
            }
        }

        private static Line Parse(string line)
        {
            var index = line.IndexOf(':');

            var numbers = new List<long>();
            var span = line.AsSpan(index + 2);

            foreach (var range in span.Split(' '))
            {
                var number = span[range];
                if (number.IsEmpty) continue;
                numbers.Add(long.Parse(number));
            }

            return new Line(
                long.Parse(line.AsSpan(0, index)),
                numbers.ToArray());
        }
    }
}