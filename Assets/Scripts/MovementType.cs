using UnityEngine;

public abstract class MovementType
{
    public abstract void Move();
}

public class FreeMovement : MovementType
{
    public override void Move()
    {
        Debug.Log("It's " + nameof(FreeMovement));
    }
}

public class StickingMovement : MovementType
{
    public override void Move()
    {
        Debug.Log("It's " + nameof(StickingMovement));
    }
}