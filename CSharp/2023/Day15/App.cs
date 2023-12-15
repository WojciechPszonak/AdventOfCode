using AoC2023_Base;
using System.Text.RegularExpressions;

public partial class App : Base<string>
{
    private static long ComputeHash(string input)
    {
        var currentValue = 0L;

        foreach (var character in input)
        {
            var ascii = (int)character;
            currentValue += ascii;
            currentValue *= 17;
            currentValue %= 256;
        }

        return currentValue;
    }

    public override string Parse(string[] input)
    {
        return input[0];
    }

    public override object Part1(string input)
    {
        return input.Split(',')
            .Sum(ComputeHash);
    }

    public override object Part2(string input)
    {
        var boxes = Enumerable.Range(0, 256)
            .Select(x => new List<Lens>())
            .ToArray();

        var instructions = input.Split(',').ToList();

        foreach (var instruction in instructions)
        {
            var match = InstructionRegex().Match(instruction);

            var lens = match.Groups[1].Value;
            var operation = match.Groups[2].Value;
            int.TryParse(match.Groups[3].Value, out int value);

            var boxNumber = ComputeHash(lens);
            var box = boxes[boxNumber];

            if (operation == "=")
            {
                var existingLens = box.SingleOrDefault(x => x.Name == lens);

                if (existingLens is not null)
                {
                    existingLens.FocalLength = value;
                }
                else
                {
                    box.Add(new Lens(lens, value));
                }
            }
            else
            {
                var existingLensIndex = box.FindIndex(x => x.Name == lens);
                if (existingLensIndex > -1)
                {
                    box.RemoveAt(existingLensIndex);
                }
            }
        }

        var result = boxes.Select((box, boxIndex) => box.Select((lens, lensIndex) => ((long)boxIndex + 1) * (lensIndex + 1) * lens.FocalLength))
            .SelectMany(x => x)
            .Sum();

        return result;

    }

    private class Lens(string name, int focalLength)
    {
        public string Name { get; } = name;
        public int FocalLength { get; set; } = focalLength;
    }

    [GeneratedRegex("([a-z]+)(-|=)(\\d?)")]
    private static partial Regex InstructionRegex();
}