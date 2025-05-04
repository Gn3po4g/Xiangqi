using Godot;
using Xiangqi.Scripts.Pieces;

namespace Xiangqi.Scripts.StateMachine.States;

public partial class RedTurn : BaseState
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
            StateMachine.SwitchTo(nameof(RedSelect));
            return;
        }

        if (StateMachine.RedAi)
        {
            StateMachine.SwitchTo(nameof(RedAi));
        }
    }

    public override void ClickBoard(Vector2I tile)
    {
        if (StateMachine.Grid[tile] is not { Side: PieceSide.Red }) return;
        StateMachine.SelectedTile = tile;
        _stateEnd = true;
    }
}