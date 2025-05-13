using System.Text.RegularExpressions;
using Godot;

namespace Xiangqi.Scripts.StateMachine;

public partial class BaseState : Node
{
    protected StateMachine StateMachine => GetParent<StateMachine>();

    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
    }

    public virtual void Process(double delta)
    {

    }

    public virtual void ClickBoard(Vector2I coords)
    {
    }

    [GeneratedRegex(@"^bestmove ([a-i][0-9][a-i][0-9]).*$")]
    private static partial Regex MovePattern();

    protected readonly Regex MoveRegex = MovePattern();
}