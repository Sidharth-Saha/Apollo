using Godot;
using System;
using System.Diagnostics;

public partial class Player : CharacterBody2D
{
	[Export]
	public float Speed = 50.0f;

	private static Vector2[] validMovementDirections =
	{
		Vector2.Up,
		Vector2.Right,
		Vector2.Down,
		Vector2.Left,
		(Vector2.Up + Vector2.Right).Normalized(),
		(Vector2.Up + Vector2.Left).Normalized(),
		(Vector2.Down + Vector2.Right).Normalized(),
		(Vector2.Down + Vector2.Left).Normalized()
	};

	private AnimatedSprite2D animatedSprite2D;

	private void ProcessMovementInput()
	{
		Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
		// Pick the valid movement direction that is closest to the input direction
		Vector2 movementDirection = Vector2.Zero;
		if (inputDirection.LengthSquared() > 0.0f)
		{
			float maxDotProduct = 0.0f;
			foreach (Vector2 direction in validMovementDirections)
			{
				float dotProduct = inputDirection.Dot(direction);
				if (dotProduct > maxDotProduct)
				{
					maxDotProduct = dotProduct;
					movementDirection = direction;
				}
			}
		}
		Velocity = movementDirection * Speed;
		if (Velocity.LengthSquared() > 0.0f)
		{
			if (Velocity.Y > 0.0f)
			{
				animatedSprite2D.Play("down");
			}
            else if (Velocity.Y < 0.0f)
			{
				animatedSprite2D.Play("up");
			}
			else if (Velocity.X > 0.0f)
			{
				animatedSprite2D.Play("right");
			}
			else
			{
				animatedSprite2D.Play("left");
			}
        }
		else
		{
			animatedSprite2D.Stop();
			animatedSprite2D.Frame = 0;
		}
    }

    public override void _Ready()
    {
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Debug.Assert(animatedSprite2D != null);
    }

    public override void _PhysicsProcess(double delta)
    {
		ProcessMovementInput();
		MoveAndSlide();
    }
    /*
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
	*/
}
