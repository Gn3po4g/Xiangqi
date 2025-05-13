using System.Linq;
using Godot;
using Xiangqi.Scripts.Pieces;

namespace Xiangqi.Scripts.StateMachine.States;

public partial class SelectState : BaseState
{
    private Vector2I[] _validMoves = [];
    private bool _stateEnd;

    [Export] public PieceSide Side { get; set; } = PieceSide.None;
    [Export] public NodePath AiState { get; set; } = "";
    [Export] public NodePath NextState { get; set; } = "";
    [Export] public NodePath StartState { get; set; } = "";

    public override void Enter()
    {
        var selectedTile = StateMachine.SelectedTile;
        if (selectedTile.HasValue && StateMachine.Grid[selectedTile.Value]?.Side == Side)
        {
            _validMoves = StateMachine.PieceMover.ReadyToMove(selectedTile.Value);
        }
        else
        {
            GD.PrintErr($"No piece selected, fallback to [{StartState}]");
            StateMachine.SwitchTo(GetNode(StartState).GetPath());
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
            StateMachine.SwitchTo(GetNode(NextState).GetPath());
            return;
        }

        if (Side == PieceSide.Red && StateMachine.RedAi ||
            Side == PieceSide.Black && StateMachine.BlackAi)
        {
            StateMachine.SwitchTo(GetNode(AiState).GetPath());
        }
    }

    public override void ClickBoard(Vector2I tile)
    {
        if (StateMachine.PieceMover.IsMoving) return;
        if (StateMachine.Grid[tile]?.Side == Side)
        {
            StateMachine.SelectedTile = tile;
            StateMachine.SwitchTo(GetPath());
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