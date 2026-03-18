using Godot;
using System;

public partial class Pistol : Gun
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		fireRate = 0.5d;
		damage = 50;
	}
}
