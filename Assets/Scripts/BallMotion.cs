using System.Collections;
using UnityEngine;

public class BallMotion : MonoBehaviour
{
    public GameManager manaager;
    public Transform[] throwPositions;
    public float moveSpeed = 5.0f;
    public float minimumDistance = 0.05f;
    public float maxTimeAfterBounce = 5.0f;

    public float maxSwingForce = 10.0f;
    public float maxSpinForce = 1;

    private TrailRenderer trail;

    void Start()
    {
        trail = GetComponent<TrailRenderer>();
    }

    public void SwitchSide(bool bowlFromLeft)
    {
        trail.emitting = false;
        Vector3 throwPos = throwPositions[bowlFromLeft ? 0 : 1].position;
        transform.position = throwPos;
        trail.emitting = true;
    }

    public void Throw(BallThrowType type, bool directionIsLeft, Vector3 targetPos)
    {
        StartCoroutine(MoveToBounceTarget(type, directionIsLeft, targetPos));
    }

    private IEnumerator MoveToBounceTarget(BallThrowType type, bool directionIsLeft, Vector3 target)
    {
        Vector3 direction = Vector3.forward;
        float horizontalSpeed = maxSwingForce;
        float maxDistance = (target - transform.position).magnitude;
        float typeDir = directionIsLeft ? -1 : 1;

        // Travelling in air
        while (transform.position.z < target.z)
        {
            if (type == BallThrowType.Swing)
            {
                Vector3 displacement = (target - transform.position);
                float distance = displacement.magnitude;
                displacement = displacement.normalized * moveSpeed * Time.deltaTime;
                displacement.x += typeDir * maxSwingForce * (distance / maxDistance) * Time.deltaTime;
                direction = displacement.normalized;

                transform.forward = direction;
                transform.position += displacement;
            }
            else // Spin
            {
                direction = (target - transform.position).normalized;
                transform.forward = direction;
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
            yield return new WaitForEndOfFrame();
        }

        //  Bounce
        if (type == BallThrowType.Swing)
        {
            direction.y = -direction.y;
        }
        else // Spin
        {
            transform.Rotate(Vector3.up, typeDir * maxSpinForce, Space.World);
            Vector3 newForward = transform.forward;
            newForward.y = -newForward.y;
            direction = newForward;
        }

        // After Bounce
        StartCoroutine(MovementAfterBounce(direction));
    }

    private IEnumerator MovementAfterBounce(Vector3 direction)
    {
        float timer = 0.0f;
        while (timer < maxTimeAfterBounce)
        {
            timer += Time.deltaTime;
            transform.position += direction * moveSpeed * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        manaager.ResetBall();
    }
}
