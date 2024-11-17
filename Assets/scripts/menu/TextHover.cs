using UnityEngine;
using UnityEngine.EventSystems;

public class TextHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    //Text that gets replaced when we hover the button
    public GameObject OriginalText;

    //Text that appears when we hover the button
    public GameObject HoverText;

    //Triggers when we hover
    public void OnPointerEnter(PointerEventData eventData)
    {
        OriginalText.SetActive(false);
        HoverText.SetActive(true);
    }

    //Triggers when we are not hovering
    public void OnPointerExit(PointerEventData eventData)
    {
        HoverText.SetActive(false);
        OriginalText.SetActive(true);
    }
}
