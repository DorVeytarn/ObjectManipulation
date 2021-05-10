using UnityEngine;

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
        base.Move();

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10000, ignoredLayer))
        {
            if (hit.collider != stickingSurface)
                if (hit.collider.TryGetComponent(out Surface surface) && surface.Type == targetObjectData.ObjectSurfaceType)
                    stickingSurface = hit.collider;
                else
                    return;

            targetObject.transform.forward = hit.normal;

            targetRigidbody.velocity = (hit.point - targetRigidbody.transform.position) * speed * Time.fixedDeltaTime;
        }
    }

    public override void EndMove()
    {
        base.EndMove();

        stickingSurface = null;
        targetRigidbody.velocity = Vector3.zero;

        targetRigidbody.isKinematic = false;
    }
}