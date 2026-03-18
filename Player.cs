using Godot;
using System;
using System.Diagnostics;

public partial class Player : CharacterBody3D
{
	private Camera3D camera;
	private RayCast3D raycast3D;
	private Gun gun;
	
	public float Speed = 5.0f;
	public float Boost = 1.0f;
	public const float JumpVelocity = 4.5f;

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
		camera = GetNode<Camera3D>("Camera3D");
		raycast3D = camera.GetNode<RayCast3D>("RayCast3D");
		gun = camera.GetNode<Node3D>("GunHolder").GetNode<Gun>("Gun");
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if(@event is InputEventMouseMotion mouseMotion){
			RotateY(-mouseMotion.Relative.X * 0.002f);
			if(camera.Rotation.X-(mouseMotion.Relative.Y * 0.002f) > -1 && camera.Rotation.X-(mouseMotion.Relative.Y * 0.002f) < 1)
				camera.RotateX(-mouseMotion.Relative.Y * 0.002f);
		}
	}

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("fire"))
		{
			gun.Fire(raycast3D);	
		}
    }

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Handle Jump.
		if (Input.IsActionPressed("run") && !Input.IsActionPressed("crouch"))
		{
			Boost = 2.0f;
		}
		else
			if(IsOnFloor())
				Boost = 1.0f;

		// Handle Jump.
		if (Input.IsActionPressed("crouch"))
		{
			camera.Position = new Vector3(camera.Position.X, 0.248f, camera.Position.Z);
			Speed = 2.5f;
		}
		else{
			camera.Position = new Vector3(camera.Position.X, 0.748f, camera.Position.Z);
			Speed = 5.0f;
			}


		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_back");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed * Boost;
			velocity.Z = direction.Z * Speed * Boost;
		}
		else
		{
			if(IsOnFloor()){
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
				velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
			}
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
