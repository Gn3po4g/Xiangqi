using Godot;
using Xiangqi.Scripts.Engine;

namespace Xiangqi.Scripts.StateMachine;

public partial class StateMachine : Node
{
    [Export] public required BaseState InitialState { get; set; }
    [Export] public required PieceGrid Grid { get; set; }
    [Export] public required PieceMover PieceMover { get; set; }
    [Export] public required GameEngine GameEngine { get; set; }

    public bool RedAi { get; set; }
    public bool BlackAi { get; set; }

    public Vector2I? SelectedTile { get; set; }

    public int Round { get; set; }

    private BaseState CurrentState
    {
        get;
        set
        {
            field.Exit();
            field = value;
            field.Enter();
        }
    } = new();


    public override void _Ready()
    {
        GameEngine.Start();
        ToSignal(Owner, Node.SignalName.Ready).GetAwaiter().GetResult();
        ResetState();
    }

    public void ResetState()
    {
        SelectedTile = null;
        CurrentState = InitialState;
        SetProcess(true);
    }

    public override void _Process(double delta)
    {
        CurrentState.Process(delta);
    }

    private void OnClickBoard(Vector2I coords)
    {
        CurrentState.ClickBoard(coords);
    }

    public void SwitchTo(NodePath name)
    {
        if (!HasNode(name))
        {
            GD.PrintErr("Unknown state name: " + name);
            return;
        }

        var state = GetNode<BaseState>(name);
        CurrentState = state;
        GD.Print("Switched to state: " + name);
    }
}