using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Xiangqi.Scripts.Pieces;

public partial class Xiang : Piece
{
    public override char Id => Side == PieceSide.Black ? 'b' : 'B';

    public override IEnumerable<Vector2I> GetMovablePositions(PieceGrid grid, Vector2I coords)
    {
        return new[]
            {
                (Vector2I.Left + Vector2I.Up) * 2,
                (Vector2I.Right + Vector2I.Up) * 2,
                (Vector2I.Left + Vector2I.Down) * 2,
                (Vector2I.Right + Vector2I.Down) * 2
            }
            .Where(d => Side switch
            {
                PieceSide.Black => grid.BlackSide.HasPoint(coords + d),
                PieceSide.Red => grid.RedSide.HasPoint(coords + d),
                _ => false
            })
            .Where(d => grid[coords + d / 2]?.Side == null)
            .Where(d => grid[coords + d]?.Side != Side)
            .Select(d => coords + d);
    }
}