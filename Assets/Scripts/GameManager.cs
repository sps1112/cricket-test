using NUnit.Framework;
using UnityEngine;
public enum BallThrowType
{
    Swing,
    Spin,
}

public class GameManager : MonoBehaviour
{
    private UIManager ui;
    public bool canBowl = true;
    public BallMotion ball;
    public BounceTarget target;
    [SerializeField] private bool bowlingFromLeft = true;
    [SerializeField] private BallThrowType type = BallThrowType.Swing;
    [SerializeField] private bool directionIsLeft = true;

    void Start()
    {
        ui = GetComponent<UIManager>();
        ui.UpdateTypeUI(type == BallThrowType.Swing);
        ui.UpdateDirectionUI(directionIsLeft);
        ball.SwitchSide(bowlingFromLeft);
    }

    public void ChangeBowlingSide()
    {
        bowlingFromLeft = !bowlingFromLeft;
        ball.SwitchSide(bowlingFromLeft);
    }

    public void SetThrowType(bool isSwing)
    {
        type = (isSwing == true) ? BallThrowType.Swing : BallThrowType.Spin;
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
            ball.Throw(type, directionIsLeft, target.transform.position);
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
