using System.Collections;
using UnityEngine;

public class BallMotion : MonoBehaviour
{
    public GameManager manaager;
    public Transform[] throwPositions;
    public float moveSpeed = 5.0f;
    public float minimumDistance = 0.05f;
    public float maxTimeAfterBounce = 5.0f;
    void Start()
    {
        Reset();
    }

    public void Reset()
    {

    }

    public void SwitchSide(bool bowlFromLeft)
    {
        Vector3 throwPos = throwPositions[bowlFromLeft ? 0 : 1].position;
        transform.position = throwPos;
    }

    public void Throw(BallThrowType type, Vector3 targetPos)
    {
        Debug.Log("Bowl");
        StartCoroutine(MoveToBounceTarget(targetPos));
    }

    private IEnumerator MoveToBounceTarget(Vector3 target)
    {
        Vector3 direction = Vector3.forward;
        while (transform.position.z < target.z)
        {
            direction = (target - transform.position).normalized;
            transform.forward = direction;
            transform.position += direction * moveSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        direction.y = -direction.y;
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
