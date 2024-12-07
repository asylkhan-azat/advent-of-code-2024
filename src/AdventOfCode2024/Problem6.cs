using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2024;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public sealed class Problem6 : IProblem
{
    private readonly string _path;
    private readonly TextWriter _textWriter;

    public Problem6(string path, TextWriter textWriter)
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

    private void SolvePartTwo()
    {
        var (map, guard) = ReadMapAndGuard(_path);

        var pointsThatCreateLoop = GetVisitedPointsExcludingGuardPosition(map, guard)
            .Count(point => IsLoop(map, guard, point));

        _textWriter.WriteLine(pointsThatCreateLoop);
    }

    private static bool IsLoop(
        Map map,
        Guard guard,
        Point newObstacle)
    {
        var visitedObstacles = new HashSet<(Point, Point)>();

        while (map.HasCell(guard.NextPosition))
        {
            if (map.IsObstacle(guard.NextPosition) || guard.NextPosition == newObstacle)
            {
                if (!visitedObstacles.Add((guard.NextPosition, guard.Position)))
                {
                    return true;
                }

                guard.Turn();
                continue;
            }

            guard.Move();
        }

        return false;
    }

    private static HashSet<Point> GetVisitedPointsExcludingGuardPosition(Map map, Guard guard)
    {
        var visited = new HashSet<Point>();
        var initialPosition = guard.Position;

        while (map.HasCell(guard.NextPosition))
        {
            visited.Add(guard.Position);

            if (map.IsObstacle(guard.NextPosition))
            {
                guard.Turn();
                continue;
            }

            guard.Move();
        }

        visited.Add(guard.Position);
        visited.Remove(initialPosition);
        return visited;
    }

    private void SolvePartOne()
    {
        var (map, guard) = ReadMapAndGuard(_path);
        var visited = new HashSet<Point>();

        while (map.HasCell(guard.NextPosition))
        {
            visited.Add(guard.Position);

            if (map.IsObstacle(guard.NextPosition))
            {
                guard.Turn();
                continue;
            }

            guard.Move();
        }

        visited.Add(guard.Position);
        _textWriter.WriteLine(visited.Count);
    }

    private static (Map, Guard) ReadMapAndGuard(string path)
    {
        var lines = File.ReadAllLines(path);

        var cells = new char[lines.Length, lines[0].Length];
        Point guardPosition = default;

        for (var i = 0; i < cells.GetLength(0); i++)
        {
            for (var j = 0; j < cells.GetLength(1); j++)
            {
                cells[i, j] = lines[i][j];
                if (cells[i, j] is '^') guardPosition = new Point(i, j);
            }
        }

        return (new Map(cells), new Guard(guardPosition));
    }

    private struct Guard
    {
        private Point _position;
        private Point _direction;

        public Guard(Point initialPosition)
        {
            _position = initialPosition;
            _direction = Directions.Up;
        }

        public Point Position => _position;

        public Point NextPosition => _position.Add(_direction);

        public void Turn()
        {
            _direction = Directions.Next(_direction);
        }

        public void Move()
        {
            _position = _position.Add(_direction);
        }
    }

    private readonly struct Map
    {
        private readonly char[,] _cells;

        public Map(char[,] cells)
        {
            _cells = cells;
        }

        public int Rows => _cells.GetLength(0);
        public int Columns => _cells.GetLength(1);

        public bool IsObstacle(Point point) => CellAt(point) is '#';
        public ref char CellAt(Point point) => ref _cells[point.Row, point.Column];

        public bool HasCell(Point point) =>
            point.Row >= 0 && point.Row < Rows &&
            point.Column >= 0 && point.Column < Columns;
    }

    private readonly record struct Point(int Row, int Column)
    {
        public Point Add(Point other)
        {
            return new Point(Row + other.Row, Column + other.Column);
        }
    }

    private static class Directions
    {
        public static readonly Point Up = new(-1, 0);
        public static readonly Point Right = new(0, 1);
        public static readonly Point Left = new(0, -1);
        public static readonly Point Down = new(1, 0);

        private static readonly Point[] Movements = [Up, Right, Down, Left];

        public static Point Next(Point point)
        {
            var index = Array.IndexOf(Movements, point);

            Debug.Assert(index is not -1);

            return index + 1 >= Movements.Length ? Movements[0] : Movements[index + 1];
        }
    }
}