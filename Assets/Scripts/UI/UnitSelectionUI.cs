using System;
using UnityEngine;

public class UnitSelectionUI : MonoBehaviour{

    [SerializeField] private RectTransform rectTransform;


    private void Start(){
        UnitSelectionManager.instance.OnSelectionAreaStart += UnitSelectionManager_OnSelectionAreaStart;
        UnitSelectionManager.instance.OnSelectionAreaEnd += UnitSelectionManager_OnSelectionAreaEnd;

        rectTransform.gameObject.SetActive(false);
    }

    private void Update(){
        if(rectTransform.gameObject.activeSelf){
            UpdateIndicator();
        }
    }

    private void UnitSelectionManager_OnSelectionAreaStart(object sender, EventArgs e) {
        rectTransform.gameObject.SetActive(true);

        UpdateIndicator();
    }

    private void UnitSelectionManager_OnSelectionAreaEnd(object sender, EventArgs e) {
        rectTransform.gameObject.SetActive(false);
    }

    private void UpdateIndicator(){
        Rect rect = UnitSelectionManager.instance.GetSelectionAreaRect();

        rectTransform.anchoredPosition = rect.position;
        rectTransform.sizeDelta = rect.size;
    }

}
