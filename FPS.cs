using Godot;
using System;

public partial class FPS : Label
{

    public override void _Process(double delta)
    {
        this.Text = $"{Engine.GetFramesPerSecond()} FPS";
    }
}
