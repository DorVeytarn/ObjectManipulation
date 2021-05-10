using System;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [SerializeField] MovableObjectData objectData;

    public event Action MovementTypeChanged;
    public event Action TargetSurfaceCollisioned;

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
        if(collision.gameObject.TryGetComponent(out Surface surface) && surface.Type == objectData.ObjectSurfaceType)
            TargetSurfaceCollisioned?.Invoke();
    }

    public void SetMovementType(MovementType type)
    {
        if(type == null)
        {
            Debug.LogError($"MovementType on {name} is null!");
            return;
        }

        if(type != MovementType)
            type.Init(objectData);

        MovementType = type;

        MovementTypeChanged?.Invoke();
    }
}
