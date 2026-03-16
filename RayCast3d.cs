using Godot;
using System;

public partial class RayCast3d : RayCast3D
{
	double fireDelay = 0.5;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("fire"))
		{
			if(fireDelay < 0)
			{
				Fire();
				fireDelay = 0.5;
			}	
		}
		if(fireDelay > 0)
			fireDelay -= delta;

	}

	private void Fire()
	{
		GD.Print("fire");
			if(IsColliding())
				if(GetCollider() is Enemy n)
					(n as Enemy).Death();
		GetTree().CreateTimer(0.5);
	}
}
