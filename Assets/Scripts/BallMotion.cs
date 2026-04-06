using System.Collections;
using UnityEngine;

public class BallMotion : MonoBehaviour
{
    public GameManager manaager;
    public Transform[] throwPositions;
    public float moveSpeed = 5.0f;
    public float minimumDistance = 0.05f;
    public float maxTimeAfterBounce = 5.0f;
    public float maxSpinForce = 1;

    public void SwitchSide(bool bowlFromLeft)
    {
        Vector3 throwPos = throwPositions[bowlFromLeft ? 0 : 1].position;
        transform.position = throwPos;
    }

    public void Throw(BallThrowType type, bool directionIsLeft, Vector3 targetPos)
    {
        StartCoroutine(MoveToBounceTarget(type, directionIsLeft, targetPos));
    }

    private IEnumerator MoveToBounceTarget(BallThrowType type, bool directionIsLeft, Vector3 target)
    {
        Vector3 direction = Vector3.forward;

        // Travelling in air
        while (transform.position.z < target.z)
        {
            if (type == BallThrowType.Swing)
            {
                direction = (target - transform.position).normalized;
                transform.forward = direction;
                transform.position += direction * moveSpeed * Time.deltaTime;
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
            float rotateDir = directionIsLeft ? -1 : 1;
            transform.Rotate(Vector3.up, rotateDir * maxSpinForce, Space.World);
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
