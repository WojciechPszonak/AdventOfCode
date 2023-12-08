using AoC2023_Base;
using System.Numerics;

public class App : Base
{
    private static readonly Dictionary<string, Node> nodes = [];
    private char[] instructions = [];

    public class Node(string name, string left, string right)
    {
        private readonly Lazy<Node> leftNode = new(() => nodes[left]);
        private readonly Lazy<Node> rightNode = new(() => nodes[right]);

        public Node Left => leftNode.Value;

        public Node Right => rightNode.Value;

        public string Name { get; } = name;
    }

    public override string[] Parse(string[] input)
    {
        instructions = input[0].ToArray();

        for (var i = 2; i < input.Length; i++)
        {
            var line = input[i];
            var parts = line.Split(" = ");
            var nextNodes = parts[1].Trim('(', ')').Split(", ");

            var node = new Node(parts[0], nextNodes[0], nextNodes[1]);
            nodes.Add(parts[0], node);
        }

        return base.Parse(input);
    }

    private long GetSteps(string startingNode, Func<string, bool> endCondition)
    {
        var currentNode = nodes[startingNode];
        var steps = 0;
        var index = 0;

        while (!endCondition(currentNode.Name))
        {
            if (index >= instructions.Length)
            {
                index = 0;
            }

            var direction = instructions[index];
            currentNode = direction == 'L' ? currentNode.Left : currentNode.Right;

            index++;
            steps++;
        }

        return steps;
    }

    public static long GCD(long a, long b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    public static long LCM(long a, long b)
    {
        return (a * b) / GCD(a, b);
    }

    public static long LCM(params long[] numbers)
    {
        return numbers.Aggregate(LCM);
    }

    public override object Part1(string[] input)
    {
        return GetSteps("AAA", x => x == "ZZZ");
    }

    public override object Part2(string[] input)
    {
        var startingNodes = nodes
            .Select(x => x.Key)
            .Where(x => x.EndsWith('A'))
            .ToArray();

        var steps = startingNodes
            .Select(node => GetSteps(node, x => x.EndsWith('Z')))
            .ToArray();

        return LCM(steps);
    }
}