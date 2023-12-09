using AoC2023_Base;

public class App : Base
{
    private static long PredictNextValue(long[] values)
    {
        var differences = GetDifferences(values);

        return differences.All(x => x == 0) ? values.Last() : values.Last() + PredictNextValue(differences);
    }

    private static long PredictPreviousValue(long[] values)
    {
        var differences = GetDifferences(values);

        return differences.All(x => x == 0) ? values.First() : values.First() - PredictPreviousValue(differences);
    }

    private static long[] GetDifferences(long[] values)
    {
        return values.Aggregate(
            (Previous: 0L, Items: new List<long>()),
            (agg, curr) =>
            {
                agg.Items.Add(curr - agg.Previous);
                agg.Previous = curr;
                return agg;
            },
            x => x.Items.Skip(1).ToArray());
    }

    public override object Part1(string[] input)
    {
        var result = 0L;

        foreach (var line in input)
        {
            var values = line.Split(' ')
                .Select(long.Parse)
                .ToArray();

            result += PredictNextValue(values);
        }

        return result;
    }

    public override object Part2(string[] input)
    {
        var result = 0L;

        foreach (var line in input)
        {
            var values = line.Split(' ')
                .Select(long.Parse)
                .ToArray();

            result += PredictPreviousValue(values);
        }

        return result;
    }
}