using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    [SerializeField] private float speed;

    private float flyingAxis;

    private void Update()
    {

        if (Input.GetKey(KeyCode.Q))
            flyingAxis = 1;

        if (Input.GetKey(KeyCode.E))
            flyingAxis = -1;

        if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E))
            flyingAxis = 0;

        if (UserInput.IsFreeRotation == false)
        {
            float moveHorizontal = Input.GetAxis(UserInput.HORIZONTAL_AXIS);
            float moveVertical = Input.GetAxis(UserInput.VERTICAL_AXIS);

            Vector3 movement = new Vector3(-moveVertical, flyingAxis, moveHorizontal);

            transform.Translate(movement * speed * Time.fixedDeltaTime);
        }

    }
}
