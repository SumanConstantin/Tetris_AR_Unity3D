using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VirtualControllerBehavior : MonoBehaviour {
    private Vector3 initRotation;
    private const float elementRotationAngleThreshold = 45;
    private const float elementMoveAngleThreshold = 6;
    private const float elementDropAngleMinThreshold = 10;
    private const float elementDropAngleMaxThreshold = 45;
    private bool isDropping = false;
    private bool controllerIsReturnedFromDrop = true;

    void Start()
    {
        Init();
    }

	public void Init()
    {
        InitControllerParams();
        InitEventListeners();
    }

    private void InitControllerParams()
    {
        InitElementRotationParams();
    }

    private void InitElementRotationParams()
    {
        Vector3 rot = transform.eulerAngles;
        initRotation = new Vector3(rot.x, rot.y, rot.z);

        isDropping = false;
    }

    private void InitEventListeners()
    {
        GameEvent.onElementTouchedField += OnElementTouchedField;
    }

    private void RemoveEventListeners()
    {
        GameEvent.onElementTouchedField -= OnElementTouchedField;
    }

    private void OnElementTouchedField(AbstractElementModel element, bool lose = false)
    {
        if (!lose)
        {
            InitElementRotationParams();
        }
    }

    // Update is called once per frame
    void Update () {
        CheckInput();
	}

    private void CheckInput()
    {
        //CheckRotationX();   // Drop

        if (!isDropping)
        {
            CheckRotationY();   // Horizontal Move
            CheckRotationZ();   // Element Rotation
        }
    }

    private void CheckRotationX()
    {
        float rotDelta = transform.eulerAngles.x - initRotation.x;
        int elementMoveValue = 0; // 1 => Right; -1 => Left

        /*if (!controllerIsReturnedFromDrop)
        {
            if (Mathf.Abs(rotDelta) - elementDropAngleMaxThreshold < 0)
            {
                controllerIsReturnedFromDrop = true;
            }
            else
            {
                return;
            }
        }*/

        if (rotDelta > elementDropAngleMaxThreshold)
        {
            elementMoveValue = 1;
        }

        if (elementMoveValue == 1)
        {
            GameEvent.DropElement();
            //initRotation.x += rotDelta;
            isDropping = true;
            controllerIsReturnedFromDrop = false;
        }
    }

    private void CheckRotationY()
    {
        float rotDelta = transform.eulerAngles.y - initRotation.y;
        int elementMoveValue = 0; // 1 => Right; -1 => Left

        if (rotDelta > elementMoveAngleThreshold)
        {
            elementMoveValue = 1;
        }
        else
        if (rotDelta < -elementMoveAngleThreshold)
        {
            elementMoveValue = -1;
        }

        if (elementMoveValue != 0)
        {
            GameEvent.HorizontalMove(elementMoveValue);
            initRotation.y += rotDelta;
        }
    }

    private void CheckRotationZ()
    {
        float rotDelta = transform.eulerAngles.z - initRotation.z;
        int elementRotationValue = 0; // 1 => Clockwise; -1 => Counterclockwise

        if (rotDelta > elementRotationAngleThreshold)
        {
            elementRotationValue = -1;
        }
        else
        if (rotDelta < -elementRotationAngleThreshold)
        {
            elementRotationValue = 1;
        }

        if (elementRotationValue != 0)
        {
            GameEvent.RotateElement(elementRotationValue);
            initRotation.z += rotDelta;
        }
    }
}
