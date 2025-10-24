using UnityEngine;
using VContainer;
using Cysharp.Threading.Tasks;


public enum PartsOwnerType
{
    Player,
    Thief,
    Muscle,
    Fire,
    Assassin
}

public class CollectibleItem : MonoBehaviour
{
    /// <summary>
    /// コレクター要素のあるアイテムの取得に関するスクリプト
    /// </summary>

    //アイテムのID(外部キーの役割)
    [SerializeField] private int itemID;

    // ItemManagerの参照
    [Inject] private ItemManager itemManager;
    // ItemDataの参照 
    [Inject] private ItemData itemData;
    // PlayerPartsRatioの参照
    [Inject] private PlayerPartsRatio partsRatio;
    // GameTextDisplayの参照
    [Inject] private GameTextDisplay gameTextDisplay;

    // アイテム名
    private string itemName;
    // アイテムの説明文
    private string itemDescription;
    // アイテムのスプライト
    private string itemText;

    // アイテムを取得情報をインベントリに保存するメソッド
    public void CollectItem()
    {
        itemManager.ObtainItem(itemID);
    }

    // アイテム名を取得するメソッド
    public string GetItemName()
    {
        return itemData.GetItemNameByID(itemID);
    }

    // アイテムのテキストを表示するメソッド
    public string GetItemText()
    {
        return itemData.GetItemTextByID(itemID);
    }

    // プレイヤーが触れた時の処理など
    async void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"取得したアイテム: {GetItemName()}");
            Debug.Log($"アイテムテキスト: {GetItemText()}");

            var item = itemData.GetItemByID(itemID);
            var descriptions = item?.descriptions;
            if (item == null || descriptions == null)
            {
                Debug.LogWarning("アイテム情報または説明文がありません");
                return;
            }

            // 最大占有率のキャラを取得
            var maxPartsChara = (PartsOwnerType)partsRatio.GetDominantParts();
            float maxRatio = partsRatio.GetPartsRatio((PartsOwnerType)maxPartsChara);

            string description = "";

            // 100%かつアイテム所有者と一致
            if (Mathf.Approximately(maxRatio, 100f) && maxPartsChara.ToString() == item.ownerType.ToString())
            {
                description = descriptions.ownFullTone;
            }
            else
            {
                // 最大占有率キャラの口調
                switch (maxPartsChara)
                {
                    case PartsOwnerType.Player:
                        description = descriptions.playerTone;
                        break;
                    case PartsOwnerType.Thief:
                        description = descriptions.theifTone;
                        break;
                    case PartsOwnerType.Muscle:
                        description = descriptions.muscleTone;
                        break;
                    case PartsOwnerType.Fire:
                        description = descriptions.fireTone;
                        break;
                    case PartsOwnerType.Assassin:
                        description = descriptions.assassinTone;
                        break;
                    default:
                        description = descriptions.playerTone;
                        break;
                }
            }

            Debug.Log($"説明文: {description}");

            CollectItem();

            if (gameTextDisplay != null)
            {
                await gameTextDisplay.ShowText(description);
            }

            Destroy(gameObject);
        }
    }
}
