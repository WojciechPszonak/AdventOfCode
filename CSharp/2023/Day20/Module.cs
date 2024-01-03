public class Module(string name)
{
    public string Name { get; } = name;

    public List<Module> Inputs { get; } = [];

    public List<Module> Outputs { get; } = [];

    public long LowSent { get; set; }
    
    public long HighSent { get; set; }

    public Pulse State { get; protected set; } = Pulse.Low;

    public virtual void Reset()
    {
        State = Pulse.Low;
        LowSent = 0;
        HighSent = 0;
    }

    public virtual void HandleMessage(Module? input, Pulse pulse)
    {
    }

    public virtual void AddInput(Module input) => Inputs.Add(input);

    public virtual void AddOutput(Module output) => Outputs.Add(output);
}

public class BroadcasterModule : Module
{
    public BroadcasterModule() : base("broadcaster")
    {
    }

    public override void HandleMessage(Module? input, Pulse pulse)
    {
        State = pulse;

        foreach (var module in Outputs)
        {
            App.SendPulse(this, module, pulse);
        }

        base.HandleMessage(input, pulse);
    }
}

public class FlipFlopModule(string name) : Module(name)
{
    public override void HandleMessage(Module? input, Pulse pulse)
    {
        if (pulse == Pulse.Low)
        {
            State = State == Pulse.Low ? Pulse.High : Pulse.Low;

            foreach (var module in Outputs)
            {
                App.SendPulse(this, module, State);
            }
        }

        base.HandleMessage(input, pulse);
    }
}

public class ConjunctionModule(string name) : Module(name)
{
    private readonly Dictionary<Module, Pulse> inputStates = [];

    public override void Reset()
    {
        foreach (var input in Inputs)
        {
            inputStates[input] = Pulse.Low;
        }

        base.Reset();
    }

    public override void AddInput(Module input)
    {
        inputStates.Add(input, Pulse.Low);
        base.AddInput(input);
    }

    public override void HandleMessage(Module? input, Pulse pulse)
    {
        inputStates[input!] = pulse;

        State = inputStates.All(x => x.Value == Pulse.High) ? Pulse.Low : Pulse.High;

        foreach (var module in Outputs)
        {
            App.SendPulse(this, module, State);
        }

        base.HandleMessage(input, pulse);
    }
}