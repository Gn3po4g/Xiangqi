using Godot;
using Xiangqi.Scripts.Pieces;

namespace Xiangqi.Scripts.StateMachine.States;

public partial class RedAi : BaseState
{
    private Piece? _selectedPiece;
    private bool _stateEnd;

    public override void Enter()
    {
        StateMachine.GameEngine.EngineOutputted += OnEngineOutput;
        StateMachine.GameEngine.SetFen($"{StateMachine.Grid.GetGridFen()} w - - 0 {StateMachine.Round}");
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
            StateMachine.SwitchTo(StateMachine.BlackAi ? nameof(BlackAi) : nameof(BlackTurn));
            return;
        }

        if (!StateMachine.RedAi)
        {
            StateMachine.SwitchTo(nameof(RedSelect));
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