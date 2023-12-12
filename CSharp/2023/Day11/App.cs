using AoC2023_Base;

public class App : Base<char[,]>
{
    private List<(int X, int Y)> galaxies = [];
    private List<int> emptyColumns = [];
    private List<int> emptyRows = [];

    public override char[,] Parse(string[] input)
    {
        var space = new char[input[0].Length, input.Length];

        for (var y = 0; y < input.Length; ++y)
        {
            for (var x = 0; x < input[0].Length; ++x)
            {
                space[x, y] = input[y][x];

                if (space[x, y] == '#')
                {
                    galaxies.Add((x, y));
                }
            }
        }

        emptyColumns = GetEmptyColumns(space);
        emptyRows = GetEmptyRows(space);

        return space;
    }

    private static List<int> GetEmptyColumns(char[,] space)
    {
        var result = new List<int>();

        for (var x = space.GetLowerBound(0); x < space.GetLength(0); ++x)
        {
            if (GetColumn(space, x).All(i => i == '.'))
            {
                result.Add(x);
            }
        }

        return result;
    }

    private static List<int> GetEmptyRows(char[,] space)
    {
        var result = new List<int>();

        for (var y = space.GetLowerBound(1); y < space.GetLength(1); ++y)
        {
            if (GetRow(space, y).All(i => i == '.'))
            {
                result.Add(y);
            }
        }

        return result;
    }

    public static char[] GetColumn(char[,] matrix, int columnNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(1))
                .Select(x => matrix[columnNumber, x])
                .ToArray();
    }

    public static char[] GetRow(char[,] matrix, int rowNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(0))
                .Select(x => matrix[x, rowNumber])
                .ToArray();
    }

    private long GetDistance((int X, int Y) galaxy1, (int X, int Y) galaxy2, int expandRatio)
    {
        var emptyColumnCount = emptyColumns.Count(c => c > Math.Min(galaxy1.X, galaxy2.X) && c < Math.Max(galaxy1.X, galaxy2.X));
        var emptyRowCount = emptyRows.Count(c => c > Math.Min(galaxy1.Y, galaxy2.Y) && c < Math.Max(galaxy1.Y, galaxy2.Y));

        return Math.Abs(galaxy1.X - galaxy2.X) + Math.Abs(galaxy1.Y - galaxy2.Y) + (emptyColumnCount + emptyRowCount) * (expandRatio - 1);
    }

    public override object Part1(char[,] input)
    {
        var result = 0L;

        for (var i1 = 0; i1 < galaxies.Count - 1; ++i1)
        {
            for (var i2 = i1 + 1; i2 < galaxies.Count; ++i2)
            {
                result += GetDistance(galaxies[i1], galaxies[i2], 2);
            }
        }

        return result;
    }

    public override object Part2(char[,] input)
    {
        var result = 0L;

        for (var i1 = 0; i1 < galaxies.Count - 1; ++i1)
        {
            for (var i2 = i1 + 1; i2 < galaxies.Count; ++i2)
            {
                result += GetDistance(galaxies[i1], galaxies[i2], 1_000_000);
            }
        }

        return result;
    }
}