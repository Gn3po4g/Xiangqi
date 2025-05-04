using System.Collections.Generic;
using Godot;

namespace Xiangqi.Scripts.Pieces;

public enum PieceSide
{
    None,
    Red,
    Black
}

public partial class Piece : Node2D
{
    public virtual char Id => ' ';

    [Export] public PieceSide Side { get; set; } = PieceSide.None;

    public virtual IEnumerable<Vector2I> GetMovablePositions(PieceGrid grid, Vector2I coords) => [];
}