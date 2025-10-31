using UnityEngine;

public class ItemReset : Button
{
    /// <summary>
    /// コレクトアイテムの進捗をリセットする
    /// </summary>

    // 確認画面の取得
    [SerializeField] private ItemResetConfirmation resetConfirm;

    public override void OnClick()
    {
        base.OnClick(); // 決定音を鳴らす
        resetConfirm.ShowConfirmationDialog();
    }
}
