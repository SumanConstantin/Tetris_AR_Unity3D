using System;
using UnityEngine;

public class MoveAction : AbstractAction
{
	private Vector2 addVector;
	public Vector2 AddVector
	{
		get{return addVector;}
		set{addVector = value;}
	}
		
	public MoveAction (Vector2 addVector)
	{
		this.addVector = addVector;
        Type = ActionType.MOVE;
    }
}

