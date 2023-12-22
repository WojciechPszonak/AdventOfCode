using AoC2023_Base;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

public partial class App : Base
{
    private readonly Dictionary<string, IEnumerable<Rule>> workflows = [];
    private readonly List<Part> parts = [];
    private readonly List<Part> accepted = [];
    private readonly List<Part> rejected = [];

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
                        ? new Rule(ParseCondition(x[0]), x[1])
                        : new Rule(part => true, x[0]))
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
        var result = workflows.SelectMany(x => x.Value.Select(y => y.Destination));
        return null!;
    }

    private record Part(int X, int M, int A, int S);

    private record Rule(Func<Part, bool> Condition, string Destination);

    [GeneratedRegex("(\\w+){(.*)}")]
    private static partial Regex WorkflowRegex();

    [GeneratedRegex("\\d+")]
    private static partial Regex PartRegex();

    [GeneratedRegex("(\\w)(<|>)(\\d+)")]
    private static partial Regex ConditionRegex();
}