using System;
using UnityEngine;

public class DropAction : AbstractAction
{
	private Vector3 addVector;
	public Vector3 AddVector
	{
		get{return addVector;}
		set{addVector = value;}
	}
		
	public DropAction(Vector3 addVector)
	{
		this.addVector = addVector;
        Type = ActionType.DROP;
    }
}

