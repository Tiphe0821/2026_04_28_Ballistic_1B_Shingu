using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonIndicator : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerDownHandler, IPointerUpHandler
{
    public GameObject indicatorImage;

    public Sprite clickedSprite;

    private Sprite originalSprite;
    private Image indicatorComponent;

    void Awake()
    {
        if (indicatorImage != null)
        {
            indicatorComponent = indicatorImage.GetComponent<Image>();
            if (indicatorComponent != null)
            {
                originalSprite = indicatorComponent.sprite;
            }
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (indicatorImage != null)
        {
            indicatorImage.SetActive(true);
            if (indicatorComponent != null) indicatorComponent.sprite = originalSprite;
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (indicatorImage != null)
        {
            indicatorImage.SetActive(false);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (indicatorImage != null && clickedSprite != null && indicatorComponent != null)
        {
            indicatorComponent.sprite = clickedSprite; 
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (indicatorImage != null && indicatorComponent != null)
        {
            indicatorComponent.sprite = originalSprite;
        }
    }
}