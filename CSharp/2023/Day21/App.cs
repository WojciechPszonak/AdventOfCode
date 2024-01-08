using AoC2023_Base;
using System.Numerics;

public partial class App : Base<char[,]>
{
    private Point start = new(0, 0);

    private record Point(int X, int Y);

    public override char[,] Parse(string[] input)
    {
        var result = new char[input[0].Length, input.Length];

        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                result[x, y] = input[y][x];

                if (result[x, y] == 'S')
                {
                    start = new(x, y);
                    result[x, y] = '.';
                }
            }
        }

        return result;
    }

    private char? GetSymbol(char[,] input, Point point)
    {
        if (point.X < 0 || point.Y < 0 || point.X >= input.GetLength(0) || point.Y >= input.GetLength(1))
        {
            return null;
        }

        return input[point.X, point.Y];
    }

    private IEnumerable<Point> GetDestinations(char[,] input, IEnumerable<Point> startingPoints)
    {
        var result = new HashSet<Point>();

        foreach (var point in startingPoints)
        {
            var possibilities = new Point[]
            {
                new(point.X + 1, point.Y),
                new(point.X - 1, point.Y),
                new(point.X, point.Y + 1),
                new(point.X, point.Y - 1),
            };

            foreach (var possibility in possibilities)
            {
                if (GetSymbol(input, possibility) == '.')
                {
                    result.Add(possibility);
                }
            }
        }

        return result;
    }

    private int Mod(int x, int modulo) => (x % modulo + modulo) % modulo;

    private char? GetSymbol2(char[,] input, Point point)
    {
        return input[Mod(point.X, input.GetLength(0)), Mod(point.Y, input.GetLength(1))];
    }

    private IEnumerable<Point> GetDestinations2(char[,] input, IEnumerable<Point> startingPoints)
    {
        var result = new HashSet<Point>();

        foreach (var point in startingPoints)
        {
            var possibilities = new Point[]
            {
                new(point.X + 1, point.Y),
                new(point.X - 1, point.Y),
                new(point.X, point.Y + 1),
                new(point.X, point.Y - 1),
            };

            foreach (var possibility in possibilities)
            {
                if (GetSymbol2(input, possibility) == '.')
                {
                    result.Add(possibility);
                }
            }
        }

        return result;
    }

    public override object Part1(char[,] input)
    {
        var steps = isTest ? 6 : 64;
        var startingPoints = new Point[] { start };

        for (var i = 0; i < steps; i++)
        {
            startingPoints = GetDestinations(input, startingPoints).ToArray();
        }

        return startingPoints.Length;
    }

    public override object Part2(char[,] input)
    {
        var steps = isTest ? 5000 : 26501365;
        var startingPoints = new Point[] { start };

        for (var i = 0; i < steps; i++)
        {
            startingPoints = GetDestinations2(input, startingPoints).ToArray();
        }

        return startingPoints.LongLength;
    }
}