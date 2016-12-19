using System;
using UnityEngine;

public class RotateAction : AbstractAction
{
    private int rotateValue;
    public int RotateValue
    {
        get { return rotateValue; }
        set { rotateValue = value; }
    }

    public RotateAction(int rotateValue)
    {
        this.rotateValue = rotateValue;
        Type = ActionType.ROTATE;
    }
}