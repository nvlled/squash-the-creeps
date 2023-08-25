using Godot;
using System;

public partial class main : Node
{
    [Export]
    public PackedScene MobScene { get; set; }

    public override void _Ready()
    {
        var mobTimer = GetNode<Timer>("MobTimer");
        mobTimer.Timeout += onMobTimerTimeout;

        GetNode<Control>("UserInterface/Retry").Hide();
        GetNode<player>("Player").Hit += OnPlayerHit;
    }

    public override void _UnhandledInput(InputEvent @event)
    {

        if (@event.IsActionPressed("ui_accept") && GetNode<Control>("UserInterface/Retry").Visible)
        {
            GetTree().ReloadCurrentScene();
        }
    }

    private void OnPlayerHit()
    {
        GetNode<Timer>("MobTimer").Stop();
        GetNode<Control>("UserInterface/Retry").Show();
    }

    private void onMobTimerTimeout()
    {
        var mob = MobScene.Instantiate<mob>();
        var spawnLocation = GetNode<PathFollow3D>("SpawnPath/SpawnLocation");
        spawnLocation.ProgressRatio = GD.Randf();

        var playerPos = GetNode<player>("Player").Position;
        mob.Initialize(spawnLocation.Position, playerPos);

        var label = GetNode<ScoreLabel>("UserInterface/ScoreLabel");
        mob.Squashed += label.OnMobSquashed;

        AddChild(mob);
    }
}
