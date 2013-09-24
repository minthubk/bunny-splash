using UnityEngine;
using System.Collections;

public class FlyingPlatformScript : MonoBehaviour
{
    public Vector3 Direction;
    public float Amplitude = 1;
    public float Speed = 2f;

    private Vector3 mStartPosition;
    private Vector3 mTarget;
    private Vector3 mDistanceBetweenCenterAndCollider;

    void Awake()
    {
        acquireTarget();

        mDistanceBetweenCenterAndCollider = collider.bounds.center - transform.position;
    }

    private void acquireTarget()
    {
        mStartPosition = transform.position;
        mTarget = mStartPosition + (Direction * Amplitude);
    }

    void Update()
    {
        // Check if collider reached the target
        float distance = Vector3.Distance(collider.bounds.center, mTarget + mDistanceBetweenCenterAndCollider);

        if (distance > 0.15f)
        {
            // Move
            Vector3 movement = Direction * Speed * Time.deltaTime;
            transform.Translate(movement);
        }
        else
        {
            // Swap target
            Direction.x = -Direction.x;
            Direction.y = -Direction.y;

            acquireTarget();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(mStartPosition + mDistanceBetweenCenterAndCollider, mTarget + mDistanceBetweenCenterAndCollider);
    }
}
