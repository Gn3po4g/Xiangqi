using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Xiangqi.Scripts.Pieces;

public partial class Bing : Piece
{
    public override char Id => Side == PieceSide.Black ? 'p' : 'P';

    public override IEnumerable<Vector2I> GetMovablePositions(PieceGrid grid, Vector2I coords)
    {
        return GetTowards(grid, coords)
            .Where(grid.Boundary.HasPoint)
            .Where(c => grid[c]?.Side != Side);
    }

    private IEnumerable<Vector2I> GetTowards(PieceGrid grid, Vector2I coords)
    {
        switch (Side)
        {
            case PieceSide.Black:
            {
                yield return coords + Vector2I.Down;
                if (grid.BlackSide.HasPoint(coords)) yield break;
                break;
            }
            case PieceSide.Red:
            {
                yield return coords + Vector2I.Up;
                if (grid.RedSide.HasPoint(coords)) yield break;
                break;
            }
            case PieceSide.None:
            default: yield break;
        }

        yield return coords + Vector2I.Left;
        yield return coords + Vector2I.Right;
    }
}