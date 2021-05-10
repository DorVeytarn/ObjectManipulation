using Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementType
{
    protected float speed;
    protected Camera mainCamera;
    protected GameObject targetObject;
    protected MovableObjectData targetObjectData;
    protected Rigidbody targetRigidbody;
    protected MovableObject movable;
    protected Vector3 originalEulerAngles;

    public virtual void Init(MovableObjectData objectData)
    {
        mainCamera = Camera.main;

        targetObjectData = objectData;

        targetObject = targetObjectData.GameObject;
        targetRigidbody = targetObjectData.Rigidbody;
        speed = targetObjectData.MovingSpeed;
        movable = targetObjectData.MovableObject;

        targetRigidbody.isKinematic = true;
        originalEulerAngles = targetObject.transform.eulerAngles;
    }

    public virtual void Move()
    {
        targetRigidbody.isKinematic = false;
    }

    public virtual void EndMove()
    {
        targetRigidbody.isKinematic = true;
    }
}
