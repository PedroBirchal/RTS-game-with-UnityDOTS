using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonMenu : MonoBehaviour, IPointerEnterHandler
{
    public MenuSelector menuSelector;

    public void OnPointerEnter(PointerEventData eventData)
    {
        menuSelector.MoverParaBotao(gameObject);
    }
}
