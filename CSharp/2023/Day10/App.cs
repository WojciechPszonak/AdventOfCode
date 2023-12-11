using AoC2023_Base;

public class App : Base<char[,]>
{
    private (int X, int Y) start = (-1, -1);

    private readonly Dictionary<char, Direction[]> connections = new()
    {
        {'L', new Direction[2] { Direction.Right, Direction.Up} },
        {'F', new Direction[2] { Direction.Right, Direction.Down} },
        {'J', new Direction[2] { Direction.Left, Direction.Up} },
        {'7', new Direction[2] { Direction.Left, Direction.Down} },
        {'-', new Direction[2] { Direction.Left, Direction.Right} },
        {'|', new Direction[2] { Direction.Up, Direction.Down} }
    };

    public enum Direction { Left, Right, Up, Down };

    public override char[,] Parse(string[] input)
    {
        var map = new char[input[0].Length, input.Length];

        for (var y = 0; y < input.Length; ++y)
        {
            for (var x = 0; x < input[y].Length; ++x)
            {
                map[x, y] = input[y][x];

                if (map[x, y] == 'S')
                {
                    start = (x, y);
                    var connects = new Direction[2];
                    var i = 0;

                    if (x > 0 && connections.Any(c => c.Value.Contains(Direction.Right) && c.Key == input[y][x - 1]))
                    {
                        connects[i++] = Direction.Left;
                    }
                    if (x < input[y].Length - 1 && connections.Any(c => c.Value.Contains(Direction.Left) && c.Key == input[y][x + 1]))
                    {
                        connects[i++] = Direction.Right;
                    }
                    if (y > 0 && connections.Any(c => c.Value.Contains(Direction.Down) && c.Key == input[y - 1][x]))
                    {
                        connects[i++] = Direction.Up;
                    }
                    if (y < input.Length - 1 && connections.Any(c => c.Value.Contains(Direction.Up) && c.Key == input[y + 1][x]))
                    {
                        connects[i++] = Direction.Down;
                    }

                    map[x, y] = connections.Single(c => c.Value.SequenceEqual(connects)).Key;
                }
            }
        }

        return map;
    }

    private static (int X, int Y) Go((int X, int Y) from, Direction direction)
    {
        return direction switch
        {
            Direction.Left => (from.X - 1, from.Y),
            Direction.Right => (from.X + 1, from.Y),
            Direction.Up => (from.X, from.Y - 1),
            Direction.Down => (from.X, from.Y + 1),
            _ => throw new InvalidOperationException()
        };
    }

    public override object Part1(char[,] input)
    {
        var current = start;
        var previous = (X: -1, Y: -1);
        var steps = 0;

        do
        {
            var direction = connections[input[current.X, current.Y]];

            var next = Go(current, direction[0]);

            if (next == previous)
                next = Go(current, direction[1]);

            previous = current;
            current = next;

            ++steps;
        }
        while (current != start);

        return double.Ceiling(steps / 2);
    }

    public override object Part2(char[,] input)
    {
        var current = start;
        var previous = (X: -1, Y: -1);

        var tiles = new char[input.GetLength(0), input.GetLength(1)];

        do
        {
            tiles[current.X, current.Y] = input[current.X, current.Y];

            var direction = connections[input[current.X, current.Y]];

            var next = Go(current, direction[0]);

            if (next == previous)
                next = Go(current, direction[1]);

            previous = current;
            current = next;
        }
        while (current != start);

        var field = 0;

        for (var y = tiles.GetLowerBound(1); y < input.GetLength(1); y++)
        {
            var isOutside = true;

            for (var x = tiles.GetLowerBound(0); x < input.GetLength(0); x++)
            {
                var character = tiles[x, y];

                if (connections.ContainsKey(character))
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    if (character == 'F' || character == '7' || character == '|')
                    {
                        isOutside = !isOutside;
                    }
                }
                else if (character == default)
                {

                    if (isOutside)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        tiles[x, y] = '0';
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        tiles[x, y] = 'I';
                        field++;
                    }
                }

                Console.Write(tiles[x, y]);
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
            }

            Console.Write(Environment.NewLine);
        }

        return field;
    }
}