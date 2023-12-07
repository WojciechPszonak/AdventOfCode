namespace AoC2023_Base
{
    public abstract class Base<T>
    {
        private static string[] Read(bool useTest = false)
            => [.. File.ReadAllLines(useTest ? "input2.txt" : "input.txt")];

        private void Print(Func<T, object> func, T input)
        {
            Console.WriteLine(func.Method.Name);
            try
            {
                Console.WriteLine(func(input));
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("Not implemented");
            }
        }

        public abstract T Parse(string[] input);

        public abstract object Part1(T input);

        public abstract object Part2(T input);

        public virtual void Run(bool useTest = false)
        {
            var input = Read(useTest);
            var data = Parse(input);

            Print(Part1, data);
            Print(Part2, data);
        }
    }
}
