using Godot;

namespace Xiangqi.Scripts;

public partial class PieceLayer : TileMapLayer
{
    private PieceGrid? _grid;

    [Signal]
    public delegate void BoardClickedEventHandler(Vector2I coords);

    public override void _Ready()
    {
        _grid = GetNode<PieceGrid>("PieceGrid");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!@event.IsActionPressed("Click") || _grid == null) return;
        var clickedCell = LocalToMap(GetLocalMousePosition());
        if (_grid.Boundary.HasPoint(clickedCell))
        {
            EmitSignalBoardClicked(clickedCell);
        }
    }

    public void ShowMoveIndication(Vector2I from, Vector2I[] moves)
    {
        Clear();
        if (_grid == null) return;
        SetCell(from, 0, Vector2I.Zero);
        foreach (var move in moves)
        {
            SetCell(move, _grid[move] == null ? 1 : 2, Vector2I.Zero);
        }
    }

    public void ShowMove(Vector2I from, Vector2I to)
    {
        Clear();
        SetCell(from, 3, Vector2I.Zero);
        SetCell(to, 4, Vector2I.Zero);
    }
}