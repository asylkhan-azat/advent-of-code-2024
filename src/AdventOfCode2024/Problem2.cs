namespace AdventOfCode2024;

public sealed class Problem2 : IProblem
{
    private readonly string _path;
    private readonly TextWriter _writer;

    public Problem2(string path, TextWriter writer)
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
        var count = 0;

        foreach (var report in Report.FromFile(_path))
        {
            if (report.IsSafeUsingDampener)
            {
                count++;
            }
        }

        _writer.WriteLine(count);
    }
    
    private void SolvePartOne()
    {
        var count = 0;

        foreach (var report in Report.FromFile(_path))
        {
            if (report.IsSafe)
            {
                count++;
            }
        }

        _writer.WriteLine(count);
    }

    private readonly record struct Report(int[] Levels)
    {
        public bool IsSafeUsingDampener
        {
            get
            {
                if (IsSafe) return true;

                for (var i = 0; i < Levels.Length; i++)
                {
                    var reportWithoutLevel = Levels.ToList();
                    reportWithoutLevel.RemoveAt(i);

                    if (CheckIfSafe(reportWithoutLevel))
                    {
                        return true;
                    }
                }
                
                return false;
            }
        }
        
        public bool IsSafe => CheckIfSafe(Levels);

        private static bool CheckIfSafe(IReadOnlyList<int> levels)
        {
            var previousLevel = levels[0];
            var previousSign = new int?();

            for (var i = 1; i < levels.Count; i++)
            {
                var level = levels[i];
                var sign = Math.Sign(level - previousLevel);

                previousSign ??= sign;
                if (previousSign != sign)
                {
                    return false;
                }

                var diff = Math.Abs(level - previousLevel);

                if (diff is < 1 or > 3)
                {
                    return false;
                }

                previousSign = sign;
                previousLevel = level;
            }

            return true;
        }

        public static IEnumerable<Report> FromFile(string path)
        {
            using var fs = File.OpenRead(path);
            using var reader = new StreamReader(fs);

            while (reader.ReadLine() is { } line)
            {
                var levels = line
                    .Split(' ')
                    .Select(int.Parse)
                    .ToArray();

                yield return new Report(levels);
            }
        }
    }
}