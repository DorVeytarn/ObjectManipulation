using Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    private Camera mainCamera;
    private UserInput userInput;

    public event Action<MovableObject> ObjectSelected; 

    private void Start()
    {
        mainCamera = Camera.main;
        userInput = S.DI<UserInput>();

        userInput.SelectorButtonClicked += OnSelectorButtonClicked;
    }

    private void OnDestroy()
    {
        userInput.SelectorButtonClicked -= OnSelectorButtonClicked;
    }

    private void OnSelectorButtonClicked()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Physics.Raycast(ray, out hit);

        if (hit.collider != null && hit.collider.TryGetComponent(out MovableObject movableObject))
        {
            ObjectSelected?.Invoke(movableObject);
            Debug.Log($"Object selected {movableObject.name}");
        }
    }
}
