using AoC2023_Base;
using System.Data.Common;

public class App : Base<List<char[,]>>
{
    public override List<char[,]> Parse(string[] input)
    {
        var patterns = new List<char[,]>();
        var aggInput = new List<List<string>> { new() };

        _ = input.Aggregate(aggInput[0], (agg, curr) =>
        {
            if (string.IsNullOrWhiteSpace(curr))
            {
                agg = [];
                aggInput.Add(agg);
            }
            else
            {
                agg.Add(curr);
            }

            return agg;
        });

        foreach (var item in aggInput)
        {
            var pattern = new char[item[0].Length, item.Count];

            for (var y = 0; y < item.Count; ++y)
            {
                for (var x = 0; x < item[y].Length; ++x)
                {
                    pattern[x, y] = item[y][x];
                }
            }

            patterns.Add(pattern);
        }

        return patterns;
    }

    private static char[] GetColumn(char[,] matrix, int columnNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(1))
                .Select(x => matrix[columnNumber, x])
                .ToArray();
    }

    private static char[] GetRow(char[,] matrix, int rowNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(0))
                .Select(x => matrix[x, rowNumber])
                .ToArray();
    }

    private static int Compare(char[] x1, char[] x2)
    {
        if (x1.SequenceEqual(x2))
            return 0;

        var differences = 0;

        for (var i = 0; i < x1.Length; ++i)
            if (x1[i] != x2[i])
                ++differences;

        return differences;
    }

    private static int? FindReflectionLine(char[,] pattern, int itemLength, Func<char[,], int, char[]> itemGetter, bool withSmudge)
    {
        int? foundCandidate = null;
        bool foundSmudge;
        var startIndex = 0;

        var items = Enumerable.Range(0, itemLength)
            .Select(i => itemGetter(pattern, i))
            .ToList();

        do
        {
            foundCandidate = null;
            foundSmudge = false;

            for (var i = startIndex; i < itemLength; ++i)
            {
                var item = items[i];

                if (foundCandidate.HasValue) // Check remaining items one by one
                {
                    var comparingWith = 2 * foundCandidate.Value - i - 1;

                    if (comparingWith >= 0)
                    {
                        var differences = Compare(items[comparingWith], item);

                        if (differences == 1 && withSmudge)
                        {
                            if (!foundSmudge)
                            {
                                foundSmudge = true;
                            }
                            else // Second smudge found -> incorrect candidate
                            {
                                startIndex = foundCandidate.Value;
                                foundCandidate = null;
                                foundSmudge = false;
                            }
                        }
                        else if (differences > 0) // More differences between lines -> incorrect candidate
                        {
                            startIndex = foundCandidate.Value;
                            foundCandidate = null;
                            foundSmudge = false;
                        }
                    }
                }
                else if (i != startIndex) // Try to find first candidate
                {
                    var differences = Compare(items[i - 1], item);

                    if (differences == 0)
                    {
                        foundCandidate = i;
                    }
                    else if (differences == 1 && withSmudge)
                    {
                        foundCandidate = i;
                        foundSmudge = true;
                    }
                }
            }

            ++startIndex;
        }
        while (withSmudge && !foundSmudge && startIndex < itemLength);

        return foundCandidate;
    }

    private static int? FindHorizontalReflectionLine(char[,] pattern, bool withSmudge)
        => FindReflectionLine(pattern, pattern.GetLength(1), GetRow, withSmudge);

    private static int? FindVerticalReflectionLine(char[,] pattern, bool withSmudge)
        => FindReflectionLine(pattern, pattern.GetLength(0), GetColumn, withSmudge);

    private static int FindReflectionLine(char[,] pattern, out bool isHorizontal, bool withSmudge = false)
    {
        var horizontal = FindHorizontalReflectionLine(pattern, withSmudge);

        if (horizontal.HasValue)
        {
            isHorizontal = true;
            return horizontal.Value;
        }
        else
        {
            var vertical = FindVerticalReflectionLine(pattern, withSmudge);
            isHorizontal = false;
            return vertical ?? throw new Exception();
        }
    }

    public override object Part1(List<char[,]> input)
    {
        var result = 0L;

        foreach (var pattern in input)
        {
            var number = FindReflectionLine(pattern, out bool isHorizontal);

            result += isHorizontal ? number * 100 : number;
        }

        return result;
    }

    public override object Part2(List<char[,]> input)
    {
        var result = 0L;

        foreach (var pattern in input)
        {
            var number = FindReflectionLine(pattern, out bool isHorizontal, true);

            result += isHorizontal ? number * 100 : number;
        }

        return result;
    }
}