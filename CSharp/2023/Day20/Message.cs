public class Message
{
    public Module? Source { get; set; }

    public Module Destination { get; set; } = default!;

    public Pulse Pulse { get; set; }
}