using UnityEngine;
using DG.Tweening;

public class RadioButton : MonoBehaviour, IClickable
{
    [SerializeField] private RadioButtonType buttonType;
    [SerializeField] private float currentYPosition;
    [SerializeField] private float targetYPosition;
    [SerializeField] private float duration = 0.2f;

    public RadioButtonType OnClicked()
    {
        return buttonType;
    }

    public void SetPressed(bool pressed)
    {
        if (pressed)
        {
            // Bas�l� duruma getir
            transform.DOLocalMoveY(targetYPosition, duration);
        }
        else
        {
            // Normal duruma getir
            transform.DOLocalMoveY(currentYPosition, duration);
        }
    }
}
