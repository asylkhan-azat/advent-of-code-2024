using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2024;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public sealed class Problem4 : IProblem
{
    private readonly string _path;
    private readonly TextWriter _writer;

    public Problem4(string path, TextWriter writer)
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
        var matrix = ReadMatrix(_path);
        var count = 0;

        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                var point = new Point(i, j);

                if (!Box.TryGet(matrix, point, out var box))
                {
                    continue;
                }

                var match = box.Cursors.All(cursor =>
                {
                    var letters = new string(cursor.Take(3).ToArray());
                    return letters is "MAS" or "SAM";
                });

                if (match)
                {
                    count++;
                }
            }
        }

        _writer.WriteLine(count);
    }

    private void SolvePartOne()
    {
        var matrix = ReadMatrix(_path);
        var count = 0;

        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                count += Point.Directions
                    .Select(direction => new Cursor(matrix, direction, new Point(i, j)))
                    .Count(cursor => cursor.Take(4).SequenceEqual("XMAS"));
            }
        }

        _writer.WriteLine(count);
    }

    private static char[,] ReadMatrix(string path)
    {
        var lines = File.ReadAllLines(path);

        var result = new char[lines.Length, lines[0].Length];

        for (var i = 0; i < lines.Length; i++)
        {
            for (var j = 0; j < lines[i].Length; j++)
            {
                result[i, j] = lines[i][j];
            }
        }

        return result;
    }

    private readonly record struct Box(char[,] Matrix, Point Start)
    {
        private const int Size = 2;

        public IEnumerable<Cursor> Cursors
        {
            get
            {
                yield return new Cursor(Matrix, new Point(1, 1), Start);
                yield return new Cursor(Matrix, new Point(1, -1), Start with { Column = Start.Column + 2 });
            }
        }

        public static bool TryGet(
            char[,] matrix,
            Point point,
            out Box box)
        {
            box = default;

            if (point.Row < 0 || point.Row + Size >= matrix.GetLength(0))
            {
                return false;
            }

            if (point.Column < 0 || point.Column + Size >= matrix.GetLength(1))
            {
                return false;
            }

            box = new Box(matrix, point);
            return true;
        }
    }

    private struct Cursor : IEnumerable<char>
    {
        private readonly char[,] _matrix;
        private readonly Point _direction;
        private Point _current;

        public Cursor(
            char[,] matrix,
            Point direction,
            Point current)
        {
            _matrix = matrix;
            _direction = direction;
            _current = current;
        }

        public IEnumerator<char> GetEnumerator()
        {
            while (HasValue)
            {
                yield return _matrix[_current.Row, _current.Column];
                _current = _current.Add(_direction);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private bool HasValue => _current.Row < _matrix.GetLength(0) &&
                                 _current.Row >= 0 &&
                                 _current.Column < _matrix.GetLength(1) &&
                                 _current.Column >= 0;
    }

    private readonly record struct Point(int Row, int Column)
    {
        public static readonly Point[] Directions =
        [
            new(0, 1),
            new(0, -1),

            new(1, 0),
            new(-1, 0),

            new(1, 1),
            new(1, -1),

            new(-1, 1),
            new(-1, -1)
        ];

        public Point Add(Point other)
        {
            return new Point(Row + other.Row, Column + other.Column);
        }
    }
}