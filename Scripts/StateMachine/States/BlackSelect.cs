using System.Linq;
using Godot;
using Xiangqi.Scripts.Pieces;

namespace Xiangqi.Scripts.StateMachine.States;

public partial class BlackSelect : BaseState
{
    private Vector2I[] _validMoves = [];
    private bool _stateEnd;

    public override void Enter()
    {
        var selectedTile = StateMachine.SelectedTile;
        if (selectedTile.HasValue && StateMachine.Grid[selectedTile.Value] is { Side: PieceSide.Black })
        {
            _validMoves = StateMachine.PieceMover.ReadyToMove(selectedTile.Value);
        }
        else
        {
            GD.PrintErr("You must select a black piece. Change to state [BlackTurn]");
            StateMachine.SwitchTo(nameof(BlackTurn));
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
            StateMachine.SwitchTo(StateMachine.RedAi ? nameof(RedAi) : nameof(RedTurn));
            return;
        }

        if (StateMachine.BlackAi)
        {
            StateMachine.SwitchTo(nameof(BlackAi));
        }
    }

    public override void ClickBoard(Vector2I tile)
    {
        if (StateMachine.PieceMover.IsMoving) return;
        if (StateMachine.Grid[tile] is { Side: PieceSide.Black })
        {
            StateMachine.SelectedTile = tile;
            StateMachine.SwitchTo(nameof(BlackSelect));
        }
        else if (_validMoves.Contains(tile) && StateMachine.SelectedTile.HasValue)
        {
            var selectedTile = StateMachine.SelectedTile.Value;
            StateMachine.SetProcess(false);
            StateMachine.PieceMover.MovePiece(selectedTile, tile, () =>
            {
                _stateEnd = true;
                StateMachine.SetProcess(true);
                StateMachine.Round++;
            });
        }
    }
}