using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private GameObject target;

    [Header("Rotate Settings")]
    [SerializeField] private float sensitivityX = 1f;
    [SerializeField] private float sensitivityY = 1f;
    [SerializeField] private Vector2 yRotationLimite = new Vector2(-40, 40);
    [SerializeField] private Vector2 xRotationLimite = new Vector2(-360, 360);

    private float xRotate;
    private float yRotate;

    private void Update()
    {
        if (UserInput.IsFreeRotation == false && Input.GetMouseButton(1))
        {
            yRotate -= Input.GetAxis(UserInput.MOUSE_VERTICAL_AXIS) * sensitivityY;
            yRotate = ClampAngle(yRotate, yRotationLimite.x, yRotationLimite.y);

            xRotate += Input.GetAxis(UserInput.MOUSE_HORIZONTAL_AXIS) * sensitivityX;
            xRotate = ClampAngle(xRotate, xRotationLimite.x, xRotationLimite.y);

            target.transform.rotation = Quaternion.Euler(0, xRotate, yRotate);
        }
    }

    private float ClampAngle(float value, float min, float max)
    {
        if (value < -360)
            value += 360;

        if (value > 360)
            value -= 360;

        return Mathf.Clamp(value, min, max);
    }
}
