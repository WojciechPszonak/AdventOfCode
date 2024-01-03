using AoC2023_Base;

public partial class App : Base
{
    private List<Module> modules = [];
    private static Queue<Message> queue = new();

    private static long lowSent = 0;
    private static long highSent = 0;

    public override string[] Parse(string[] input)
    {
        var pairs = input.Select(line => line.Split("->", StringSplitOptions.TrimEntries));

        modules = pairs
            .Select(x => x[0])
            .Select(x =>
            {
                if (x == "broadcaster")
                    return new BroadcasterModule();
                else if (x.StartsWith('%'))
                    return new FlipFlopModule(x[1..]);
                else if (x.StartsWith('&'))
                    return new ConjunctionModule(x[1..]);
                else
                    return new Module(x);
            })
            .ToList();

        foreach (var pair in pairs)
        {
            var name = pair[0].TrimStart('%').TrimStart('&');
            var module = modules.Single(x => x.Name == name);

            var outputs = pair[1].Split(',', StringSplitOptions.TrimEntries);

            foreach (var output in outputs)
            {
                var outputModule = modules.SingleOrDefault(x => x.Name == output);

                if (outputModule is null)
                {
                    outputModule = new Module(output);
                    modules.Add(outputModule);
                }

                module.AddOutput(outputModule);
                outputModule.AddInput(module);
            }
        }

        return base.Parse(input);
    }

    public static void SendPulse(Module? source, Module destination, Pulse pulse)
    {
        queue.Enqueue(new Message
        {
            Source = source,
            Destination = destination,
            Pulse = pulse
        });

        _ = pulse == Pulse.Low ? lowSent++ : highSent++;

        if (source is not null)
        {
            _ = pulse == Pulse.Low ? source.LowSent++ : source.HighSent++;
        }
    }

    private void Reset()
    {
        foreach (var module in modules)
        {
            module.Reset();
        }

        lowSent = 0;
        highSent = 0;
    }

    private static long GCD(long a, long b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    private static long LCM(long a, long b)
    {
        return (a * b) / GCD(a, b);
    }

    private static long LCM(params long[] numbers)
    {
        return numbers.Aggregate(LCM);
    }

    public override object Part1(string[] input)
    {
        var broadcaster = modules.Single(x => x is BroadcasterModule);

        for (var i = 0; i < 1000; i++)
        {
            SendPulse(null, broadcaster, Pulse.Low);

            while (queue.TryDequeue(out var message))
            {
                message.Destination.HandleMessage(message.Source, message.Pulse);
            }
        }

        return lowSent * highSent;
    }

    public override object Part2(string[] input)
    {
        if (isTest)
        {
            return null!;
        }

        Reset();
        var broadcaster = modules.Single(x => x is BroadcasterModule);
        long buttonPressed = 0;

        var rx = modules.Single(x => x.Name == "rx");
        var trackedModules = modules.Where(x => x.Outputs.Any(y => y.Outputs.Contains(rx))).ToArray();
        var cycles = new Dictionary<Module, long>();

        while (cycles.Count != trackedModules.Length)
        {
            buttonPressed++;
            SendPulse(null, broadcaster, Pulse.Low);

            while (queue.TryDequeue(out var message))
            {
                message.Destination.HandleMessage(message.Source, message.Pulse);
            }

            foreach (var module in trackedModules)
            {
                if (module.HighSent == 1)
                {
                    cycles.TryAdd(module, buttonPressed);
                }
            }
        }

        return LCM([.. cycles.Values]);
    }
}