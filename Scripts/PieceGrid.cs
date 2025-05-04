using System.Collections.Frozen;
using System.Collections.Generic;
using System.Text;
using Godot;
using Xiangqi.Scripts.Pieces;

namespace Xiangqi.Scripts;

public partial class PieceGrid : Node2D
{
    private readonly FrozenDictionary<char, PackedScene> _typeDic = new Dictionary<char, PackedScene>
        {
            ['k'] = GD.Load<PackedScene>("res://Scenes/Board/Pieces/BK.tscn"),
            ['K'] = GD.Load<PackedScene>("res://Scenes/Board/Pieces/WK.tscn"),
            ['a'] = GD.Load<PackedScene>("res://Scenes/Board/Pieces/BA.tscn"),
            ['A'] = GD.Load<PackedScene>("res://Scenes/Board/Pieces/WA.tscn"),
            ['b'] = GD.Load<PackedScene>("res://Scenes/Board/Pieces/BB.tscn"),
            ['B'] = GD.Load<PackedScene>("res://Scenes/Board/Pieces/WB.tscn"),
            ['n'] = GD.Load<PackedScene>("res://Scenes/Board/Pieces/BN.tscn"),
            ['N'] = GD.Load<PackedScene>("res://Scenes/Board/Pieces/WN.tscn"),
            ['r'] = GD.Load<PackedScene>("res://Scenes/Board/Pieces/BR.tscn"),
            ['R'] = GD.Load<PackedScene>("res://Scenes/Board/Pieces/WR.tscn"),
            ['c'] = GD.Load<PackedScene>("res://Scenes/Board/Pieces/BC.tscn"),
            ['C'] = GD.Load<PackedScene>("res://Scenes/Board/Pieces/WC.tscn"),
            ['p'] = GD.Load<PackedScene>("res://Scenes/Board/Pieces/BP.tscn"),
            ['P'] = GD.Load<PackedScene>("res://Scenes/Board/Pieces/WP.tscn"),
        }
        .ToFrozenDictionary();

    private PieceLayer? _pieceLayer;

    private readonly Dictionary<Vector2I, Piece?> _pieces = new();

    private readonly Vector2I _size = new(9, 10);

    public Rect2I Boundary => new(0, 0, _size);

    public Rect2I BlackSide => new(0, 0, _size / new Vector2I(1, 2));

    public Rect2I RedSide => new(0, 5, _size / new Vector2I(1, 2));

    public Rect2I BlackHome => new(3, 0, 3, 3);

    public Rect2I RedHome => new(3, 7, 3, 3);

    public Piece? this[Vector2I coord]
    {
        get => _pieces[coord];
        set => _pieces[coord] = value;
    }

    public override void _Ready()
    {
        _pieceLayer = GetParent<PieceLayer>();
        InitBoard();
    }

    private void ClearBoard()
    {
        for (var i = 0; i < _size.X; i++)
        {
            for (var j = 0; j < _size.Y; j++)
            {
                _pieces[new Vector2I(i, j)] = null;
            }
        }
    }

    public void InitBoard()
    {
        SetGrid("rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 1");
    }

    public void SetGrid(string fen)
    {
        if (_pieceLayer == null) return;

        foreach (var child in GetChildren())
        {
            child.QueueFree();
        }

        ClearBoard();

        var layout = fen.Split(' ')[0];
        int row = 0, col = 0;
        foreach (var ch in layout)
        {
            if (char.IsDigit(ch))
            {
                col += ch - '0';
            }
            else if (ch == '/')
            {
                row++;
                col = 0;
            }
            else
            {
                if (_typeDic.TryGetValue(ch, out var value))
                {
                    var pos = new Vector2I(col, row);
                    var piece = value.Instantiate<Piece>();
                    piece.Position = _pieceLayer.MapToLocal(pos);
                    _pieces[pos] = piece;
                    AddChild(piece);
                }

                col++;
            }
        }
    }

    public string GetGridFen()
    {
        var lines = new List<string>();
        for (var row = 0; row < _size.Y; row++)
        {
            var sb = new StringBuilder();
            var emptyCount = 0;
            for (var col = 0; col < _size.X; col++)
            {
                if (_pieces[new Vector2I(col, row)] is { } piece)
                {
                    if (emptyCount > 0) sb.Append(emptyCount);
                    sb.Append(piece.Id);
                    emptyCount = 0;
                }
                else
                {
                    emptyCount++;
                }
            }

            if (emptyCount > 0)
            {
                sb.Append(emptyCount);
            }

            lines.Add(sb.ToString());
            sb.Clear();
        }

        return string.Join('/', lines);
    }
}