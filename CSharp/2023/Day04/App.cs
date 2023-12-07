using AoC2023_Base;

public class App : Base<Card[]>
{
    private static int[] GetNumbers(string input)
        => input
        .Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(int.Parse)
        .ToArray();

    public override Card[] Parse(string[] input)
    {
        var games = new Card[input.Length]
            .Select((card, id) => new Card { Id = id + 1 })
            .ToArray();

        for (int i = 0; i < input.Length; i++)
        {
            var line = input[i].Split(":")[1].Trim();
            var groups = line.Split("|");

            var winning = GetNumbers(groups[0]);
            var mine = GetNumbers(groups[1]);

            var won = winning.Intersect(mine).ToArray();

            games[i].MatchingNumbers = won.Length;

            for (int j = 1; j <= games[i].MatchingNumbers; j++)
            {
                if (i + j < input.Length)
                {
                    games[i + j].Count += games[i].Count;
                }
            }
        }

        return games;
    }

    public override object Part1(Card[] input)
    {
        return input
            .Where(x => x.MatchingNumbers > 0)
            .Sum(x => Math.Pow(2, x.MatchingNumbers - 1));
    }

    public override object Part2(Card[] input)
    {
        return input
            .Sum(x => x.Count);
    }
}

public class Card
{
    public int Id { get; set; }

    public int Count { get; set; } = 1;

    public int MatchingNumbers { get; set; }
}