using UnityEngine;

public class FreeMovement : MovementType
{
    private const int dropVelocity = 15;

    private Vector3 originalTargetPosition;
    private float selectionDistance;
    private Vector3 originalScreenTargetPosition;

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
        base.Move();

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
