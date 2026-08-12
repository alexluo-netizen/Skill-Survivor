using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeButtonHover :
    MonoBehaviour,
    IPointerEnterHandler
{
    [SerializeField] private UpgradeUI upgradeUI;
    [SerializeField] private int buttonIndex;

    public void OnPointerEnter(
        PointerEventData eventData)
    {
        upgradeUI.SelectButton(buttonIndex);
    }
}