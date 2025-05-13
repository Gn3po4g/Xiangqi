using Godot;
using Xiangqi.Scripts.Pieces;

namespace Xiangqi.Scripts.StateMachine.States;

public partial class StartState : BaseState
{
    private bool _stateEnd;

    [Export] public PieceSide Side { get; set; } = PieceSide.None;
    [Export] public NodePath AiState { get; set; } = "";
    [Export] public NodePath SelectState { get; set; } = "";

    public override void Exit()
    {
        _stateEnd = false;
    }

    public override void Process(double delta)
    {
        if (_stateEnd)
        {
            StateMachine.SwitchTo(GetNode(SelectState).GetPath());
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
        if (StateMachine.Grid[tile]?.Side != Side) return;
        StateMachine.SelectedTile = tile;
        _stateEnd = true;
    }
}