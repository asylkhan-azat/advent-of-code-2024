using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace AdventOfCode2024;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public sealed class Problem5 : IProblem
{
    private readonly string _path;
    private readonly TextWriter _textWriter;

    public Problem5(string path, TextWriter textWriter)
    {
        _path = path;
        _textWriter = textWriter;
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
        var (rules, updates) = ReadRulesAndUpdatesFromFile(_path);

        var total = 0;

        foreach (var update in updates)
        {
            if (update.IsCorrectlyOrderedAccordingTo(rules))
            {
                total += update.MiddleNumber;
            }
        }

        _textWriter.WriteLine(total);
    }

    private void SolvePartTwo()
    {
        var (rules, updates) = ReadRulesAndUpdatesFromFile(_path);

        var total = 0;

        foreach (var update in updates)
        {
            if (!update.IsCorrectlyOrderedAccordingTo(rules))
            {
                update.ReorderAccordingTo(rules);
                total += update.MiddleNumber;
            }
        }

        _textWriter.WriteLine(total);
    }

    private readonly struct Update
    {
        private readonly int[] _values;

        private Update(int[] values)
        {
            _values = values;
        }

        public int MiddleNumber => _values[_values.Length / 2];

        public bool IsCorrectlyOrderedAccordingTo(OrderingRules rules)
        {
            for (var i = 0; i < _values.Length - 1; i++)
            {
                if (!rules.IsBefore(_values[i], _values[i + 1]))
                {
                    return false;
                }
            }

            return true;
        }

        public void ReorderAccordingTo(OrderingRules rules)
        {
            var comparer = Comparer<int>.Create((lhs, rhs) => rules.IsBefore(lhs, rhs) ? -1 : 1);
            Array.Sort(_values, comparer);
        }

        public static Update FromLine(string line)
        {
            return new Update(line.Split(',').Select(int.Parse).ToArray());
        }
    }

    private readonly struct OrderingRules
    {
        private readonly Dictionary<int, HashSet<int>> _rules = [];

        public OrderingRules()
        {
        }

        public void AddRuleFromLine(ReadOnlySpan<char> line)
        {
            var index = line.IndexOf('|');
            AddRule(int.Parse(line[..index]), int.Parse(line[(index + 1)..]));
        }

        private void AddRule(int value, int valueAfter)
        {
            ref var set = ref CollectionsMarshal.GetValueRefOrAddDefault(
                _rules,
                value,
                out var exists);

            if (!exists) set = [];

            Debug.Assert(set is not null);
            set.Add(valueAfter);
        }

        public bool IsBefore(int value, int valueAfter)
        {
            return _rules.TryGetValue(value, out var afterValues) && afterValues.Contains(valueAfter);
        }
    }

    private static (OrderingRules, Update[]) ReadRulesAndUpdatesFromFile(string path)
    {
        var lines = File.ReadAllLines(path);

        var indexOfEmpty = Array.FindIndex(lines, line => line is "");

        var rules = new OrderingRules();

        for (var i = 0; i < indexOfEmpty; i++)
        {
            rules.AddRuleFromLine(lines[i]);
        }

        var updates = new Update[lines.Length - indexOfEmpty - 1];

        for (var i = indexOfEmpty + 1; i < lines.Length; i++)
        {
            updates[i - indexOfEmpty - 1] = Update.FromLine(lines[i]);
        }

        return (rules, updates);
    }
}