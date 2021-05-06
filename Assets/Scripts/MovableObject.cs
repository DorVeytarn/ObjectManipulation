using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    private MovementType movementType;

    public Action MovementTypeChanged; 

    public void SetMovementType(MovementType type)
    {
        movementType = type;

        MovementTypeChanged?.Invoke();
    }
}
