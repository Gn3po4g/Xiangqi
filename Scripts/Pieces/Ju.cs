using System.Collections.Generic;
using Godot;

namespace Xiangqi.Scripts.Pieces;

public partial class Ju : Piece
{
    public override char Id => Side == PieceSide.Black ? 'r' : 'R';

    public override IEnumerable<Vector2I> GetMovablePositions(PieceGrid grid, Vector2I coords)
    {
        var directions = new[]
        {
            Vector2I.Left, Vector2I.Right, Vector2I.Up, Vector2I.Down
        };
        foreach (var dir in directions)
        {
            for (var start = coords + dir; grid.Boundary.HasPoint(start); start += dir)
            {
                if (grid[start] is { } piece)
                {
                    if (piece.Side != Side) yield return start;
                    break;
                }

                yield return start;
            }
        }
    }
}