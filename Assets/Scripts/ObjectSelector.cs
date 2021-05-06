using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    [SerializeField] private int mouseButtonToSelect;

    private Camera mainCamera;
    private MovableObject selectedObject;

    public Action<MovableObject> ObjectSelected; 

    public MovableObject SelectedObject
    {
        get
        {
            return selectedObject;
        }
        set
        {
            selectedObject = value;
            ObjectSelected?.Invoke(selectedObject);
            Debug.Log($"Object selected {selectedObject.name}");
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(mouseButtonToSelect))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Physics.Raycast(ray, out hit);

            if(hit.collider != null && hit.collider.TryGetComponent(out MovableObject movableObject))
            {
                SelectedObject = movableObject;
            }
        }
    }
}
