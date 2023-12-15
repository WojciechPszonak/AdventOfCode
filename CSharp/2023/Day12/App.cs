using AoC2023_Base;

public class App : Base
{
    private static int Analyze(string text, int[] groups)
    {
        if (text.StartsWith('.'))
        {
            return Analyze(text[1..], groups);
        }
        else if (text.StartsWith('?'))
        {
            return Analyze('.' + text[1..], groups) + Analyze('#' + text[1..], groups);
        }
        else if (text.StartsWith('#'))
        {
            if (groups.Length > 0 && groups[0] <= text.Length && !text[0..groups[0]].Contains('.'))
            {
                if (text[groups[0]..].StartsWith('#'))
                {
                    return 0;
                }
                else if(text[groups[0]..].StartsWith('?'))
                {
                    return Analyze('.' + text[(groups[0] + 1)..], groups[1..]);
                }
                else
                {
                    return Analyze(text[groups[0]..], groups[1..]);
                }
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return groups.Length > 0 ? 0 : 1;
        }
    }

    public override object Part1(string[] input)
    {
        var total = 0;

        foreach (var line in input)
        {
            var parts = line.Split(' ');
            var groups = parts[1]
                .Split(',')
                .Select(int.Parse)
                .ToArray();

            total += Analyze(parts[0], groups);
        }

        return total;
    }

    public override object Part2(string[] input)
    {
        throw new NotImplementedException();
    }
}