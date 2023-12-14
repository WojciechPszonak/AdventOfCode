using AoC2023_Base;

public class App : Base<char[,]>
{
    public override char[,] Parse(string[] input)
    {
        var result = new char[input[0].Length, input.Length];

        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                result[x, y] = input[y][x];
            }
        }

        return result;
    }

    private static char[,] RollNorth(char[,] input)
    {
        var platform = (char[,])input.Clone();

        for (var x = 0; x < platform.GetLength(0); x++)
        {
            for (var y = 0; y < platform.GetLength(1); y++)
            {
                if (platform[x, y] == 'O')
                {
                    platform[x, y] = '.';

                    for (var y1 = y; y1 >= 0; y1--)
                    {
                        if (platform[x, y1] == 'O' || platform[x, y1] == '#')
                        {
                            platform[x, y1 + 1] = 'O';
                            break;
                        }
                        else if (y1 == 0)
                        {
                            platform[x, y1] = 'O';
                        }
                    }
                }
            }
        }

        return platform;
    }

    private static char[,] RollWest(char[,] input)
    {
        var platform = (char[,])input.Clone();

        for (var y = 0; y < platform.GetLength(1); y++)
        {
            for (var x = 0; x < platform.GetLength(0); x++)
            {
                if (platform[x, y] == 'O')
                {
                    platform[x, y] = '.';

                    for (var x1 = x; x1 >= 0; x1--)
                    {
                        if (platform[x1, y] == 'O' || platform[x1, y] == '#')
                        {
                            platform[x1 + 1, y] = 'O';
                            break;
                        }
                        else if (x1 == 0)
                        {
                            platform[x1, y] = 'O';
                        }
                    }
                }
            }
        }

        return platform;
    }
    private static char[,] RollSouth(char[,] input)
    {
        var platform = (char[,])input.Clone();

        for (var x = 0; x < platform.GetLength(0); x++)
        {
            for (var y = platform.GetLength(1) - 1; y >= 0; y--)
            {
                if (platform[x, y] == 'O')
                {
                    platform[x, y] = '.';

                    for (var y1 = y; y1 < platform.GetLength(1); y1++)
                    {
                        if (platform[x, y1] == 'O' || platform[x, y1] == '#')
                        {
                            platform[x, y1 - 1] = 'O';
                            break;
                        }
                        else if (y1 == platform.GetLength(1) - 1)
                        {
                            platform[x, y1] = 'O';
                        }
                    }
                }
            }
        }

        return platform;
    }

    private static char[,] RollEast(char[,] input)
    {
        var platform = (char[,])input.Clone();

        for (var y = 0; y < platform.GetLength(1); y++)
        {
            for (var x = platform.GetLength(0) - 1; x >= 0; x--)
            {
                if (platform[x, y] == 'O')
                {
                    platform[x, y] = '.';

                    for (var x1 = x; x1 < platform.GetLength(0); x1++)
                    {
                        if (platform[x1, y] == 'O' || platform[x1, y] == '#')
                        {
                            platform[x1 - 1, y] = 'O';
                            break;
                        }
                        else if (x1 == platform.GetLength(0) - 1)
                        {
                            platform[x1, y] = 'O';
                        }
                    }
                }
            }
        }
        return platform;
    }

    private static char[] GetRow(char[,] matrix, int rowNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(0))
                .Select(x => matrix[x, rowNumber])
                .ToArray();
    }

    private static int GetTotalLoad(char[,] platform)
    {
        var height = platform.GetLength(1);
        var result = 0;

        for (var y = 0; y < height; y++)
        {
            var row = GetRow(platform, y);
            var count = row.Count(x => x == 'O');

            result += count * (height - y);
        }

        return result;
    }

    public static void PrintPlatform(char[,] input)
    {
        Console.Clear();

        for (var y = 0; y < input.GetLength(1); y++)
        {
            for (var x = 0; x < input.GetLength(0); x++)
            {
                Console.Write(input[x, y]);
            }

            Console.WriteLine();
        }

        Console.ReadLine();
    }

    public override object Part1(char[,] input)
    {
        var platform = RollNorth(input);
        int result = GetTotalLoad(platform);

        return result;
    }

    public override object Part2(char[,] input)
    {
        var iterations = 1_000_000_000;
        var configurations = new List<char[,]>();
        var cycleStart = 0;
        var cycleEnd = 0;

        for (var i = 0; i < iterations; i++)
        {
            input = RollNorth(input);
            input = RollWest(input);
            input = RollSouth(input);
            input = RollEast(input);

            var existingConfigurationIndex = configurations.FindIndex(x => x.Cast<char>().SequenceEqual(input.Cast<char>()));

            if (existingConfigurationIndex > -1)
            {
                cycleStart = existingConfigurationIndex;
                cycleEnd = i;

                break;
            }
            else
            {
                configurations.Add(input);
            }
        }

        var cycleLength = cycleEnd - cycleStart;
        var offset = (iterations - cycleStart - 1) % cycleLength;

        var resultConfiguration = configurations[cycleStart + offset];

        return GetTotalLoad(resultConfiguration);
    }
}