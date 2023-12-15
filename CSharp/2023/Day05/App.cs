using AoC2023_Base;

public class App : Base
{
    private long[] seeds = [];
    private readonly ICollection<Map>[] mapLevels = new ICollection<Map>[7];

    public class Map
    {
        public long SourceStart { get; set; }

        public long SourceEnd { get; set; }

        public long DestinationShift { get; set; }
    }

    public class Range(long start, long end)
    {
        public long Start { get; set; } = start;

        public long End { get; set; } = end;
    }

    public ICollection<Map> GetMaps(string[] input, ref int startIndex)
    {
        var maps = new List<Map>();

        for (var i = startIndex; i < input.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(input[i]))
            {
                startIndex = i + 1;
                break;
            }
            if (input[i].EndsWith("map:"))
            {
                continue;
            }

            var values = input[i]
                .Split(' ')
                .Select(long.Parse)
                .ToArray();

            var destinationStart = values[0];
            var sourceStart = values[1];
            var range = values[2];

            var shift = destinationStart - sourceStart;
            var startEnd = sourceStart + range - 1;

            var map = new Map
            {
                SourceStart = sourceStart,
                SourceEnd = startEnd,
                DestinationShift = shift,
            };

            maps.Add(map);
        }

        return maps;
    }

    public override string[] Parse(string[] input)
    {
        seeds = input[0]
            .Split(": ")[1]
            .Split(' ')
            .Select(long.Parse)
            .ToArray();

        var startIndex = 2;
        mapLevels[0] = GetMaps(input, ref startIndex); // seed-to-soil
        mapLevels[1] = GetMaps(input, ref startIndex); // soil-to-fertilizer
        mapLevels[2] = GetMaps(input, ref startIndex); // fertilizer-to-water
        mapLevels[3] = GetMaps(input, ref startIndex); // water-to-light
        mapLevels[4] = GetMaps(input, ref startIndex); // light-to-temperature
        mapLevels[5] = GetMaps(input, ref startIndex); // temperature-to-humidity
        mapLevels[6] = GetMaps(input, ref startIndex); // humidity-to-location

        return base.Parse(input);
    }

    private long GetMinLocation(long[] seeds)
    {
        var locations = new List<long>();

        foreach (var seed in seeds)
        {
            long mapped = seed;

            foreach (var mapLevel in mapLevels)
            {
                var map = mapLevel.FirstOrDefault(x => x.SourceStart <= mapped && x.SourceEnd >= mapped);

                if (map is not null)
                {
                    mapped += map.DestinationShift;
                }
            }

            locations.Add(mapped);
        }

        return locations.Min();
    }

    public override object Part1(string[] input)
    {
        return GetMinLocation(seeds);
    }

    private Range[] MergeRanges(Range[] ranges)
    {
        ranges = [.. ranges.OrderBy(x => x.Start)];

        var merged = new List<Range>();
        var agg = ranges[0];

        for (var i = 1; i < ranges.Length; i++)
        {
            var curr = ranges[i];

            if (agg.End >= curr.Start - 1)
            {
                agg.End = Math.Max(agg.End, curr.End);
            }
            else
            {
                merged.Add(agg);
                agg = curr;
            }
        }

        merged.Add(agg);
        return [.. merged];
    }

    public override object Part2(string[] input)
    {
        var ranges = new Range[seeds.Length / 2];

        for (var i = 0; i < seeds.Length; i += 2)
        {
            var seedStart = seeds[i];
            var length = seeds[i + 1];

            var seedEnd = seedStart + length - 1;

            ranges[i / 2] = new(seedStart, seedEnd);
        }

        var tasks = new List<Task>();

        foreach (var range in ranges)
        {
            var seeds2 = new List<long>();

            for (var i = range.Start; i < range.End; i++)
            {
                seeds2.Add(i);
            }

            var task = Task.Run(() =>
            {
                var location = GetMinLocation([.. seeds2]);
                Console.WriteLine(location);
            });
            tasks.Add(task);
        }

        Task.WhenAll(tasks).Wait();
        return null!;
    }
}