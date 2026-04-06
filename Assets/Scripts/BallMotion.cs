using System.Collections;
using UnityEngine;

public class BallMotion : MonoBehaviour
{
    [SerializeField] private GameManager manaager;

    [SerializeField] private Transform[] throwPositions;

    [SerializeField] private float moveSpeed = 5.0f;

    [Tooltip("Time to reset ball after bounce")]
    [SerializeField] private float maxTimeAfterBounce = 5.0f;

    [SerializeField] private float maxSwingForce = 10.0f;

    [Tooltip("Maximum rotation caused in spin")]
    [SerializeField] private float maxSpinForce = 1;

    private TrailRenderer trail;

    void Start()
    {
        trail = GetComponent<TrailRenderer>();
    }

    // Switches side of ball from left or right side of wickets
    public void SwitchSide(bool bowlFromLeft)
    {
        trail.emitting = false;
        Vector3 throwPos = throwPositions[bowlFromLeft ? 0 : 1].position;
        transform.position = throwPos;
        trail.emitting = true;
    }

    // Starts the Ball throw
    public void Throw(bool isSwing, bool directionIsLeft, float power, Vector3 targetPos)
    {
        StartCoroutine(MoveToBounceTarget(isSwing, directionIsLeft, power, targetPos));
    }

    private IEnumerator MoveToBounceTarget(bool isSwing, bool directionIsLeft, float power, Vector3 target)
    {
        Vector3 direction = Vector3.forward;
        float maxDistance = (target - transform.position).magnitude;
        float typeDir = directionIsLeft ? -1 : 1;

        // Travelling in air
        while (transform.position.z < target.z)
        {
            if (isSwing) // Swing
            {
                Vector3 displacement = target - transform.position;
                float distance = displacement.magnitude;
                displacement = displacement.normalized * moveSpeed * Time.deltaTime;
                displacement.x += typeDir * (maxSwingForce * power) * (distance / maxDistance) * Time.deltaTime;
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
        if (isSwing)  // Swing
        {
            direction.y = -direction.y;
        }
        else // Spin
        {
            transform.Rotate(Vector3.up, typeDir * maxSpinForce * power, Space.World);
            Vector3 newForward = transform.forward;
            newForward.y = -newForward.y;
            direction = newForward;
        }
        transform.forward = direction;

        // After Bounce
        StartCoroutine(MovementAfterBounce(direction));
    }

    // Moves for a little while and then resets
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
