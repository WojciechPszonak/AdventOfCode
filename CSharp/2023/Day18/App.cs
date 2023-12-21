using AoC2023_Base;
using System.Globalization;

public partial class App : Base
{
    private class Point(long X, long Y)
    {
        public long X { get; set; } = X;
        public long Y { get; set; } = Y;

        public Point(Point other) : this(other.X, other.Y)
        {
        }
    }

    private static List<Point> Parse1(string[] input)
    {
        var current = new Point(0, 0);
        var vertices = new List<Point>();

        foreach (var line in input)
        {
            var values = line.Split(' ');

            var direction = values[0];
            var steps = int.Parse(values[1]);

            for (var i = 0; i < steps; i++)
            {
                switch (direction)
                {
                    case "L": current.X--; break;
                    case "R": current.X++; break;
                    case "U": current.Y--; break;
                    case "D": current.Y++; break;
                }
            }

            vertices.Add(new Point(current));
        }

        return vertices;
    }

    private static List<Point> Parse2(string[] input)
    {
        var current = new Point(0, 0);
        var vertices = new List<Point>();

        foreach (var line in input)
        {
            var values = line.Split(' ');

            var color = values[2].TrimStart('(').TrimStart('#').TrimEnd(')');
            var direction = color[5];
            var steps = long.Parse(color[0..5], NumberStyles.HexNumber);

            for (var i = 0; i < steps; i++)
            {
                switch (direction)
                {
                    case '0': current.X++; break;
                    case '1': current.Y++; break;
                    case '2': current.X--; break;
                    case '3': current.Y--; break;
                }
            }

            vertices.Add(new Point(current));
        }

        return vertices;
    }

    private static long CalculateField(List<Point> vertices)
    {
        var result = 0L;

        for (var i1 = 0; i1 < vertices.Count; i1++)
        {
            var i2 = (i1 + 1) % vertices.Count;

            var vertex1 = vertices[i1];
            var vertex2 = vertices[i2];

            result += vertex1.X * vertex2.Y - vertex1.Y * vertex2.X;
            result += Math.Abs(vertex1.X - vertex2.X + vertex1.Y - vertex2.Y);
        }

        return result / 2 + 1;
    }

    public override object Part1(string[] input)
    {
        var vertices = Parse1(input);

        return CalculateField(vertices);
    }

    public override object Part2(string[] input)
    {
        var vertices = Parse2(input);

        return CalculateField(vertices);
    }
}