using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSelector : MonoBehaviour
{
    public RectTransform dedoImage;

    public void MoverParaBotao(GameObject botao)
    {
        Vector3 pos = botao.transform.position;
        pos.x -= 125f; 
        dedoImage.position = pos;
    }
}