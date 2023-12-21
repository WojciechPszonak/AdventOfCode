using AoC2023_Base;

public partial class App : Base
{
    private int[,] weights = null!;
    private int width;
    private int height;

    private const int MaxSteps = 3;

    private enum Direction { Left, Right, Up, Down };

    private record Node(int X, int Y, Direction Direction)
    {
        public bool Visited { get; set; }

        public int StepsInDirection { get; set; } = 1;

        public Direction Left
            => Direction switch
            {
                Direction.Left => Direction.Down,
                Direction.Right => Direction.Up,
                Direction.Up => Direction.Left,
                Direction.Down => Direction.Right,
                _ => throw new NotImplementedException(),
            };

        public Direction Right
            => Direction switch
            {
                Direction.Left => Direction.Up,
                Direction.Right => Direction.Down,
                Direction.Up => Direction.Right,
                Direction.Down => Direction.Left,
                _ => throw new NotImplementedException(),
            };
    }

    private class State(Node node, int distance)
    {
        public Node Node { get; } = node;

        public int Distance { get; } = distance;
    }

    public override string[] Parse(string[] input)
    {
        width = input[0].Length;
        height = input.Length;

        weights = new int[width, height];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                weights[x, y] = input[y][x] - '0';
            }
        }

        return base.Parse(input);
    }

    public override object Part1(string[] input)
    {
        var end = (width - 1, height - 1);

        var visited = new HashSet<Node>();
        var queue = new PriorityQueue<State, int>();

        var initialNodes = new Node[]
        {
            new(0, 1, Direction.Down),
            new(1, 0, Direction.Right)
        };

        foreach (var node in initialNodes)
        {
            var distance = weights[node.X, node.Y];
            queue.Enqueue(new State(node, distance), distance);
        }

        while (queue.TryDequeue(out var state, out _))
        {
            if (visited.Contains(state.Node))
            {
                continue;
            }

            visited.Add(state.Node);

            var neighbors = GetUnvisitedNeighbors(state.Node);

            foreach (var neighbor in neighbors)
            {
                var nextState = GetNextState(state, neighbor);
                queue.Enqueue(nextState, nextState.Distance);
            }

            if ((state.Node.X, state.Node.Y) == end)
            {
                return state.Distance;
            }
        }

        return -1;
    }

    private Node[] GetUnvisitedNeighbors(Node current)
    {
        var possibleNeighbors = new Dictionary<Direction, (int X, int Y)>()
        {
            { Direction.Right, (current.X + 1, current.Y)},
            { Direction.Left, (current.X - 1, current.Y)},
            { Direction.Down, (current.X, current.Y + 1)},
            { Direction.Up, (current.X, current.Y - 1)},
        };

        return possibleNeighbors
            .Where(node => ((node.Key == current.Direction && current.StepsInDirection < MaxSteps) || node.Key == current.Left || node.Key == current.Right)
                && node.Value.X >= 0 && node.Value.X < width
                && node.Value.Y >= 0 && node.Value.Y < height)
            .Select(node => new Node(node.Value.X, node.Value.Y, node.Key))
            .ToArray();
    }

    private State GetNextState(State currentState, Node nextNode)
    {
        var newDistance = currentState.Distance + weights[nextNode.X, nextNode.Y];

        nextNode.StepsInDirection = nextNode.Direction == currentState.Node.Direction ? currentState.Node.StepsInDirection + 1 : 1;

        return new State(nextNode, newDistance);
    }

    public override object Part2(string[] input)
    {
        throw new NotImplementedException();
    }
}