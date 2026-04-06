using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameManager manager;

    public GameObject UIWindow;
    void Start()
    {
        manager = GetComponent<GameManager>();
    }

    public void SwitchUIState(bool status)
    {
        UIWindow.SetActive(status);
    }

    public void ClickBowlSideChange()
    {
        manager.ChangeBowlingSide();
    }

    public void ClickBowl()
    {
        manager.Bowl();
    }
}
