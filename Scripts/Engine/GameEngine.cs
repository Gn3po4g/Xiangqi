using Godot;

namespace Xiangqi.Scripts.Engine;

public delegate void EngineMessageHandler(string message);

public abstract partial class GameEngine : Node
{
    public event EngineMessageHandler? EngineOutputted;

    protected virtual void OnEngineOutput(string msg)
    {
        EngineOutputted?.Invoke(msg);
    }

    public abstract void Start();

    public abstract void SetFen(string fen);

    public abstract void CalcMove();
}