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


    public virtual void Init(MovableObjectData objectData)
    {
        mainCamera = Camera.main;

        targetObjectData = objectData;

        targetObject = targetObjectData.GameObject;
        targetRigidbody = targetObjectData.Rigidbody;
        speed = targetObjectData.MovingSpeed;
        movable = targetObjectData.MovableObject;

        targetRigidbody.isKinematic = false;
    }

    public abstract void Move();
    public abstract void EndMove();
}

public class FreeMovement : MovementType
{
    private const int dropVelocity = 10;

    private Vector3 originalTargetPosition;
    private float selectionDistance;
    private Vector3 originalScreenTargetPosition;
    private Vector3 originalEulerAngles;

    public override void Init(MovableObjectData objectData)
    {
        base.Init(objectData);

        UserInput.Instance.ResetRotationKeyClicked += ResetRotation;

        targetRigidbody.useGravity = false;

        originalTargetPosition = targetObject.transform.position;

        selectionDistance = Vector3.Distance(targetObject.transform.position, mainCamera.transform.position);
        originalScreenTargetPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                                                                 Input.mousePosition.y,
                                                                                 selectionDistance));
    }

    public override void Move()
    {
        if (UserInput.IsFreeRotation)
            Rotate();
        else
        {
            Vector3 mousePositionOffset = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, selectionDistance)) - originalScreenTargetPosition;
            targetRigidbody.velocity = (originalTargetPosition + mousePositionOffset - targetRigidbody.transform.position) * speed * Time.deltaTime;
        }
    }

    public override void EndMove()
    {
        targetRigidbody.velocity = Vector3.down * dropVelocity;
        targetRigidbody.useGravity = true;

        UserInput.Instance.ResetRotationKeyClicked -= ResetRotation;

        targetRigidbody.isKinematic = false;
    }

    public void Rotate()
    {
        targetRigidbody.velocity = Vector3.zero;

        //вращаем каждую ось отдельно + относительно камеры, другие способы визуально неясны
        targetObject.transform.RotateAround(targetObject.transform.position,
                                            mainCamera.transform.up, 
                                            Input.GetAxis(UserInput.HORIZONTAL_AXIS) * targetObjectData.RotatingSpeed);

        targetObject.transform.RotateAround(targetObject.transform.position,
                                            mainCamera.transform.right,
                                            Input.GetAxis(UserInput.VERTICAL_AXIS) * targetObjectData.RotatingSpeed);

    }

    private void ResetRotation()
    {
        targetObject.transform.rotation = Quaternion.Euler(originalEulerAngles);
    }
}

public class StickingMovement : MovementType
{
    private Collider stickingSurface;
    private int ignoredLayer;

    public override void Init(MovableObjectData objectData)
    {
        base.Init(objectData);
        targetRigidbody.useGravity = false;

        ignoredLayer = 1 << targetObject.layer;
        ignoredLayer = ~ignoredLayer;
    }

    public override void Move()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10000, ignoredLayer))
        {
            if (hit.collider != stickingSurface)
                if (hit.collider.TryGetComponent(out Surface surface) && surface.Type == targetObjectData.ObjectSurfaceType)
                    stickingSurface = hit.collider;
                else
                {
                    return;
                }

            targetObject.transform.forward = hit.normal;

            targetRigidbody.velocity = (hit.point - targetRigidbody.transform.position) * speed * Time.fixedDeltaTime;
        }
    }

    public override void EndMove()
    {
        stickingSurface = null;
        targetRigidbody.velocity = Vector3.zero;

        targetRigidbody.isKinematic = false;
    }
}