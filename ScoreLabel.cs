using Godot;
using System;

public partial class ScoreLabel : Label
{
    private int score = 0;

    internal void OnMobSquashed()
    {
        score++;
        Text = $"Score: {score}";
    }
}
