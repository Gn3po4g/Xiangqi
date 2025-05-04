using System.Collections.Generic;
using Godot;

namespace Xiangqi.Scripts.Pieces;

public partial class Pao : Piece
{
    public override char Id => Side == PieceSide.Black ? 'c' : 'C';

    public override IEnumerable<Vector2I> GetMovablePositions(PieceGrid grid, Vector2I coords)
    {
        var directions = new[]
        {
            Vector2I.Left, Vector2I.Right, Vector2I.Up, Vector2I.Down
        };
        foreach (var dir in directions)
        {
            var start = coords + dir;
            for (; grid.Boundary.HasPoint(start); start += dir)
            {
                if (grid[start] is not null)
                {
                    start += dir;
                    break;
                }

                yield return start;
            }

            for (; grid.Boundary.HasPoint(start); start += dir)
            {
                if (grid[start] is not { } piece) continue;

                if (piece.Side != Side)
                {
                    yield return start;
                }

                break;
            }
        }
    }
}