using Godot;
using System;

public partial class Gun : Node3D
{
	protected double fireRate = 0.0d;
	protected int magCount = 0;
	protected int currentMagCount;
	protected int damage = 0;

	private double reloadDelay = 2.5;

	private double fireDelay;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		fireDelay = fireRate;
		currentMagCount = magCount;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(fireDelay > 0)
			fireDelay -= delta;
		if(reloadDelay > 0 && reloadDelay < 2)
			reloadDelay -= delta;
		if(reloadDelay <= 0.001)
		{
			currentMagCount = magCount;
			reloadDelay = 2.5;
		}
	}

	public void Fire(RayCast3D raycast3D)
	{
		if(fireDelay <= 0.001 && currentMagCount > 0)
		{
			GD.Print("fire");
			if(raycast3D.IsColliding())
				if(raycast3D.GetCollider() is Enemy n){
					raycast3D.SetCollisionMaskValue(2,true);
					raycast3D.ForceRaycastUpdate();				
					(n as Enemy).Hit(damage,(HitArea)((int)raycast3D.GetCollider().GetMeta("HitArea", -1)));
					raycast3D.SetCollisionMaskValue(2,false);
				}
			fireDelay = fireRate;
			currentMagCount -= 1;
		}
		else if(fireDelay <= 0.001 && currentMagCount == 0)
			Reload();
	}

	public void Reload()
	{
		reloadDelay = 1.5;
	}

	public int GetMagCount()
	{
		return currentMagCount;
	}
}
