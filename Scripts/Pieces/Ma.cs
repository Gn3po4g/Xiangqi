using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Xiangqi.Scripts.Pieces;

public partial class Ma : Piece
{
    public override char Id => Side == PieceSide.Black ? 'n' : 'N';

    public override IEnumerable<Vector2I> GetMovablePositions(PieceGrid grid, Vector2I coords)
    {
        return new[]
            {
                Vector2I.Left + Vector2I.Up * 2,
                Vector2I.Right + Vector2I.Up * 2,
                Vector2I.Left + Vector2I.Down * 2,
                Vector2I.Right + Vector2I.Down * 2,
                Vector2I.Left * 2 + Vector2I.Up,
                Vector2I.Right * 2 + Vector2I.Up,
                Vector2I.Left * 2 + Vector2I.Down,
                Vector2I.Right * 2 + Vector2I.Down
            }
            .Where(d => grid.Boundary.HasPoint(coords + d))
            .Where(d => grid[coords + d / 2]?.Side == null)
            .Where(d => grid[coords + d]?.Side != Side)
            .Select(d => coords + d);
    }
}