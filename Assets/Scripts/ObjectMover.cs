using Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    private ObjectSelector objectSelector;
    private UserInput userInput;
    private MovableObject currentObject;
    private FreeMovement freeMovement;
    private StickingMovement stickingMovement;


    private void Start()
    {
        objectSelector = S.DI<ObjectSelector>();
        userInput = S.DI<UserInput>();

        objectSelector.ObjectSelected += OnObjectSelected;
        userInput.MovementKeyActivated += OnMovementKeyActivated;

        freeMovement = new FreeMovement();
        stickingMovement = new StickingMovement();
    }

    private void OnDestroy()
    {
        objectSelector.ObjectSelected -= OnObjectSelected;
        userInput.MovementKeyActivated -= OnMovementKeyActivated;
    }

    private void OnMovementKeyActivated(UserInput.MovementType movementType)
    {
        MovementType newType = null;

        switch (movementType)
        {
            case UserInput.MovementType.Free:
                newType = freeMovement;
                break;
            case UserInput.MovementType.Sticking:
                newType = stickingMovement;
                break;
        }

        currentObject.SetMovementType(newType);
    }

    private void OnObjectSelected(MovableObject movableObject)
    {
        currentObject = movableObject;
    }
}
