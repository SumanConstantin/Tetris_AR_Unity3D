using UnityEngine;
using System.Collections;

public class InputController {	
	public void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RotateElement();
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveElement(1);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveElement(-1);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DropElement();
        }
    }

    void RotateElement()
    {
        GameEvent.RotateElement(1);
    }

    void MoveElement(int x)
    {
        GameEvent.HorizontalMove(x);
    }

    void DropElement()
    {
        GameEvent.DropElement();
    }
}
