using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonVisual : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler ,IPointerDownHandler
{
    Image image => GetComponent<Image>();
    [SerializeField] Sprite[] sprites;

    public void OnPointerDown(PointerEventData eventData)
    {
        image.sprite = sprites[2];
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = sprites[1];
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = sprites[0];
    }
}
