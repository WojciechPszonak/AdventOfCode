using AoC2023_Base;
using static App;

public partial class App : Base<Tile[,]>
{
    public enum Direction { Left, Right, Up, Down };

    public class Tile(char value) : ICloneable
    {
        public char Value { get; } = value;

        public HashSet<Direction> VisitedWithDirections { get; } = [];

        public object Clone()
        {
            return new Tile(Value);
        }
    }

    public override Tile[,] Parse(string[] input)
    {
        var result = new Tile[input[0].Length, input.Length];

        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                result[x, y] = new Tile(input[y][x]);
            }
        }

        return result;
    }

    private static int Go(Tile[,] input, (int X, int Y) start, Direction direction)
    {
        var current = start;
        var result = 0;

        while (current.X >= 0
            && current.Y >= 0
            && current.X < input.GetLength(0)
            && current.Y < input.GetLength(1)
            && !input[current.X, current.Y].VisitedWithDirections.Contains(direction))
        {
            var currentTile = input[current.X, current.Y];
            if (currentTile.VisitedWithDirections.Count == 0)
            {
                result++;
            }
            currentTile.VisitedWithDirections.Add(direction);

            if (currentTile.Value == '.'
                || (currentTile.Value == '-' && (direction == Direction.Left || direction == Direction.Right))
                || (currentTile.Value == '|' && (direction == Direction.Up || direction == Direction.Down)))
            {
                current = direction switch
                {
                    Direction.Left => (current.X - 1, current.Y),
                    Direction.Right => (current.X + 1, current.Y),
                    Direction.Up => (current.X, current.Y - 1),
                    Direction.Down => (current.X, current.Y + 1),
                    _ => throw new Exception()
                };
            }
            else if (currentTile.Value == '-')
            {
                result += Go(input, (current.X - 1, current.Y), Direction.Left);
                result += Go(input, (current.X + 1, current.Y), Direction.Right);
                return result;
            }
            else if (currentTile.Value == '|')
            {
                result += Go(input, (current.X, current.Y - 1), Direction.Up);
                result += Go(input, (current.X, current.Y + 1), Direction.Down);
                return result;
            }
            else if (currentTile.Value == '\\')
            {
                (current, direction) = direction switch
                {
                    Direction.Left => ((current.X, current.Y - 1), Direction.Up),
                    Direction.Right => ((current.X, current.Y + 1), Direction.Down),
                    Direction.Up => ((current.X - 1, current.Y), Direction.Left),
                    Direction.Down => ((current.X + 1, current.Y), Direction.Right),
                    _ => throw new Exception()
                };
            }
            else if (currentTile.Value == '/')
            {
                (current, direction) = direction switch
                {
                    Direction.Left => ((current.X, current.Y + 1), Direction.Down),
                    Direction.Right => ((current.X, current.Y - 1), Direction.Up),
                    Direction.Up => ((current.X + 1, current.Y), Direction.Right),
                    Direction.Down => ((current.X - 1, current.Y), Direction.Left),
                    _ => throw new Exception()
                };
            }
        }

        return result;
    }

    private static Tile[,] CloneTiles(Tile[,] input)
    {
        var clone = (Tile[,])input.Clone();

        for (int x = 0; x < input.GetLength(0); x++)
        {
            for (int y = 0; y < input.GetLength(1); y++)
            {
                clone[x,y] = (Tile)input[x, y].Clone();
            }
        }

        return clone;
    }

    public override object Part1(Tile[,] input)
    {
        var array = CloneTiles(input);
        var result = Go(array, (X: 0, Y: 0), Direction.Right);

        return result;
    }

    public override object Part2(Tile[,] input)
    {
        var maxResult = 0;

        for (var x = 0; x < input.GetLength(0); x++)
        {
            var array1 = CloneTiles(input);
            int result1 = Go(array1, (X: x, Y: 0), Direction.Down);

            if (result1 > maxResult)
                maxResult = result1;

            var array2 = CloneTiles(input);
            var result2 = Go(array2, (X: x, Y: input.GetLength(1) - 1), Direction.Up);

            if (result2 > maxResult)
                maxResult = result2;
        }

        for (var y = 0; y < input.GetLength(1); y++)
        {
            var array1 = CloneTiles(input);
            int result1 = Go(array1, (X: 0, Y: y), Direction.Right);

            if (result1 > maxResult)
                maxResult = result1;

            var array2 = CloneTiles(input);
            var result2 = Go(array2, (X: input.GetLength(0) - 1, Y: y), Direction.Left);

            if (result2 > maxResult)
                maxResult = result2;
        }

        return maxResult;
    }
}