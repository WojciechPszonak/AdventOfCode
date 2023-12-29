using AoC2023_Base;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

public partial class App : Base
{
    private const int MaxValue = 4000;

    private readonly Dictionary<string, IEnumerable<Rule>> workflows = [];
    private readonly List<Part> parts = [];
    private readonly List<Part> accepted = [];
    private readonly List<Part> rejected = [];
    private readonly Dictionary<string, IEnumerable<Setup>> cache = [];

    public override string[] Parse(string[] input)
    {
        var readingParts = false;

        foreach (var line in input)
        {
            if (!readingParts)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    readingParts = true;
                }

                var match = WorkflowRegex().Match(line);
                var workflowName = match.Groups[1].Value;

                var rules = match.Groups[2].Value.Split(',')
                    .Select(x => x.Split(':'))
                    .Select(x => x.Length > 1
                        ? new Rule(ParseCondition(x[0]), x[0], x[1])
                        : new Rule(part => true, null, x[0]))
                    .ToList();

                workflows.Add(workflowName, rules);
            }
            else
            {
                var matches = PartRegex().Matches(line).ToArray();
                var part = new Part(
                    int.Parse(matches[0].Value),
                    int.Parse(matches[1].Value),
                    int.Parse(matches[2].Value),
                    int.Parse(matches[3].Value));

                parts.Add(part);
            }
        }

        return base.Parse(input);
    }

    private static Func<Part, bool> ParseCondition(string condition)
    {
        var match = ConditionRegex().Match(condition);

        var parameter = Expression.Parameter(typeof(Part));
        var property = Expression.Property(parameter, match.Groups[1].Value.ToUpper());
        var constant = Expression.Constant(int.Parse(match.Groups[3].Value));

        var operation = match.Groups[2].Value == ">"
            ? Expression.GreaterThan(property, constant)
            : Expression.LessThan(property, constant);

        return Expression.Lambda<Func<Part, bool>>(operation, parameter).Compile();
    }

    private Setup[] Split(Setup setup, char parameter, char @operator, int value)
    {
        var result = new Setup[]
        {
            setup.Clone(),
            setup.Clone(),
        };

        var ranges = parameter switch
        {
            'x' => (result[0].X, result[1].X),
            'm' => (result[0].M, result[1].M),
            'a' => (result[0].A, result[1].A),
            's' => (result[0].S, result[1].S),
            _ => throw new Exception()
        };

        switch (@operator)
        {
            case '>':
                ranges.Item1.Min = Math.Max(ranges.Item1.Min, value + 1);
                ranges.Item2.Max = Math.Min(ranges.Item2.Max, value);
                break;
            case '<':
                ranges.Item1.Max = Math.Min(ranges.Item1.Max, value - 1);
                ranges.Item2.Min = Math.Max(ranges.Item2.Min, value);
                break;
        };

        return result;
    }

    private IEnumerable<Setup> GetSetups(Setup currentSetup, IEnumerable<Rule> rules)
    {
        var result = new List<Setup>();

        foreach (var rule in rules)
        {
            var destination = rule.Destination;

            if (rule.Raw is not null)
            {
                var parameter = rule.Raw[0];
                var @operator = rule.Raw[1];
                var value = int.Parse(rule.Raw[2..]);

                var adjustedSetups = Split(currentSetup, parameter, @operator, value);

                if (destination == "A")
                {
                    result.Add(adjustedSetups[0]);
                }
                else if (destination != "R")
                {
                    result.AddRange(GetSetups(adjustedSetups[0], workflows[destination]));
                }

                currentSetup = adjustedSetups[1];
            }
            else if (destination == "A")
            {
                result.Add(currentSetup);
            }
            else if (destination != "R")
            {
                result.AddRange(GetSetups(currentSetup, workflows[destination]));
            }
        }

        return result;
    }

    public override object Part1(string[] input)
    {
        foreach (var part in parts)
        {
            var currentWorkflow = workflows["in"];

            while (currentWorkflow is not null)
            {
                foreach (var rule in currentWorkflow)
                {
                    if (rule.Condition(part))
                    {
                        switch (rule.Destination)
                        {
                            case "A": accepted.Add(part); currentWorkflow = null; break;
                            case "R": rejected.Add(part); currentWorkflow = null; break;
                            default: currentWorkflow = workflows[rule.Destination]; break;
                        }

                        break;
                    }
                }
            }
        }

        return accepted.Sum(part => part.X + part.M + part.A + part.S);
    }

    public override object Part2(string[] input)
    {
        var result = GetSetups(new Setup(), workflows["in"]);

        return result
            .Sum(x => (long)x.X.Length * x.M.Length * x.A.Length * x.S.Length);
    }

    private record Part(int X, int M, int A, int S);

    private record Rule(Func<Part, bool> Condition, string? Raw, string Destination);

    private class Setup
    {
        public Range X { get; set; } = new();
        public Range M { get; set; } = new();
        public Range A { get; set; } = new();
        public Range S { get; set; } = new();

        public Setup Clone()
            => new()
            {
                X = X.Clone(),
                M = M.Clone(),
                A = A.Clone(),
                S = S.Clone()
            };
    }

    private class Range
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public int Length => Max - Min + 1;

        public Range(int min = 1, int max = MaxValue)
        {
            Min = min;
            Max = max;
        }

        public Range Clone()
            => new(Min, Max);
    }

    [GeneratedRegex("(\\w+){(.*)}")]
    private static partial Regex WorkflowRegex();

    [GeneratedRegex("\\d+")]
    private static partial Regex PartRegex();

    [GeneratedRegex("(\\w)(<|>)(\\d+)")]
    private static partial Regex ConditionRegex();
}