using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Xiangqi.Scripts.Pieces;

public partial class Shi : Piece
{
    public override char Id => Side == PieceSide.Black ? 'a' : 'A';

    public override IEnumerable<Vector2I> GetMovablePositions(PieceGrid grid, Vector2I coords)
    {
        return new[]
            {
                coords + Vector2I.Left + Vector2I.Up,
                coords + Vector2I.Right + Vector2I.Up,
                coords + Vector2I.Left + Vector2I.Down,
                coords + Vector2I.Right + Vector2I.Down
            }
            .Where(c => Side switch
            {
                PieceSide.Black => grid.BlackHome.HasPoint(c),
                PieceSide.Red => grid.RedHome.HasPoint(c),
                _ => false
            })
            .Where(c => grid[c]?.Side != Side);
    }
}