using AoC2023_Base;

public class Race
{
    public long Time { get; set; }

    public long RecordDistance { get; set; }
}

public class App : Base<Race[]>
{
    public override Race[] Parse(string[] input)
    {
        var times = input[0]
            .Split(':')[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(long.Parse)
            .ToArray();
        var distances = input[1]
            .Split(':')[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(long.Parse)
            .ToArray();

        var races = times
            .Zip(distances)
            .Select(x => new Race { Time = x.First, RecordDistance = x.Second })
            .ToArray();

        return races;
    }

    public override object Part1(Race[] input)
    {
        var winningWays = new List<long>();

        foreach (var race in input)
        {
            long minWinning = 0;

            for (var i = 0; i <= race.RecordDistance; i++)
            {
                var distance = (race.Time - i) * i;

                if (distance > race.RecordDistance)
                {
                    minWinning = i;
                    break;
                }
            }

            var maxWinning = race.Time - minWinning;

            winningWays.Add(maxWinning - minWinning + 1);
        }

        return winningWays.Aggregate((prev, curr) => prev *= curr);
    }

    public override object Part2(Race[] input)
    {
        var thisOneRace = new Race
        {
            Time = long.Parse(string.Join(string.Empty, input.Select(x => x.Time))),
            RecordDistance = long.Parse(string.Join(string.Empty, input.Select(x => x.RecordDistance)))
        };

        return Part1([thisOneRace]);
    }
}