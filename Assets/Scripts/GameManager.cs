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

    void Start()
    {
        ui = GetComponent<UIManager>();
        ball.SwitchSide(bowlingFromLeft);
    }

    public void ChangeBowlingSide()
    {
        bowlingFromLeft = !bowlingFromLeft;
        ball.SwitchSide(bowlingFromLeft);
    }

    public void Bowl()
    {
        if (canBowl)
        {
            canBowl = false;
            ui.SwitchUIState(canBowl);
            target.SwitchState(canBowl);
            ball.Throw(type, target.transform.position);
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
