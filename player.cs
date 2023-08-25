using System;
using Godot;

public partial class player : CharacterBody3D
{
    [Signal]
    public delegate void HitEventHandler();


    [Export]
    public int Speed { get; set; } = 14;

    [Export]
    public int FallAcceleration { get; set; } = 75;

    [Export]
    public int JumpImpulse { get; set; } = 20;

    [Export]
    public int BounceImpulse { get; set; } = 20;


    private Vector3 _targetVelocity = Vector3.Zero;

    public override void _Ready()
    {
        var mobDetector = GetNode<Area3D>("MobDetector");
        mobDetector.BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node3D body)
    {
        Die();
    }

    private void Die()
    {
        EmitSignal(SignalName.Hit);
        QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
        var pivot = GetNode<Node3D>("Pivot");
        var direction = Vector3.Zero;

        if (Input.IsActionPressed("move_right"))
            direction.X += 1;
        if (Input.IsActionPressed("move_left"))
            direction.X -= 1;

        if (Input.IsActionPressed("move_back"))
            direction.Z += 1;
        if (Input.IsActionPressed("move_forward"))
            direction.Z -= 1;

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            pivot.LookAt(Position + direction, Vector3.Up);
            GetNode<AnimationPlayer>("AnimationPlayer").SpeedScale = 4;
        }
        else
        {
            GetNode<AnimationPlayer>("AnimationPlayer").SpeedScale = 1;
        }

        _targetVelocity.X = direction.X * Speed;
        _targetVelocity.Z = direction.Z * Speed;

        if (!IsOnFloor())
        {
            _targetVelocity.Y -= FallAcceleration * (float)delta;
        }

        if (IsOnFloor() && Input.IsActionJustPressed("jump"))
        {
            _targetVelocity.Y = JumpImpulse;
        }

        for (var i = 0; i < GetSlideCollisionCount(); i++)
        {
            var collision = GetSlideCollision(i);

            if (collision.GetCollider() is mob mob)
            {
                if (Vector3.Up.Dot(collision.GetNormal()) > 0.1)
                {
                    mob.Squash();
                    _targetVelocity.Y = BounceImpulse;
                }

            }
        }

        Velocity = _targetVelocity;
        MoveAndSlide();

        var rotation = pivot.Rotation;
        rotation.X = (float)Math.PI / 6 * Velocity.Y / JumpImpulse;
        pivot.Rotation = rotation;
    }
}
