using Godot;
using System;

public partial class Rifle : Gun
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		fireRate = 0.01d;
		damage = 10;
		magCount = 30;
		base._Ready();
	}

}
