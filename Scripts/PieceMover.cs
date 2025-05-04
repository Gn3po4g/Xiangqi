using System;
using System.Linq;
using Godot;

namespace Xiangqi.Scripts;

public partial class PieceMover : Node
{
    [Export] private float _moveTime = 1f;
    private Tween? _moveTween;

    [Export] public required PieceLayer PieceMapLayer { get; set; }
    [Export] public required PieceGrid Grid { get; set; }

    public bool IsMoving => _moveTween?.IsRunning() ?? false;

    public Vector2I[] ReadyToMove(Vector2I tile)
    {
        if (Grid[tile] is not { } selectedPiece) return [];
        var validTiles = selectedPiece.GetMovablePositions(Grid, tile).ToArray();
        PieceMapLayer.ShowMoveIndication(tile, validTiles);
        return validTiles;
    }

    public void MovePiece(Vector2I from, Vector2I to, Action? onFinish = null)
    {
        var piece = Grid[from];
        var target = Grid[to];
        if (piece == null) return;
        var zIndex = piece.ZIndex;
        piece.ZIndex = 99;
        Grid[from] = null;
        Grid[to] = piece;
        _moveTween?.Kill();
        _moveTween = piece.CreateTween().SetEase(Tween.EaseType.InOut);
        _moveTween.TweenProperty(
            piece,
            new NodePath(Node2D.PropertyName.Position),
            PieceMapLayer.MapToLocal(to),
            _moveTime
        );
        _moveTween.TweenCallback(Callable.From(() =>
        {
            piece.ZIndex = zIndex;
            target?.QueueFree();
            onFinish?.Invoke();
        }));
        PieceMapLayer.ShowMove(from, to);
    }
}