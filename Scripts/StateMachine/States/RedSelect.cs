using System.Linq;
using Godot;
using Xiangqi.Scripts.Pieces;

namespace Xiangqi.Scripts.StateMachine.States;

public partial class RedSelect : BaseState
{
    private Vector2I[] _validMoves = [];
    private bool _stateEnd;

    public override void Enter()
    {
        var selectedTile = StateMachine.SelectedTile;
        if (selectedTile.HasValue && StateMachine.Grid[selectedTile.Value] is { Side: PieceSide.Red })
        {
            _validMoves = StateMachine.PieceMover.ReadyToMove(selectedTile.Value);
        }
        else
        {
            GD.PrintErr("You must select a red piece. Change to state [RedTurn]");
            StateMachine.SwitchTo(nameof(RedTurn));
        }
    }

    public override void Exit()
    {
        _stateEnd = false;
    }

    public override void Process(double delta)
    {
        if (_stateEnd)
        {
            StateMachine.SwitchTo(StateMachine.BlackAi ? nameof(BlackAi) : nameof(BlackTurn));
            return;
        }

        if (StateMachine.RedAi)
        {
            StateMachine.SwitchTo(nameof(RedAi));
        }
    }

    public override void ClickBoard(Vector2I tile)
    {
        if (StateMachine.PieceMover.IsMoving) return;
        if (StateMachine.Grid[tile] is { Side: PieceSide.Red })
        {
            StateMachine.SelectedTile = tile;
            StateMachine.SwitchTo(nameof(RedSelect));
        }
        else if (_validMoves.Contains(tile) && StateMachine.SelectedTile.HasValue)
        {
            var selectedTile = StateMachine.SelectedTile.Value;
            StateMachine.SetProcess(false);
            StateMachine.PieceMover.MovePiece(selectedTile, tile, () =>
            {
                _stateEnd = true;
                StateMachine.SetProcess(true);
            });
        }
    }
}