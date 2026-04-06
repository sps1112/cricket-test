using UnityEngine;

public class BounceTarget : MonoBehaviour
{
    private bool canMoveTarget = true;

    [Tooltip("Bounds in Xmin,Xmax,Ymin,Ymax format")]
    [SerializeField] private Vector4 bounceHitRange = Vector2.one;

    [SerializeField] private float moveSpeed = 2.0f;

    private Vector3 startOrigin;

    private Vector2 currentDisplacement;

    [SerializeField] private bool resetAfterEachBowl = false;

    void Start()
    {
        startOrigin = transform.position;
        Reset();
    }

    // Resets the bounce target to origin
    private void Reset()
    {
        currentDisplacement = Vector2.zero;
        transform.position = startOrigin;
        canMoveTarget = true;
    }

    // Called by gamemanager to stop the target movement when we have thrown the ball
    public void SwitchState(bool status)
    {
        canMoveTarget = status;
        if (resetAfterEachBowl && canMoveTarget)
        {
            Reset();
        }
    }

    // Clamps the position in the XZ Plane inside the limits
    private void ClampDisplacement()
    {
        currentDisplacement.x = (currentDisplacement.x < bounceHitRange.x) ? bounceHitRange.x : currentDisplacement.x;
        currentDisplacement.x = (currentDisplacement.x > bounceHitRange.y) ? bounceHitRange.y : currentDisplacement.x;
        currentDisplacement.y = (currentDisplacement.y < bounceHitRange.z) ? bounceHitRange.z : currentDisplacement.y;
        currentDisplacement.y = (currentDisplacement.y > bounceHitRange.w) ? bounceHitRange.w : currentDisplacement.y;
    }

    void Update()
    {
        if (canMoveTarget)
        {
            // Along the 2D Plane
            Vector2 displacement = Vector2.zero;
            if (Input.GetKey(KeyCode.W))
            {
                displacement.y += 1.0f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                displacement.y -= 1.0f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                displacement.x -= 1.0f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                displacement.x += 1.0f;
            }

            if (displacement.sqrMagnitude > 0.0f)
            {
                displacement.Normalize();
                currentDisplacement += displacement * moveSpeed * Time.deltaTime;
                ClampDisplacement();
                transform.position = startOrigin + (Vector3.right * currentDisplacement.x + Vector3.forward * currentDisplacement.y);
            }
        }
    }
}
