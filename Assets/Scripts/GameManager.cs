using NUnit.Framework;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private UIManager ui;

    private bool canBowl = true;

    [SerializeField] private BallMotion ball;

    [SerializeField] private BounceTarget target;

    [SerializeField] private bool bowlingFromLeft = true;

    [SerializeField] private bool isTypeSwing = true;

    [Tooltip("Which direction to apply the swing or spin")]
    [SerializeField] private bool directionIsLeft = true;

    void Start()
    {
        ui = GetComponent<UIManager>();
        ui.UpdateTypeUI(isTypeSwing);
        ui.UpdateDirectionUI(directionIsLeft);
        ui.SwitchUIState(true);
        Invoke("RefreshObjects", 0.1f);
    }

    // Resets ball to near the wicket. Some issues in Start hence invoked some time later
    private void RefreshObjects()
    {
        ball.SwitchSide(bowlingFromLeft);
    }

    public void ChangeBowlingSide()
    {
        bowlingFromLeft = !bowlingFromLeft;
        ball.SwitchSide(bowlingFromLeft);
    }

    public void SetThrowType(bool isSwing)
    {
        isTypeSwing = isSwing;
    }

    public void SetThrowDirection(bool isLeft)
    {
        directionIsLeft = isLeft;
    }

    public void Bowl()
    {
        if (canBowl)
        {
            canBowl = false;
            ui.SwitchUIState(canBowl);
            target.SwitchState(canBowl);
            ball.Throw(isTypeSwing, directionIsLeft, ui.GetPowerScale(), target.transform.position);
        }
    }

    public void ResetBall()
    {
        canBowl = true;
        ui.SwitchUIState(canBowl);
        target.SwitchState(canBowl);
        ball.SwitchSide(bowlingFromLeft);
    }
}
