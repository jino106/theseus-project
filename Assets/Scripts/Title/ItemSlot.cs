using UnityEngine;
using UnityEngine.UI;
using VContainer;
using System.Linq;

public class ItemSlot : Button
{
    [SerializeField] int itemID;
    [SerializeField] ItemData itemData;
    [SerializeField] InventoryData inventoryData;
    private Image iconImage;

    public string ItemName => itemData.GetItemNameByID(itemID);
    public string ItemText => itemData.GetItemTextByID(itemID);
    public bool IsObtained => inventoryData.GetItemObtained(itemID);

    public void Initialize()
    {
        iconImage = GetComponentsInChildren<Image>(true)
            .FirstOrDefault(img => img.gameObject.name == "IconImage");

        if (iconImage == null)
        {
            Debug.LogError("IconImageが見つかりません。ItemSlotの子にIconImageという名前のImageを配置してください。");
        }
        UpdateIcon();
    }

    protected void Start()
    {
        iconImage = GetComponentsInChildren<Image>(true)
            .FirstOrDefault(img => img.gameObject.name == "IconImage");

        if (iconImage == null)
        {
            Debug.LogError("IconImageが見つかりません。ItemSlotの子にIconImageという名前のImageを配置してください。");
        }
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        if (iconImage == null)
        {
            Debug.LogError("iconImageがnullです");
            return;
        }
        if (inventoryData == null)
        {
            Debug.LogError("inventoryDataがnullです");
            return;
        }
        if (itemData == null)
        {
            Debug.LogError("itemDataがnullです");
            return;
        }

        if (IsObtained)
        {
            iconImage.sprite = itemData.GetItemIconByID(itemID);
            iconImage.enabled = true;
        }
        else
        {
            iconImage.enabled = false;
        }
    }

    public override void OnClick()
    {
        if (IsObtained)
        {
            Debug.Log($"Item: {ItemName}\nText: {ItemText}");
        }
        else
        {
            Debug.Log("Item not obtained yet.");
        }
    }

    public override void TriggerSelectionEffects()
    {
        // 選択された際のエフェクト処理をここに追加
        Debug.Log($"ItemSlot for ItemID {itemID} selected.");
    }
}
