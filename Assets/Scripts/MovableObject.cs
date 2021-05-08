﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;

[System.Serializable]
public class MovableObjectData
{
    [SerializeField] private SurfaceType objectType;
    [SerializeField] private float movingSpeed;

    [Header("Physics")]
    [SerializeField] private Rigidbody rigidbodySelf;
    [SerializeField] private Collider selfCollider;

    public void SetMovable(MovableObject movableObject)
    {
        MovableObject = movableObject;
    }

    public MovableObject MovableObject { get; private set; }

    public float MovingSpeed => movingSpeed;
    public Rigidbody Rigidbody => rigidbodySelf;
    public SurfaceType ObjectSurfaceType => objectType;
    public Collider SelfCollider => selfCollider;
    public GameObject GameObject => MovableObject.gameObject;
}

public class MovableObject : MonoBehaviour
{
    [SerializeField] MovableObjectData objectData;

    public event Action MovementTypeChanged;
    public event Action Collisioned;

    public MovementType MovementType { get; private set; }

    private void Start()
    {
        objectData.SetMovable(this);
    }

    private void OnMouseDown()
    {
        MovementType?.Init(objectData);
    }

    private void OnMouseDrag()
    {
        MovementType?.Move();
    }

    private void OnMouseUp()
    {
        MovementType?.EndMove();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collisioned?.Invoke();
    }

    public void SetMovementType(MovementType type)
    {
        if(type == null)
        {
            Debug.LogError($"MovementType on {name} is null!");
            return;
        }

        MovementType = type;

        MovementTypeChanged?.Invoke();
    }
}
