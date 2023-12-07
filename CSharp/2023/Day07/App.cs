using AoC2023_Base;

public class App : Base
{
    private static readonly Dictionary<string, Func<char[], bool>> HandTypes = new()
    {
        { "FiveOfKind", x => x.Distinct().Count() == 1},
        { "FourOfKind", x => x.GroupBy(card => card).Select(g => g.Count()).Any(c => c == 4)},
        { "FullHouse", x => x.GroupBy(card => card).Select(g => g.Count()).Order().SequenceEqual([2, 3])},
        { "ThreeOfKind", x => x.GroupBy(card => card).Select(g => g.Count()).Any(c => c == 3)},
        { "TwoPair", x => x.GroupBy(card => card).Select(g => g.Count()).Order().SequenceEqual([1, 2, 2])},
        { "OnePair", x => x.Distinct().Count() != x.Length},
        { "HighCard", x => true},
    };

    public override object Part1(string[] input)
    {
        var hands = input
            .Select(x => x.Split(" "))
            .Select(x => new Hand(x[0], int.Parse(x[1])))
            .ToArray();

        return hands
            .Order()
            .Select((hand, index) => (index + 1) * hand.Bid)
            .Sum();
    }

    public override object Part2(string[] input)
    {
        var hands = input
            .Select(x => x.Split(" "))
            .Select(x => new JokerHand(x[0], int.Parse(x[1])))
            .ToArray();

        return hands
            .Order()
            .Select((hand, index) => (index + 1) * hand.Bid)
            .Sum();
    }

    public class Hand : IComparable<Hand>
    {
        private char[] cards = new char[5];

        public virtual char[] CardLabels { get; } = ['A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2'];

        public char[] Cards
        {
            get => cards;
            private set
            {
                cards = value;
                SetType();
            }
        }

        public int Bid { get; private set; }

        public string Type { get; protected set; } = default!;

        public Hand(string cards, int bid)
        {
            if (cards.Length != 5)
            {
                throw new ArgumentException("Incorrect number of cards");
            }

            Cards = cards.ToArray();
            Bid = bid;
        }

        public int CompareTo(Hand? other)
        {
            if (other is null)
            {
                return 1;
            }
            else if (Type != other.Type)
            {
                var types = HandTypes.Keys.ToArray();
                return Array.IndexOf(types, Type) > Array.IndexOf(types, other.Type) ? -1 : 1;
            }
            else
            {
                for (var i = 0; i < cards.Length; i++)
                {
                    if (other.Cards[i] == cards[i])
                    {
                        continue;
                    }
                    else
                    {
                        return Array.IndexOf(CardLabels, cards[i]) > Array.IndexOf(CardLabels, other.Cards[i]) ? -1 : 1;
                    }
                }
            }

            return 0;
        }

        protected virtual void SetType()
        {
            foreach (var type in HandTypes)
            {
                if (type.Value(this.Cards))
                {
                    Type = type.Key;
                    break;
                }
            }
        }
    }

    public class JokerHand : Hand
    {
        public override char[] CardLabels { get; } = ['A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J'];

        public JokerHand(string cards, int bid)
            : base(cards, bid)
        {
        }

        protected override void SetType()
        {
            var bestCards = new char[5];
            Cards.CopyTo(bestCards, 0);

            if (Cards.Contains('J'))
            {
                var bestReplacement = Cards
                    .Where(x => x != 'J')
                    .GroupBy(x => x)
                    .Select(g => new { g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .ThenBy(x => Array.IndexOf(CardLabels, x.Key))
                    .Select(x => x.Key)
                    .FirstOrDefault();

                if (bestReplacement == default)
                {
                    bestReplacement = CardLabels[0];
                }

                bestCards = bestCards
                    .Select(x => x == 'J' ? bestReplacement : x)
                    .ToArray();
            }

            foreach (var type in HandTypes)
            {
                if (type.Value(bestCards))
                {
                    Type = type.Key;
                    break;
                }
            }
        }
    }
}