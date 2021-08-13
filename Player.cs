using Godot;
using System;

public class Player : Area2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	[Export]
	public int Speed = 400;
	[Signal]	
	public delegate void Hit();
	
	private Vector2 _screenSize;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_screenSize = GetViewport().Size;
		Hide();
	}
	
	public override void _Process(float delta)
	{
		var velocity = new Vector2();

		if (Input.IsActionPressed("ui_right"))
		{
			velocity.x += 1;
		}
		if (Input.IsActionPressed("ui_left"))
		{
			velocity.x -= 1;
		}
		if (Input.IsActionPressed("ui_up"))
		{
			velocity.y -= 1;
		}
		if (Input.IsActionPressed("ui_down"))
		{
			velocity.y += 1;
		}

		var animSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		if (velocity.Length() > 0)
		{
			if (velocity.x != 0)
			{
				animSprite.Animation = "walk";
				animSprite.FlipV = false;
				animSprite.FlipH = velocity.x < 0;
			}
			else if (velocity.y != 0)
			{
				animSprite.Animation = "up";
				animSprite.FlipV = velocity.y > 0;
				animSprite.FlipH = false;
			}
			
			velocity = velocity.Normalized() * Speed;
			animSprite.Play();
		}
		else
		{
			animSprite.Stop();
		}

		Position += velocity * delta;
		Position = new Vector2(
			x:Mathf.Clamp(Position.x, 0, _screenSize.x),
			y:Mathf.Clamp(Position.y, 0, _screenSize.y)
		);
	}

	public void Start(Vector2 pos)
	{
		Position = pos;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	public void OnPlayerBodyEntered(PhysicsBody2D body)
	{
		Hide();
		EmitSignal("Hit");
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", false);
	}
	
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

