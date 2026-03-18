using Godot;
using System;

public partial class Gun : Node3D
{
	protected double fireRate = 0.0d;
	protected int magCount = 0;
	protected int damage = 0;

	private double fireDelay;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		fireDelay = fireRate;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(fireDelay > 0)
			fireDelay -= delta;
	}

	public void Fire(RayCast3D raycast3D)
	{
		if(fireDelay <= 0.001)
		{
			GD.Print("fire");
			if(raycast3D.IsColliding())
				if(raycast3D.GetCollider() is Enemy n)
					(n as Enemy).Hit(damage);
			fireDelay = fireRate;
		}
	}
}
