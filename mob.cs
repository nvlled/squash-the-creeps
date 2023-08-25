using Godot;
using System;

public partial class mob : CharacterBody3D
{
    [Signal]
    public delegate void SquashedEventHandler();

    [Export]
    public int MinSpeed { get; set; } = 10;
    [Export]
    public int MaxSpeed { get; set; } = 18;

    public override void _Ready()
    {
        var visibilityNotifier = GetNode<VisibleOnScreenNotifier3D>("VisibleOnScreenNotifier3D");
        visibilityNotifier.ScreenExited += onScreenExit;
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }

    public void Initialize(Vector3 startPos, Vector3 playerPos)
    {
        LookAtFromPosition(startPos, playerPos, Vector3.Up);
        RotateY((float)GD.RandRange(-Mathf.Pi / 4.0, Mathf.Pi / 4.0));

        var randSpeed = GD.RandRange(MinSpeed, MaxSpeed);
        this.Velocity = (Vector3.Forward * randSpeed).Rotated(Vector3.Up, this.Rotation.Y);
        GetNode<AnimationPlayer>("AnimationPlayer").SpeedScale = randSpeed / MinSpeed;
    }

    private void onScreenExit()
    {
        QueueFree();
    }

    public void Squash()
    {
        EmitSignal(SignalName.Squashed);
        QueueFree();
    }
}
