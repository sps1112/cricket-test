using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager manager;

    public GameObject UIWindow;

    public Button swingButton;
    public Button spinButton;

    public Button leftButton;
    public Button rightButton;

    void Start()
    {
        manager = GetComponent<GameManager>();
    }

    public void SwitchUIState(bool status)
    {
        EventSystem.current.SetSelectedGameObject(null);
        UIWindow.SetActive(status);
    }

    public void ClickBowlSideChange()
    {
        manager.ChangeBowlingSide();
    }

    public void UpdateTypeUI(bool isSwing)
    {
        swingButton.interactable = !isSwing;
        spinButton.interactable = isSwing;
    }

    public void SetThrowType(bool isSwing)
    {
        manager.SetThrowType(isSwing);
        UpdateTypeUI(isSwing);
    }

    public void UpdateDirectionUI(bool isLeft)
    {
        leftButton.interactable = !isLeft;
        rightButton.interactable = isLeft;
    }

    public void SetThrowDirection(bool isLeft)
    {
        manager.SetThrowDirection(isLeft);
        UpdateDirectionUI(isLeft);
    }

    public void ClickBowl()
    {
        manager.Bowl();
    }
}
