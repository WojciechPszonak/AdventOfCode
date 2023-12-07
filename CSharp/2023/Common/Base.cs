namespace AoC2023_Base
{
    public abstract class Base : Base<string[]>
    {
        public override string[] Parse(string[] input)
            => input;
    }
}
