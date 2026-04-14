using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Room : Node3D
{

	private List<Enemy> enemyList = new List<Enemy>();
	private List<Node> spotsList = new List<Node>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		enemyList.Add(GetNode<Enemy>("Enemy"));
		var points = GetNode<Node3D>("StaticPoints").GetChildren();
		foreach(var i in points)
		{
			spotsList.Add(i);
		}
		enemyList[0].spotsList = spotsList;
		enemyList[0].StartAI();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
