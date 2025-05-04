using System.Diagnostics;
using Godot;

namespace Xiangqi.Scripts.Engine;

public partial class Pikafish : GameEngine
{
    private Process? _process;

    [Export(PropertyHint.File)] private string _enginePath = "res://Engine/pikafish-avx2.exe";

    public bool IsRunning => _process is { HasExited: false };

    private void Send(string message)
    {
        if (_process is { HasExited: false })
        {
            _process.StandardInput.WriteLine(message);
        }
    }

    public override void Start()
    {
        var startInfo = new ProcessStartInfo()
        {
            FileName = ProjectSettings.GlobalizePath(_enginePath),
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        _process = new Process() { StartInfo = startInfo };
        _process.OutputDataReceived += (_, e) =>
        {
            if (e.Data == null) return;
            var lines = e.Data.Split('\n');
            foreach (var line in lines)
            {
                base.OnEngineOutput(line);
            }
        };
        _process.Start();
        _process.BeginOutputReadLine();
        GD.Print("pikafish ready");
    }

    public override void SetFen(string fen)
    {
        Send("position fen " + fen);
    }

    public override void CalcMove()
    {
        Send("go depth 15");
    }
}