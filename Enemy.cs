using Godot;
using System;

public partial class Enemy : Node3D
{

	double delay = 0.1;
	double timespan = 0.0;
	RayCast3D raycast3D;
	bool lockedIn = false;
	Player player;
	private int health = 100;


	//AI stuff
	EnemyState enemyState = EnemyState.Idle;
	Vector3 newPosition;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		raycast3D = GetNode<RayCast3D>("RayCast3D");
		newPosition = GlobalPosition;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		switch (enemyState){
			case EnemyState.Search:
				{
					MoveToRandomPostion();
					break;
				}
		}

		/*
		timespan += delta;
		if(timespan > delay && !lockedIn){
			RotateY(0.1f);
			timespan = 0;
		}

		if(lockedIn)
		{
			var playerPosition = player.Position;
			LookAt(playerPosition);
			if(timespan > 1){
				player.GetDamage(10);
				timespan = 0;
			}
		}

		if(raycast3D.IsColliding() && !lockedIn)
			if(raycast3D.GetCollider() is Player p){
				lockedIn = true;
				player = p;
			}
			*/
	}

	private void MoveToRandomPostion()
	{
		Vector3 vec3 = GlobalPosition - newPosition;
		if(Math.Abs(vec3.X) < 0.1f && Math.Abs(vec3.X) < 0.1f)
		{
			Random r = new Random();
			newPosition = new Vector3(r.Next(-7,7), 
			GlobalPosition.Y, 
			r.Next(-7,7));
			LookAt(newPosition);
		}
		else
		{
				if(vec3.X < -0.1f)
					GlobalPosition += new Vector3(0.05f,0,0);
				else if(vec3.X > -0.1f)
					GlobalPosition -= new Vector3(0.05f,0,0);
				if(vec3.Z < -0.1f)
					GlobalPosition += new Vector3(0,0,0.05f);
				else if(vec3.Z > 0.1f)
					GlobalPosition -= new Vector3(0,0,0.05f);
		}
	}

	private double DamageMultiplier(HitArea hitArea)
	{
		switch (hitArea)
		{
			case HitArea.Head: return 1.5;
			case HitArea.Body: return 1;
			case HitArea.Legs: return 0.5;
		}
		return 0;
	}

	public void Hit(int damage, HitArea hitArea)
	{
		var dealtdamage = damage * DamageMultiplier(hitArea);
		GD.Print(dealtdamage);
		health -= (int)dealtdamage;
		if(health <= 0)
			QueueFree();
	}
}

public enum HitArea
{
	Head,
	Body,
	Legs
}

public enum EnemyState
{
	Search,
	Idle
}

