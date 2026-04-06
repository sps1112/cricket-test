using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager manager;

    [Header("UI Windows")]

    [SerializeField] private GameObject UIWindow;

    [SerializeField] private Button swingButton;

    [SerializeField] private Button spinButton;

    [SerializeField] private Button leftButton;

    [SerializeField] private Button rightButton;

    [Header("Power Scale")]

    [Tooltip("The parent object which holds the level and divisions")]
    [SerializeField] private RectTransform powerScaleUI;

    [SerializeField] private RectTransform powerScaleLever;

    [SerializeField] private float powerScaleTimePeriod;

    private float powerScale = 0.0f;


    void Start()
    {
        manager = GetComponent<GameManager>();
    }

    // Hides or shows the UI and activates the power scale when shown
    public void SwitchUIState(bool status)
    {
        EventSystem.current.SetSelectedGameObject(null);
        UIWindow.SetActive(status);
        if (status)
        {
            StartCoroutine(MovePowerScale());
        }
        else
        {
            StopAllCoroutines();
        }
    }

    // Oscillates the power scale
    private IEnumerator MovePowerScale()
    {
        float amplitude = (powerScaleUI.rect.height - powerScaleLever.rect.height) / 2.0f;
        float timer = 0.0f;
        while (UIWindow.activeSelf)
        {
            Vector2 pos = powerScaleLever.anchoredPosition;
            pos.y = amplitude * Mathf.Sin(timer * 2 * Mathf.PI / powerScaleTimePeriod);
            powerScaleLever.anchoredPosition = pos;

            if (pos.y < 0)
            {
                powerScale = 1.0f + (pos.y / amplitude);
            }
            else
            {
                powerScale = 1.0f - (pos.y / amplitude);
            }

            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public float GetPowerScale()
    {
        return powerScale;
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
