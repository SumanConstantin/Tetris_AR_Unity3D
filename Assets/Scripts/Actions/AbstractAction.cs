using System;

public class AbstractAction
{
	private String type;
	public String Type
	{
		get
		{
			return type;
		}
		set
		{
			type = value;
		}
	}

	private ActionType finished;
	public ActionType Finished
	{
		get
		{
			return finished;
		}
		set
		{
			finished = value;
		}
	}

	public void Destroy()
	{
			
	}
}

