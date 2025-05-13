using Godot;
using Xiangqi.Scripts;
using Xiangqi.Scripts.StateMachine;

namespace Xiangqi.Scenes.Board;

public partial class Board : Node2D
{
    private StateMachine? _stateMachine;
    private PieceLayer? _pieceLayer;
    private PieceGrid? _grid;

    public override void _Ready()
    {
        _stateMachine = GetNode<StateMachine>("StateMachine");
        _pieceLayer = GetNode<PieceLayer>("Pieces");
        _grid = GetNode<PieceGrid>("Pieces/PieceGrid");
    }

    private void ResetBoard()
    {
        _pieceLayer?.Clear();
        _grid?.InitBoard();
        _stateMachine?.ResetState();
    }

    private void ToggleRedAi(bool value)
    {
        if (_stateMachine != null)
        {
            _stateMachine.RedAi = value;
        }
    }

    private void ToggleBlackAi(bool value)
    {
        if (_stateMachine != null)
        {
            _stateMachine.BlackAi = value;
        }
    }
}