using Godot;
using System;
using System.Collections.Generic;

public partial class Enemy : CharacterBody3D
{

	double delay = 0.1;
	double timespan = 0.0;
	double timespan2 = 0.0;
	RayCast3D raycast3D;
	bool lockedIn = false;
	Player player;
	private int health = 100;

	//AI stuff
	EnemyState enemyState = EnemyState.Walk;
	Vector3 newPosition;
	private NavigationAgent3D navAgent;
	public List<Node> spotsList;
	private Area3D visionArea;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		raycast3D = GetNode<RayCast3D>("RayCast3D");
		newPosition = GlobalPosition;
		navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		visionArea = GetNode<Area3D>("VisionArea");
	}

	public void StartAI()
	{
		navAgent.TargetPosition = (spotsList[0] as Node3D).Position;
	}

    public override void _PhysicsProcess(double delta)
    {
        switch (enemyState){
			case EnemyState.Walk:
				{
					Move();
					target_reached();
					break;
				}
			case EnemyState.Search:
				{
					RotateY(0.01f);
					timespan += delta;
					if(timespan > 5){
						enemyState = EnemyState.Walk;
						timespan = 0;
					}
					break;
				}
			case EnemyState.Attack:
				{
					timespan2 += delta;
					if(timespan2 > 0.5){
						LookAt(player.Position);
						player.GetDamage(10);
						timespan2 = 0;
					}
					break;
				}
		}

		GD.Print(enemyState);		
		if(visionArea.HasOverlappingBodies())
		{
			foreach(var i in visionArea.GetOverlappingBodies())
			{
				if(i.Name == "Player"){
					player = i as Player;
					raycast3D.LookAt(player.Position);
					raycast3D.Rotation = new Vector3(0, raycast3D.Rotation.Y + 1.571f, 1.571f);
					if (raycast3D.IsColliding())
					{
						GD.Print((raycast3D.GetCollider() as Node).Name);
						if(raycast3D.GetCollider() is Player)
						{
							enemyState = EnemyState.Attack;
						}	
					}
				}
				else if(enemyState == EnemyState.Attack)
					enemyState = EnemyState.Search;
			}
		}
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		

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
	
	public void target_reached()
	{
		if(navAgent.IsTargetReached()){
			Random random = new Random();
			
			navAgent.TargetPosition = (spotsList[random.Next(0,2)] as Node3D).Position;

			enemyState = EnemyState.Search;
		}
	}

	private void Move()
	{
		
		var currentPostion = GlobalPosition;
		var nextLocation = navAgent.GetNextPathPosition();
		var newVelocity = (nextLocation - currentPostion).Normalized() * new Vector3(3,3,3);

		Velocity = Velocity.MoveToward(newVelocity, 0.25f);
		if(nextLocation != Position)
			LookAt(nextLocation);
		Rotation = new Vector3(0, Rotation.Y, Rotation.Z);
		MoveAndSlide();
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
	Walk,
	Search,
	Attack
}

