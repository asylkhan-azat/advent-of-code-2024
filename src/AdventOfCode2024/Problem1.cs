namespace AdventOfCode2024;

public sealed class Problem1 : IProblem
{
    private readonly string _path;
    private readonly TextWriter _writer;

    public Problem1(string path, TextWriter writer)
    {
        _path = path;
        _writer = writer;
    }

    public void Solve()
    {
        var lhs = new PriorityQueue<int, int>();
        var rhs = new PriorityQueue<int, int>();
        
        foreach (var pair in InputPair.FromFile(_path))
        {
            lhs.Enqueue(pair.Lhs, pair.Lhs);
            rhs.Enqueue(pair.Rhs, pair.Rhs);
        }

        var distance = 0;

        while (lhs.TryDequeue(out var leftElement, out _) && rhs.TryDequeue(out var rightElement, out _))
        {
            distance += Math.Abs(leftElement - rightElement);
        }

        _writer.WriteLine(distance);
    }
    
    internal readonly record struct InputPair(int Lhs, int Rhs)
    {
        public static IEnumerable<InputPair> FromFile(string path)
        {
            using var fs = File.OpenRead(path);
            using var reader = new StreamReader(fs);

            while (reader.ReadLine() is { } line)
            {
                var index = line.IndexOf(' ');

                yield return new InputPair(
                    int.Parse(line.AsSpan(0, index)),
                    int.Parse(line.AsSpan(index + 1)));
            }
        }
    }
}