using Godot;
using System;

public partial class RayCast3d : RayCast3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("fire"))
		{
			GD.Print("fire");
			if(IsColliding())
				GD.Print("trafiony");

		}

	}
}
