using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using System;

public class UserInput : MonoBehaviour
{
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

    [Header("Sticking Movement")]
    [SerializeField] private KeyCode stickingMoving;

    public event Action<MovementType> MovementKeyActivated; 
    public event Action SelectorButtonClicked; 

    private void Update()
    {
        if (Input.GetMouseButtonDown(mouseButtonToSelect))
        {
            SelectorButtonClicked?.Invoke();
        }

        if (Input.GetKeyDown(freeMoving))
        {
            MovementKeyActivated?.Invoke(MovementType.Free);
        }

        if (Input.GetKeyDown(stickingMoving))
        {
            MovementKeyActivated?.Invoke(MovementType.Sticking);
        }
    }
}
