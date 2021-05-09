using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using System;

public class UserInput : Singleton<UserInput>
{
    public const string MOUSE_VERTICAL_AXIS = "Mouse Y";
    public const string MOUSE_HORIZONTAL_AXIS = "Mouse X";
    public const string HORIZONTAL_AXIS = "Horizontal";
    public const string VERTICAL_AXIS = "Vertical";

    public enum MovementType
    {
        None,
        Free,
        Sticking
    }

    [Header("Selecting")]
    [SerializeField] private int mouseButtonToSelect;

    [Header("Free Movement")]
    [SerializeField] private KeyCode freeMoving;
    [SerializeField] private KeyCode resetRotation;
    [SerializeField] private int mouseButtonToFreeRotate;

    [Header("Sticking Movement")]
    [SerializeField] private KeyCode stickingMoving;

    private MovementType currentType;

    public static bool IsFreeRotation
    {
        get
        {
            return Instance.currentType == MovementType.Free 
                                  && Input.GetMouseButton(Instance.mouseButtonToFreeRotate) 
                                  && Input.GetMouseButton(Instance.mouseButtonToSelect);
        }
    }

    public event Action<MovementType> MovementKeyActivated; 
    public event Action SelectorButtonClicked; 
    public event Action ResetRotationKeyClicked; 

    private void Update()
    {
        if (Input.GetMouseButtonDown(mouseButtonToSelect))
        {
            SelectorButtonClicked?.Invoke();
        }

        if (Input.GetKeyDown(freeMoving))
        {
            currentType = MovementType.Free;
            MovementKeyActivated?.Invoke(currentType);
        }

        if (Input.GetKeyDown(resetRotation))
        {
            ResetRotationKeyClicked?.Invoke();
        }

        if (Input.GetKeyDown(stickingMoving))
        {
            currentType = MovementType.Sticking;
            MovementKeyActivated?.Invoke(currentType);
        }
    }
}
