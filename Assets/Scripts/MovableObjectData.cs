using UnityEngine;

[System.Serializable]
public class MovableObjectData
{
    [SerializeField] private SurfaceType objectType;
    [SerializeField] private float movingSpeed;
    [SerializeField] private float rotatingSpeed;

    [Header("Physics")]
    [SerializeField] private Rigidbody rigidbodySelf;
    [SerializeField] private Collider selfCollider;

    public void SetMovable(MovableObject movableObject)
    {
        MovableObject = movableObject;
    }

    public MovableObject MovableObject { get; private set; }

    public float MovingSpeed => movingSpeed;
    public float RotatingSpeed => rotatingSpeed;
    public Rigidbody Rigidbody => rigidbodySelf;
    public SurfaceType ObjectSurfaceType => objectType;
    public Collider SelfCollider => selfCollider;
    public GameObject GameObject => MovableObject.gameObject;
}
