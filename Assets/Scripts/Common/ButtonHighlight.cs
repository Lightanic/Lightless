using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Used in main menu
// Used to get whether a button is highlighted or not
public class ButtonHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public bool isHighlighted;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHighlighted = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isHighlighted = false;
    }
    public void OnSelect(BaseEventData eventData)
    {
        isHighlighted = true;
    }
    public void OnDeselect(BaseEventData eventData)
    {
        isHighlighted = false;
    }
}