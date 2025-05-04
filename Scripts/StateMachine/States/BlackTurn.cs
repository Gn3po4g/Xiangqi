using Godot;
using Xiangqi.Scripts.Pieces;

namespace Xiangqi.Scripts.StateMachine.States;

public partial class BlackTurn : BaseState
{
    private bool _stateEnd;

    public override void Exit()
    {
        _stateEnd = false;
    }

    public override void Process(double delta)
    {
        if (_stateEnd)
        {
            StateMachine.SwitchTo(nameof(BlackSelect));
            return;
        }

        if (StateMachine.BlackAi)
        {
            StateMachine.SwitchTo(nameof(BlackAi));
        }
    }

    public override void ClickBoard(Vector2I tile)
    {
        if (StateMachine.Grid[tile] is not { Side: PieceSide.Black }) return;
        StateMachine.SelectedTile = tile;
        _stateEnd = true;
    }
}