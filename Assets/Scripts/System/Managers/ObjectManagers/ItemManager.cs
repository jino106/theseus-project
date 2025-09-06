using UnityEngine;
using VContainer;

/// <summary>
/// アイテムの取得を管理するクラス
/// </summary>
public class ItemManager : MonoBehaviour
{
    // InventoryDataの参照
    [Inject] private InventoryData inventoryData;
    // ItemDataの参照を追加
    [Inject] private ItemData itemData;

    // アイテムを取得するメソッド
    public void ObtainItem(int itemID)
    {
        // ItemDataからアイテム情報を取得
        var item = itemData.GetItemByID(itemID);
        if (item == null)
        {
            Debug.LogWarning($"アイテムID {itemID} は ItemData に存在しません。");
            return;
        }

        // アイテムIDに応じてInventoryDataのboolをtrueに設定
        switch (itemID)
        {
            // 研究報告書
            case 1: // 少女の研究報告書
                InventoryData.Instance.SetPlayerReportObtained(true);
                break;
            case 2: // 泥棒の研究報告書
                InventoryData.Instance.SetTheifReportObtained(true);
                break;
            case 3: // マッチョの研究報告書
                InventoryData.Instance.SetMuscleReportObtained(true);
                break;
            case 4: // 消防士の研究報告書
                InventoryData.Instance.SetFireReportObtained(true);
                break;
            case 5: // アサシンの研究報告書
                InventoryData.Instance.SetAssassinReportObtained(true);
                break;

            // 日記
            case 6: // 少女の日記
                InventoryData.Instance.SetPlayerDiaryObtained(true);
                break;
            case 7: // 泥棒の日記
                InventoryData.Instance.SetTheifDiaryObtained(true);
                break;
            case 8: // マッチョの日記
                InventoryData.Instance.SetMuscleDiaryObtained(true);
                break;
            case 9: // 消防士の日記
                InventoryData.Instance.SetFireDiaryObtained(true);
                break;
            case 10: // アサシンの日記
                InventoryData.Instance.SetAssassinDiaryObtained(true);
                break;

            default:
                Debug.LogWarning($"対応していないアイテムID: {itemID}");
                return;
        }

        // ItemDataから取得した正確なアイテム名でログ出力
        Debug.Log($"アイテムを取得しました: {item.name} (ID: {itemID})");
    }

    // アイテムが取得済みかどうかを確認するメソッド
    public bool IsItemObtained(int itemID)
    {
        switch (itemID)
        {
            case 1: return InventoryData.Instance.PlayerReportObtained;
            case 2: return InventoryData.Instance.TheifReportObtained;
            case 3: return InventoryData.Instance.MuscleReportObtained;
            case 4: return InventoryData.Instance.FireReportObtained;
            case 5: return InventoryData.Instance.AssassinReportObtained;
            case 6: return InventoryData.Instance.PlayerDiaryObtained;
            case 7: return InventoryData.Instance.TheifDiaryObtained;
            case 8: return InventoryData.Instance.MuscleDiaryObtained;
            case 9: return InventoryData.Instance.FireDiaryObtained;
            case 10: return InventoryData.Instance.AssassinDiaryObtained;
            default:
                Debug.LogWarning($"対応していないアイテムID: {itemID}");
                return false;
        }
    }
}
