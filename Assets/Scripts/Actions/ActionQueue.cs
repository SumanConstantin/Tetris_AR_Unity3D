using UnityEngine;
using System;
using System.Collections.Generic;

// This is a FIFO (FirstIn-FirstOut) Actions Queue
	
public class ActionQueue
{
	private List<AbstractAction> actions = new List<AbstractAction>();
	public void Add(AbstractAction action)
	{
		actions.Add(action);
	}

	public void Remove(AbstractAction action)
	{
		actions.Remove(action);
	}

    public void Clear()
    {
        actions.Clear();
    }

	public AbstractAction PopFirst()
	{
        AbstractAction result = actions[0];
		Remove(result);
		return result;
	}

	public int Count
	{
		get{return actions.Count;}
	}

	public void Destroy()
	{
		foreach (AbstractAction action in actions)
		{
			action.Destroy();
		}

		actions = null;
	}
}

