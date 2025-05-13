using Godot;
using Xiangqi.Scripts.Pieces;

namespace Xiangqi.Scripts.StateMachine.States;

public partial class AiState : BaseState
{
    private bool _stateEnd;

    [Export] public PieceSide Side { get; set; } = PieceSide.None;
    [Export] public NodePath NextState { get; set; } = "";
    [Export] public NodePath StartState { get; set; } = "";

    public override void Enter()
    {
        StateMachine.GameEngine.EngineOutputted += OnEngineOutput;
        StateMachine.GameEngine.SetFen($"{StateMachine.Grid.GetGridFen()} {(Side == PieceSide.Red ? 'w' : 'b')} - - 0 {StateMachine.Round}");
        StateMachine.GameEngine.CalcMove();
    }

    public override void Exit()
    {
        _stateEnd = false;
        StateMachine.GameEngine.EngineOutputted -= OnEngineOutput;
    }

    public override void Process(double delta)
    {
        if (_stateEnd)
        {
            StateMachine.SwitchTo(GetNode(NextState).GetPath());
            return;
        }

        if (Side == PieceSide.Red && !StateMachine.RedAi ||
            Side == PieceSide.Black && !StateMachine.BlackAi)
        {
            StateMachine.SwitchTo(GetNode(StartState).GetPath());
        }
    }

    private void OnEngineOutput(string msg)
    {
        if (!MoveRegex.IsMatch(msg)) return;
        var move = MoveRegex.Match(msg).Groups[1].Value;
        var from = new Vector2I(move[0] - 'a', '9' - move[1]);
        var to = new Vector2I(move[2] - 'a', '9' - move[3]);
        Callable.From(() =>
        {
            StateMachine.SetProcess(false);
            StateMachine.PieceMover.MovePiece(from, to, () =>
            {
                _stateEnd = true;
                StateMachine.SetProcess(true);
            });
        }).CallDeferred();
    }
}